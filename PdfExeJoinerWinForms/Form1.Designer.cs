namespace PdfExeJoinerWinForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.PdfFilename = new System.Windows.Forms.TextBox();
            this.ExeFilename = new System.Windows.Forms.TextBox();
            this.OutputFIlename = new System.Windows.Forms.TextBox();
            this.SelectOutputButton = new System.Windows.Forms.Button();
            this.SelectExeButton = new System.Windows.Forms.Button();
            this.SelectPdfButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(278, 310);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(248, 90);
            this.button1.TabIndex = 0;
            this.button1.Text = "Join";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PdfFilename
            // 
            this.PdfFilename.Location = new System.Drawing.Point(46, 62);
            this.PdfFilename.Name = "PdfFilename";
            this.PdfFilename.Size = new System.Drawing.Size(557, 26);
            this.PdfFilename.TabIndex = 1;
            // 
            // ExeFilename
            // 
            this.ExeFilename.Location = new System.Drawing.Point(46, 154);
            this.ExeFilename.Name = "ExeFilename";
            this.ExeFilename.Size = new System.Drawing.Size(557, 26);
            this.ExeFilename.TabIndex = 2;
            // 
            // OutputFIlename
            // 
            this.OutputFIlename.Location = new System.Drawing.Point(46, 250);
            this.OutputFIlename.Name = "OutputFIlename";
            this.OutputFIlename.Size = new System.Drawing.Size(557, 26);
            this.OutputFIlename.TabIndex = 3;
            this.OutputFIlename.Text = "output.exe.pdf";
            // 
            // SelectOutputButton
            // 
            this.SelectOutputButton.Location = new System.Drawing.Point(629, 250);
            this.SelectOutputButton.Name = "SelectOutputButton";
            this.SelectOutputButton.Size = new System.Drawing.Size(38, 32);
            this.SelectOutputButton.TabIndex = 4;
            this.SelectOutputButton.Text = ". . .";
            this.SelectOutputButton.UseVisualStyleBackColor = true;
            this.SelectOutputButton.Click += new System.EventHandler(this.SelectOutputButton_Click);
            // 
            // SelectExeButton
            // 
            this.SelectExeButton.Location = new System.Drawing.Point(629, 155);
            this.SelectExeButton.Name = "SelectExeButton";
            this.SelectExeButton.Size = new System.Drawing.Size(38, 32);
            this.SelectExeButton.TabIndex = 5;
            this.SelectExeButton.Text = ". . .";
            this.SelectExeButton.UseVisualStyleBackColor = true;
            this.SelectExeButton.Click += new System.EventHandler(this.SelectExeButton_Click);
            // 
            // SelectPdfButton
            // 
            this.SelectPdfButton.Location = new System.Drawing.Point(629, 63);
            this.SelectPdfButton.Name = "SelectPdfButton";
            this.SelectPdfButton.Size = new System.Drawing.Size(38, 32);
            this.SelectPdfButton.TabIndex = 6;
            this.SelectPdfButton.Text = ". . .";
            this.SelectPdfButton.UseVisualStyleBackColor = true;
            this.SelectPdfButton.Click += new System.EventHandler(this.SelectPdfButton_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(32, 423);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(666, 260);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(46, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(244, 37);
            this.label2.TabIndex = 8;
            this.label2.Text = "output filename";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(46, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(244, 37);
            this.label3.TabIndex = 9;
            this.label3.Text = "EXE filename";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(46, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(244, 37);
            this.label4.TabIndex = 10;
            this.label4.Text = "PDF filename";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 700);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectPdfButton);
            this.Controls.Add(this.SelectExeButton);
            this.Controls.Add(this.SelectOutputButton);
            this.Controls.Add(this.OutputFIlename);
            this.Controls.Add(this.ExeFilename);
            this.Controls.Add(this.PdfFilename);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "PdfExeJoiner v1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.TextBox PdfFilename;
        private System.Windows.Forms.TextBox ExeFilename;
        private System.Windows.Forms.TextBox OutputFIlename;
        private System.Windows.Forms.Button SelectOutputButton;
        private System.Windows.Forms.Button SelectExeButton;
        private System.Windows.Forms.Button SelectPdfButton;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button button1;

        #endregion
    }
}