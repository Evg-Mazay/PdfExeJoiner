
namespace PdfExeJoinerWinForms.Joiners
{
    public interface IFileJoiner
    {
        void Join(string filename1, string filename2, string outputFilename);

        bool CanJoin(string filename1, string filename2, out string errorDescription);
    }
}