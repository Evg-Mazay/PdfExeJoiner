using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using org.pdfclown.bytes;
using org.pdfclown.tokens;
using Encoding = System.Text.Encoding;
using Stream = System.IO.Stream;

namespace PdfExeJoinerWinForms.Joiners
{
        public class PdfExeJoiner : IFileJoiner
    {
        /// <summary>
        /// Объединить .pdf и .exe
        /// РАБОТАЕТ ТОЛЬКО ДЛЯ PDF версии 1.4 и ниже, БЕЗ INCREMENTAL SAVING, ЛИНЕАРИЗАЦИИ и ШИФРОВАНИЯ!
        /// </summary>
        private struct XrefEntry
        {
            public int number;
            public long offset;
            public int generation;
            public string usage;
        }
        
        private byte[] PdfStreamEnd =>
            Encoding.ASCII.GetBytes("\nendstream" +
                                    "\nendobj\n");

        private byte[] MakeExeHeaderAndPdfStreamStart(int objNumber, long length, out int objOffset)
        {
            int resultLength = 57; // Размер результата функции известен заранее (выравнивание), он используется в рассчете length
            string header = "MZ%PDF-1.1\n";
            string objLine = $"{objNumber} 0 obj";

            objOffset = Encoding.ASCII.GetBytes(header).Length;

            long streamLength = length - resultLength;

            return Encoding.ASCII.GetBytes(
                header +
                $"{objLine,-15}\n" +
                $"<</Length {streamLength,-10}>>\n" +
                $"stream\n");
        }

        private byte[] MakeXrefWithAddedObject(
            IReadOnlyCollection<XrefEntry> originalXrefTable, 
            long firstObjectOffsetNow,
            long xrefOffsetDiff)
        {
            long originalFirstObjectOffset = originalXrefTable.OrderBy(x => x.offset).First().offset;

            string result = $"xref\n" +
                            $"0 {originalXrefTable.Count + 2}\n" +
                            $"0000000000 65535 f \n";
            
            long firstObjectOffsetDiff = originalFirstObjectOffset - firstObjectOffsetNow;
            foreach (var xrefEntry in originalXrefTable.OrderBy(x => x.number))
            {
                long newOffset = xrefEntry.offset + xrefOffsetDiff - firstObjectOffsetDiff;
                result += $"{newOffset:D10} {xrefEntry.generation:D5} {xrefEntry.usage} \n";
            }
            
            result += $"{firstObjectOffsetNow:D10} 00000 n \n";

            return Encoding.ASCII.GetBytes(result);
        }

        private byte[] MakeTrailer(long xrefOffset, int rootObjectNumber, int rootGenerationNumber)
        {
            string result = $"trailer\n" +
                            $"<< /Root {rootObjectNumber} {rootGenerationNumber} R >>\n" +
                            $"startxref\n" +
                            $"{xrefOffset}\n" +
                            $"%%EOF";

            return Encoding.ASCII.GetBytes(result);
        }

