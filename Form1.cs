namespace sudoku_v1
{
    public partial class Form1 : Form
    {
        private Sudoku sudoku; 
        private TextBox errorTextBox;
        bool correct;
        byte errorCount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sudoku = new Sudoku();

            // Génération de la grille
            sudoku.GenerateFullGrid();

            sudoku.HideCellsForPuzzle();

            // Affichage dans la console
            sudoku.DisplayInPanel(sudoku_grille);
            CreateNumberButtons();
            CreateErrorTextBox(); // Ajout de la TextBox pour les erreurs


        }
        private void CreateNumberButtons()
        {
            int buttonSize = 50; // Taille des boutons
            int offsetX = 10;    // Décalage horizontal
            int offsetY = 600;   // Position verticale (sous la grille)

            for (int i = 1; i <= 9; i++)
            {
                int number = i; // Crée une copie locale de i

                Button numberButton = new Button
                {
                    Text = number.ToString(),
                    Width = buttonSize,
                    Height = buttonSize,
                    BackColor = Color.FromArgb(87,88,187),
                    Location = new Point(offsetX + (number - 1) * (buttonSize + 5), offsetY), // Position des boutons
                    Font = new Font("Roboto", 14, FontStyle.Bold),
                    ForeColor = Color.White
                };

                // Ajouter un gestionnaire de clic
                numberButton.Click += (sender, args) => OnNumberButtonClick(number);

                // Ajouter le bouton au formulaire
                this.Controls.Add(numberButton);
            }
        }
        private void CreateErrorTextBox()
        {
            errorTextBox = new TextBox
            {
                Text = "Erreurs : 0",
                Width = 200,
                Height = 30,
                Location = new Point(10, 560), // Position en dessous de la grille
                Font = new Font("Roboto", 12, FontStyle.Bold),
                ReadOnly = true, // Empêche l'édition par l'utilisateur
                TextAlign = HorizontalAlignment.Center,
                BackColor = Color.FromArgb(223, 230, 233)
            };
            this.Controls.Add(errorTextBox);
        }
        private void OnNumberButtonClick(int number)
        {
           correct = sudoku.HandleNumberInput(number, sudoku_grille);
            if (!correct)
            {
                errorCount++; // Incrémente le compteur d'erreurs
                errorTextBox.Text = $"Erreurs : {errorCount}";

                if (errorCount >= 3)
                {
                    ShowGameOverDialog(); // Affiche la fenêtre de défaite
                }
            }
        }
        private void ShowGameOverDialog()
        {
            DialogResult result = MessageBox.Show(
                "Vous avez perdu ! Voulez-vous réessayer ou quitter ?",
                "Défaite",
                MessageBoxButtons.RetryCancel,
                MessageBoxIcon.Information
            );

            if (result == DialogResult.Retry)
            {
                Reset(); // Réinitialise le jeu
            }
            else if (result == DialogResult.Cancel)
            {
                this.Close(); // Ferme l'application
            }
        }
        private void Reset()
        {
            errorCount = 0;
            errorTextBox.Text = "Erreurs : 0";

            sudoku = new Sudoku();
            sudoku.GenerateFullGrid();
            sudoku.HideCellsForPuzzle();
            sudoku.DisplayInPanel(sudoku_grille);
        }
    }
}
