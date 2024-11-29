// Sudoku basique par Tristan Cunha

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace sudoku_v1
{
    internal class Sudoku
    {
        // mon tableau
        private int[,] grid, solutionGrid;
        private Label selectedCell = null; // Stocke la case sélectionnée
        private List<(int, int)> errorCells;
        public Sudoku()
        {
            // Initialisation d'une grille vide
            grid = new int[9, 9];
            solutionGrid = new int[9, 9]; // Grille contenant la solution, on comparera les input utilisateurs a cette grille
            errorCells = new List<(int, int)>(); // Initialisation de la liste des erreurs

        }
        public int[,] GetGrid() 
        {
            return grid;
        }
        public bool IsValidPlacement(int row, int col, int num)
        {
            // Vérifie la ligne
            for (int i = 0; i < 9; i++)
            {
                if (grid[row, i] == num)
                    return false;
            }

            // Vérifie la colonne
            for (int i = 0; i < 9; i++)
            {
                if (grid[i, col] == num)
                    return false;
            }

            // Vérifie le sous-bloc 3x3
            int startRow = (row / 3) * 3;
            int startCol = (col / 3) * 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (grid[startRow + i, startCol + j] == num)
                        return false;
                }
            }

            // Placement valide
            return true;
        }
        public bool GenerateFullGrid()
        {
            // Création d'une liste des chiffres de 1 à 9
            List<int> numbers = Enumerable.Range(1, 9).ToList();
            Random rand = new Random();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        // Mélanger l'ordre des chiffres
                        numbers = numbers.OrderBy(x => rand.Next()).ToList();

                        foreach (int num in numbers)
                        {
                            if (IsValidPlacement(row, col, num))
                            {
                                grid[row, col] = num;

                                // Appel récursif
                                if (GenerateFullGrid())
                                    return true;

                                // Backtrack
                                grid[row, col] = 0;
                            }
                        }

                        return false; // Si aucun chiffre ne fonctionne, backtrack
                    }
                }
            }
            return true; // Grille complète
        }
        public void DisplayInPanel(Panel panel)
        {
            panel.Controls.Clear(); // Nettoyer le panel pour éviter un affichage en double
            int cellSize = 50; // Taille de chaque cellule
            // Ajouter des lignes horizontales épaisses entre les blocs 3x3
            for (int i = 1; i < 3; i++) // Deux lignes pour séparer les blocs
            {
                Panel horizontalBorder = new Panel
                {
                    Width = 9 * cellSize,
                    Height = 4, // Épaisseur de la ligne
                    BackColor = Color.Black,
                    Location = new Point(0, i * 3 * cellSize - 2) // Ajustez le "-2" si nécessaire.
                };
                panel.Controls.Add(horizontalBorder);
            }

            // Ajoute des lignes verticales épaisses entre les blocs 3x3
            for (int i = 1; i < 3; i++) // Deux colonnes pour séparer les blocs
            {
                Panel verticalBorder = new Panel
                {
                    Width = 4, // Épaisseur de la ligne
                    Height = 9 * cellSize,
                    BackColor = Color.Black,
                    Location = new Point(i * 3 * cellSize - 2, 0) // Position des bordures
                };
                panel.Controls.Add(verticalBorder);
            }
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Label cell = new Label
                    {
                        Text = grid[row, col] == 0 ? "" : grid[row, col].ToString(),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Width = cellSize,
                        Height = cellSize,
                        Location = new Point(col * cellSize, row * cellSize),
                        Font = new Font("Arial", 14, FontStyle.Bold),
                        BackColor = Color.FromArgb(223,230,233),
                        BorderStyle = BorderStyle.FixedSingle, // Bordure classique pour chaque cellule
                    };

                    // Ajouter les coordonnées de la cellule comme "Tag" pour récupérer plus tard
                    cell.Tag = new { Row = row, Col = col, IsMutable = grid[row, col] == 0 };

                    // Ajouter un gestionnaire de clic à la cellule
                    cell.Click += (sender, args) => OnCellClick(sender, panel);

                    // Ajouter la cellule au panel
                    panel.Controls.Add(cell);
                }
            }

            
        }



        private void OnCellClick(object sender, Panel panel)
        {
            Label clickedCell = sender as Label;

            if (clickedCell != null)
            {
                // Récupérer les informations de la cellule
                var cellInfo = (dynamic)clickedCell.Tag;

                // Vérifier si la cellule est mutable
                if (!cellInfo.IsMutable)
                {
                    Debug.WriteLine("Cette case n'est pas modifiable.");
                    return;
                }

                // Réinitialiser la couleur de la cellule précédemment sélectionnée
                if (selectedCell != null)
                {
                    var prevCellInfo = (dynamic)selectedCell.Tag;

                    if (!errorCells.Contains((prevCellInfo.Row, prevCellInfo.Col)))
                    {
                        selectedCell.BackColor = Color.FromArgb(223, 230, 233);
                    }
                }
                if (!errorCells.Contains((cellInfo.Row, cellInfo.Col)))
                {
                    // Surligner la cellule cliquée uniquement si elle n'est pas marquée en rouge
                    clickedCell.BackColor = Color.FromArgb(18, 137, 167);
                }
                // Mémoriser la cellule sélectionnée
                selectedCell = clickedCell;

            }
        }




        public void HideCellsForPuzzle(int minPerNumber = 4, int maxPerNumber = 6)
        {
            // Copie de la grille complète avant de masquer
            Array.Copy(grid, solutionGrid, grid.Length);

            Random rand = new Random();

            // Parcourir chaque chiffre de 1 à 9
            for (int num = 1; num <= 9; num++)
            {
                List<(int row, int col)> positions = new List<(int, int)>();
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        if (grid[row, col] == num)
                        {
                            positions.Add((row, col));
                        }
                    }
                }

                positions = positions.OrderBy(x => rand.Next()).ToList();

                int keepCount = rand.Next(minPerNumber, maxPerNumber + 1);

                for (int i = keepCount; i < positions.Count; i++)
                {
                    (int row, int col) = positions[i];
                    grid[row, col] = 0;
                }
            }
        }
        /* private void SelectCell(object sender)
         {
             if (sender is Label clickedCell)
             {
                 // Réinitialiser la couleur de la cellule précédemment sélectionnée
                 if (selectedCell != null && !errorCells.Contains(((int, int))selectedCell.Tag))
                 {
                     selectedCell.BackColor = Color.White;
                 }

                 // Surligner la cellule cliquée
                 clickedCell.BackColor = Color.Lavender;

                 // Mémoriser la cellule sélectionnée
                 selectedCell = clickedCell;
             }
         }*/
        public bool HandleNumberInput(int number, Panel panel)
        {
            if (selectedCell != null)
            {
                // Récupérer les informations de la cellule depuis le Tag avec un cast dynamique
                var cellInfo = (dynamic)selectedCell.Tag;

                int row = cellInfo.Row;
                int col = cellInfo.Col;

                // Vérifier si le chiffre est correct
                if (solutionGrid[row, col] == number)
                {
                    selectedCell.Text = number.ToString();
                    selectedCell.BackColor = Color.LightGreen; // Surligner en vert
                    errorCells.Remove((row, col)); // Retirer de la liste des erreurs si corrigé


                    // Marquer la cellule comme immuable
                    selectedCell.Tag = new { Row = row, Col = col, IsMutable = false };
                    return true; // Bonne réponse

                }
                else
                {
                    selectedCell.Text = number.ToString();
                    selectedCell.BackColor = Color.LightCoral; // Surligner en rouge
                    if (!errorCells.Contains((row, col)))
                    {
                        errorCells.Add((row, col)); // Ajouter à la liste des erreurs
                    }
                    return false; // Mauvaise réponse

                }
            }
            return false; // Mauvaise réponse

        }



    }
}
