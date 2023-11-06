using System;
using System.Text;

namespace MinesweeperGame
{
    public class Grid
    {
        private int _rows;
        private int _columns;
        private int _mines;
        private Cell[,] _cells;  // Une grille 2D de Cell
        public string Difficulty { get; private set; }

        public Grid(int rows, int columns, int mines)
        {
            _rows = rows;
            _columns = columns;
            _mines = mines;
            _cells = new Cell[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _cells[i, j] = new Cell();
                }
            }
        }

        // Initialise la grille avec des mines aléatoires
        public void Initialize()
        {
            // Placer les mines de manière aléatoire
            PlaceMines();

            // Calculer les numéros pour les cellules adjacentes
            CalculateAdjacentMines();
        }



        // Affiche la grille actuelle à l'utilisateur
        public void Display(bool gameOver = false)
        {
            StringBuilder display = new StringBuilder();

            // Add the column headers with padding for double-digit numbers
            display.Append("   "); // Adjust the spacing based on your index column size
            for (int i = 0; i < _columns; i++)
            {
                display.AppendFormat("{0,2} ", i); // {0,2} formats the number with a fixed width of 2
            }
            display.AppendLine();

            // Add the row separator, adjust the number of dashes based on your grid width
            display.Append("  ");
            display.Append('-', 3 * _columns); // Assumes 3 characters per grid cell
            display.AppendLine();

            // Add each row of the grid
            for (int i = 0; i < _rows; i++)
            {
                // Add the row number with padding for double-digit numbers
                display.AppendFormat("{0,2}|", i);

                for (int j = 0; j < _columns; j++)
                {
                    // Use the updated Display method of Cell
                    _cells[i, j].Display(display, gameOver);
                }

                // Start a new line at the end of each row
                display.AppendLine();
            }

            // Print the whole grid
            Console.Write(display.ToString());
        }


        //old version, grid directlly in console
        // Affiche la grille actuelle à l'utilisateur
        //public void Display(bool gameOver = false)
        //{
        //    // Affichez chaque cellule de la grille
        //    for (int i = 0; i < _rows; i++)
        //    {
        //        for (int j = 0; j < _columns; j++)
        //        {
        //            _cells[i, j].Display(gameOver);
        //        }
        //        Console.WriteLine();  // nouvelle ligne pour chaque rangée
        //    }
        //}


        /// <summary>
        /// Recuperer la difficulté choisise en MainProgram
        /// </summary>
        /// <returns>
        /// The difficulty as String
        /// </returns>
        public string GetDifficulty()
        {
            return Difficulty;
        }

        // Place aléatoirement des mines sur la grille
        private void PlaceMines()
        {
            Random rnd = new Random();
            int minesPlaced = 0;

            while (minesPlaced < _mines)
            {
                int x = rnd.Next(_rows);
                int y = rnd.Next(_columns);

                if (!_cells[x, y].IsMine)
                {
                    _cells[x, y].IsMine = true;
                    minesPlaced++;
                }
            }
        }

        // Marque ou démarque une case avec un drapeau
        public void FlagTile(int x, int y)
        {
            // Vérifiez si les coordonnées sont valides
            if (x >= 0 && x < _rows && y >= 0 && y < _columns)
            {
                _cells[x, y].ToggleFlag();
            }
            else
            {
                Console.WriteLine("Invalid coordinates. Please try again.");
            }
        }

        // Vérifie si toutes les mines ont été correctement marquées
        public bool AllMinesFlagged()
        {
            return _cells.Cast<Cell>().Count(cell => cell.IsMine) == _cells.Cast<Cell>().Count(cell => cell.IsMine && cell.IsFlagged);
        }

        // Vérifie si toutes les cases sans mine ont été révélées
        public bool AllSafeTilesRevealed()
        {
            int totalSafeTiles = _rows * _columns - _mines;
            int revealedSafeTiles = _cells.Cast<Cell>().Count(cell => !cell.IsMine && cell.IsRevealed);
            return totalSafeTiles == revealedSafeTiles;
        }

        // Activer les relevations adjacentes
        private void RevealAdjacentTiles(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newX < _rows && newY >= 0 && newY < _columns)
                    {
                        if (!_cells[newX, newY].IsRevealed && !_cells[newX, newY].IsMine)
                        {
                            _cells[newX, newY].Reveal();
                            if (_cells[newX, newY].AdjacentMines == 0)
                            {
                                RevealAdjacentTiles(newX, newY);
                            }
                        }
                    }
                }
            }
        }


        // Révèle une case et retourne si la case contenait une mine
        public bool RevealTile(int x, int y)
        {
            // Vérifiez si les coordonnées sont valides
            if (x >= 0 && x < _rows && y >= 0 && y < _columns)
            {
                _cells[x, y].Reveal();
                if (_cells[x, y].IsMine)
                {
                    return true; // Le joueur a frappé une mine!
                }
                else if (_cells[x, y].AdjacentMines == 0)
                {
                    RevealAdjacentTiles(x, y);
                }
            }
            else
            {
                Console.WriteLine("Invalid coordinates. Please try again.");
            }
            return false;
        }

        // Calculez le nombre de mines adjacentes pour chaque cellule
        private void CalculateAdjacentMines()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (!_cells[i, j].IsMine)
                    {
                        _cells[i, j].AdjacentMines = GetAdjacentMineCount(i, j);
                    }
                }
            }
        }

        // Compte le nombre de mines autour d'une cellule spécifique
        private int GetAdjacentMineCount(int x, int y)
        {
            int count = 0;

            // Vérifiez chaque cellule autour de la cellule actuelle
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    // Assurez-vous que nous ne sortons pas des limites de la grille
                    if (newX >= 0 && newX < _rows && newY >= 0 && newY < _columns)
                    {
                        if (_cells[newX, newY].IsMine)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }
    }
}
