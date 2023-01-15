using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfExeJoinerWinForms.Joiners;

namespace PdfExeJoinerWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var joiner = new PdfExeJoiner();
                joiner.Join(PdfFilename.Text, ExeFilename.Text, OutputFIlename.Text);
                MessageBox.Show("Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void SelectPdfButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var joiner = new PdfExeJoiner();
                    if (joiner.CanJoin(openFileDialog.FileName, "", out string errorDescription))
                    {
                        PdfFilename.Text = openFileDialog.FileName;
                    }
                    else
                    {
                        MessageBox.Show(errorDescription, "Pdf Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectExeButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExeFilename.Text = openFileDialog.FileName;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectOutputButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog {FileName = "output.exe.pdf"};
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bool hasExtension = saveFileDialog.FileName.EndsWith(".exe.pdf")
                                        || saveFileDialog.FileName.EndsWith(".exe")
                                        || saveFileDialog.FileName.EndsWith(".pdf");
                    OutputFIlename.Text = hasExtension ? saveFileDialog.FileName : saveFileDialog.FileName + ".exe.pdf";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = @"
Select Pdf and Exe files, and click 'Join' to combine them
--------------------------------------------------------------
The output file is simultaneously valid exe and (almost) valid pdf.
Rename output file to .exe to open it as application.
Rename output file to .pdf to open in as document.
--------------------------------------------------------------
PDF MUST BE WITHOUT LINEARIZATION OR INCREMENTAL SAVING!
You can make pdf compatible by using online tool like 'compress pdf online'.
";
        }
    }
}