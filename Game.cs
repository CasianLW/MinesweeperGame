using System;

namespace MinesweeperGame
{
    class Game
    {
        // Déclaration des variables nécessaires pour le jeu
        private Grid _grid;
        private bool _gameOver;
        private DateTime _startTime;

        private string Difficulty { get; set; }


        public Game()
        {
            _gameOver = false;
        }

        // Fonction principale pour démarrer le jeu
        public void Start()
        {
            // Demander à l'utilisateur de choisir la difficulté du jeu
            ChooseDifficulty();

            // Initialiser la grille en fonction de la difficulté choisie
            _grid.Initialize();

            // Enregistrer le moment où le jeu commence pour le chronomètre
            _startTime = DateTime.Now;

            // Boucle principale du jeu
            do
            {
                Console.Clear();
                _grid.Display();  // Afficher la grille
                PlayerMove();     // Gérer le mouvement du joueur

            } while (!_gameOver);

            // Fonction pour terminer le jeu
            EndGame();
        }

        // Fonction pour choisir la difficulté
        private void ChooseDifficulty()
        {
            Console.WriteLine("Choose Difficulty:");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");
            Console.WriteLine("4. Custom");
            Console.Write("Your choice: ");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Invalid input! Please select a number between 1 and 4.");
                Console.Write("Your choice: ");
            }

            // Directly switch on the choice, no need for extra verification
            switch (choice)
            {
                case 1:
                    _grid = new Grid(9, 9, 10);
                    Difficulty = "Easy";
                    break;
                case 2:
                    _grid = new Grid(16, 16, 40);
                    Difficulty = "Medium";
                    break;
                case 3:
                    _grid = new Grid(30, 16, 99);
                    Difficulty = "Hard";
                    break;
                case 4:
                    Console.Write("Enter grid width: ");
                    int width = int.Parse(Console.ReadLine());

                    Console.Write("Enter grid height: ");
                    int height = int.Parse(Console.ReadLine());

                    int maxMines = width * height;

                    int mines;
                    do
                    {
                        Console.Write($"Enter number of mines (>0 and <{maxMines}): ");
                        mines = int.Parse(Console.ReadLine());

                        if (mines <= 0 || mines >= maxMines)
                        {
                            Console.WriteLine($"Invalid number of mines! Please enter a number greater than 0 and less than {maxMines}.");
                        }
                    }
                    while (mines <= 0 || mines >= maxMines);

                    Difficulty = $"Custom: w:{width} h:{height} m:{mines}";
                    _grid = new Grid(width, height, mines);
                    break;
                default:
                    Console.WriteLine("Invalid choice! Defaulting to Easy.");
                    _grid = new Grid(9, 9, 10);
                    Difficulty = "Easy";
                    break;
            }
            //else
            //{
            //    Console.WriteLine("Invalid input! Defaulting to Easy.");
            //    _grid = new Grid(9, 9, 10);
            //    Difficulty = "Easy";

            //}
        }


        // Fonction pour gérer le mouvement du joueur
        private void PlayerMove()
        {

            // Demander au joueur de faire un mouvement ou de marquer une case
            Console.WriteLine("Enter a move (format: x y) or flag a mine (format: f x y):");

            string[] input = Console.ReadLine().Split();

            // gestion erreur, si mouvement invalide afficher message
            if (input.Length == 2)
            {
                int x, y;
                if (int.TryParse(input[0], out x) && int.TryParse(input[1], out y))
                {
                    _gameOver = _grid.RevealTile(x, y);
                }
            }
            else if (input.Length == 3 && input[0] == "f")
            {
                int x, y;
                if (int.TryParse(input[1], out x) && int.TryParse(input[2], out y))
                {
                    _grid.FlagTile(x, y);
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        // Fonction pour gérer la fin du jeu
        private void EndGame()
        {
            Console.Clear();
            _grid.Display(true);
            // Calculer le temps écoulé depuis le début du jeu
            TimeSpan timeTaken = DateTime.Now - _startTime;
            string result;

    
            // Vérifier si le joueur a gagné ou perdu
            if (_grid.AllMinesFlagged() && _grid.AllSafeTilesRevealed())
            {
                Console.WriteLine("Congratulations! You have won!");
                result = "Win"; // set result as Win
            }
            else
            {
                Console.WriteLine("Game over! You hit a mine.");
                result = "Lost"; // set result as Lost

            }

            


            Console.WriteLine("Time taken: " + timeTaken.ToString(@"mm\:ss") + " - " + result);

            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

            while (string.IsNullOrEmpty(playerName.Trim()))
            {
                Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                Console.Write("Enter your name: ");
                playerName = Console.ReadLine();
            }

            // Enregistrer le score du joueur
            // ScoreManager.SaveScore(playerName, (int)timeTaken.TotalSeconds, _grid.GetDifficulty());
            // string timeString = timeTaken.ToString(@"mm\:ss");

            ScoreManager.SaveScore(playerName, (int)timeTaken.TotalSeconds, Difficulty,result);
            Console.Clear();

        }
    }
}
