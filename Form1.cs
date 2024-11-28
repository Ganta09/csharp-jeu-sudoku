namespace sudoku_v1
{
    public partial class Form1 : Form
    {
        private Sudoku sudoku; // Déclaration de sudoku au niveau de la classe

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

        private void OnNumberButtonClick(int number)
        {
            sudoku.HandleNumberInput(number, sudoku_grille);
        }
    }
}