        public void Join(string filename1, string filename2, string outputFilename)
        {
            string pdfFilename;
            string exeFilename;
            if (filename1.EndsWith(".pdf") && filename2.EndsWith(".exe"))
            {
                pdfFilename = filename1;
                exeFilename = filename2;
            }
            else if (filename1.EndsWith(".exe") && filename2.EndsWith(".pdf"))
            {
                pdfFilename = filename2;
                exeFilename = filename1;
            }
            else
            {
                throw new ArgumentException("one filename should end with .pdf, and another should end with .exe");
            }

            using (Stream exeStream = new FileStream(exeFilename, FileMode.Open))
            {
                using (Stream outputStream = new FileStream(outputFilename, FileMode.Create))
                {
                    // Прочитать xref
                    List<XrefEntry> originalXrefTable = new List<XrefEntry>();
                    long originalXrefOffset;
                    int originalRootObjectNumber;
                    int originalRootGenerationNumber;

                    using (Stream pdfStream = new FileStream(pdfFilename, FileMode.Open))
                    {
                        using (org.pdfclown.files.File pdfFile =
                               new org.pdfclown.files.File(new org.pdfclown.bytes.Stream(pdfStream)))
                        {
                            originalXrefOffset = pdfFile.Reader.Parser.RetrieveXRefOffset();
                            originalRootObjectNumber = pdfFile.Document.BaseObject.Reference.ObjectNumber;
                            originalRootGenerationNumber = pdfFile.Document.BaseObject.Reference.GenerationNumber;

                            if (pdfFile.IndirectObjects.Count < 2)
                            {
                                throw new ArgumentException("no objects in pdf");
                            }

                            for (int i = 1; i < pdfFile.IndirectObjects.Count; i++)
                            {
                                originalXrefTable.Add(new XrefEntry()
                                {
                                    number = pdfFile.IndirectObjects[i].XrefEntry.Number,
                                    offset = pdfFile.IndirectObjects[i].XrefEntry.Offset,
                                    generation = pdfFile.IndirectObjects[i].XrefEntry.Generation,
                                    usage = pdfFile.IndirectObjects[i].XrefEntry.Usage == XRefEntry.UsageEnum.Free
                                        ? "f"
                                        : "n"
                                });
                            }

                            originalXrefTable.Sort((entry1, entry2) => entry1.offset.CompareTo(entry2.offset));
                        }
                    }


                    // Записать комбинированный заголовок exe + pdf. И записать начало стрима pdf
                    var newHeader = MakeExeHeaderAndPdfStreamStart(originalXrefTable.Count + 1, exeStream.Length,
                        out int firstObjOffset);
                    outputStream.Write(newHeader, 0, newHeader.Length);
                    if (outputStream.Position >= 0x3C)
                    {
                        throw new ArgumentException("exe or pdf is too long, sorry");
                    }

                    // Добить выходной файл до позиции 0x3C
                    for (long i = outputStream.Position; i < 0x3C; i++)
                    {
                        outputStream.WriteByte(0x00);
                    }

                    // Вставить весь exe, начиная с 0x3C
                    exeStream.Seek(0x3C, SeekOrigin.Begin);
                    int currentByte;
                    while ((currentByte = exeStream.ReadByte()) != -1)
                    {
                        outputStream.WriteByte((byte)currentByte);
                    }

                    // Вставить PdfStreamEnd
                    outputStream.Write(PdfStreamEnd, 0, PdfStreamEnd.Length);

                    long newXrefOffsetDiff = outputStream.Position - firstObjOffset;

                    // Вставить объекты pdf
                    using (Stream pdfStream = new FileStream(pdfFilename, FileMode.Open))
                    {
                        pdfStream.Seek(originalXrefTable[0].offset, SeekOrigin.Begin);
                        for (long i = originalXrefTable[0].offset; i < originalXrefOffset; i++)
                        {
                            currentByte = pdfStream.ReadByte();
                            outputStream.WriteByte((byte)currentByte);
                        }
                    }

                    // Вставить xref с правильными оффсетами
                    long startxref = outputStream.Position;
                    var newXref = MakeXrefWithAddedObject(originalXrefTable, firstObjOffset, newXrefOffsetDiff);
                    outputStream.Write(newXref, 0, newXref.Length);

                    // Вставить trailer
                    var newTrailer = MakeTrailer(startxref, originalRootObjectNumber, originalRootGenerationNumber);
                    outputStream.Write(newTrailer, 0, newTrailer.Length);

                }
            }

            /* УСТАРЕВШИЙ КОММЕНТАРИЙ
             * -1. combinedHeaderLen = Заранее понять сколько займет комбинированное начало exe+pdf, это константа
             * 0.0. exeLen = Посчитать длину exe
             * 0.1. xrefPos = Прочитать последние 1024 байта pdf - найти startxref
             * 0.2. xref = Прочитать xref, запомнить все оффсеты и количество
             * 1. Начать выходной файл с MZ + заголовок pdf
             * 2. Вставить начало объекта стрима (номер = count(xref) + 1). Length = exeLen - combinedHeaderLen.
             *    Сбалансировать пробелами так, чтобы независимо от длины exe и номера стрима,
             *      мы в данный момент оказались в точке combinedHeaderLen.
             * 3. Вставить exe полностью
             * 4. Вставить конец объекта стрима
             * 5. Вставить pdf начиная с первого объекта до xrefPos. Запоминать при каждом встреченном объекте его оффсет.
             * 6. Вставить xref, записав туда правильные оффсеты
             * 7. Вставить pdf, начиная с .trailer. Записать правильный startXref
             */
        }

        public bool CanJoin(string filename1, string filename2, out string errorDescription)
        {
            string pdfFilename;
            if (filename1.EndsWith(".pdf"))
            {
                pdfFilename = filename1;
            }
            else if (filename2.EndsWith(".pdf"))
            {
                pdfFilename = filename2;
            }
            else
            {
                errorDescription = "select valid pdf (filename should end with .pdf)";
                return false;
            }

            try
            {
                using (Stream pdfStream = new FileStream(pdfFilename, FileMode.Open))
                {
                    using (org.pdfclown.files.File pdfFile = new org.pdfclown.files.File(new org.pdfclown.bytes.Stream(pdfStream)))
                    {
                    
                    }   
                }
            }
            catch (Exception e)
            {
                errorDescription = e.Message;
                return false;
            }

            errorDescription = "";
            return true;
        }
    }
}