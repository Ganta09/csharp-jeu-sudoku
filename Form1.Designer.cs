namespace sudoku_v1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            sudoku_grille = new Panel();
            SuspendLayout();
            // 
            // sudoku_grille
            // 
            sudoku_grille.Location = new Point(348, 87);
            sudoku_grille.Name = "sudoku_grille";
            sudoku_grille.Size = new Size(512, 512);
            sudoku_grille.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(34, 47, 62);
            ClientSize = new Size(1264, 681);
            Controls.Add(sudoku_grille);
            Name = "Form1";
            Text = "Sudoku - Tristan Cunha";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel sudoku_grille;
    }
}
