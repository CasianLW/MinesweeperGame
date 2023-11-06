using System.Text;

namespace MinesweeperGame
{
    public class Cell
    {
        // Indique si la cellule contient une mine.
        public bool IsMine { get; set; }

        // Indique si la cellule a été révélée.
        public bool IsRevealed { get; private set; }

        // Indique si la cellule a été marquée avec un drapeau.
        public bool IsFlagged { get; private set; }

        // Nombre de mines adjacentes à cette cellule.
        public int AdjacentMines { get; set; }

        // Constructeur par défaut
        public Cell()
        {
            IsMine = false;
            IsRevealed = false;
            IsFlagged = false;
            AdjacentMines = 0;
        }

        // Marque ou démarque la cellule avec un drapeau.
        public void ToggleFlag()
        {
            // Si la cellule n'est pas déjà révélée, on change son état de drapeau.
            if (!IsRevealed)
            {
                IsFlagged = !IsFlagged;
            }
        }

        // Révèle la cellule.
        public void Reveal()
        {
            if (!IsFlagged)
            {
                IsRevealed = true;
            }
        }


        // This version of Display will append the cell's display character to a StringBuilder.
        public void Display(StringBuilder display, bool gameOver = false)
        {
            if (IsRevealed || (gameOver && IsMine))
            {
                if (IsMine)
                {
                    display.Append(" M ");
                }
                else
                {
                    display.AppendFormat(" {0} ", AdjacentMines > 0 ? AdjacentMines.ToString() : " "); // Pad the number
                }
            }
            else
            {
                if (IsFlagged)
                {
                    display.Append(" F ");
                }
                else
                {
                    display.Append(" X ");
                }
            }
            //display.Append(" "); // Append a space to keep the grid aligned
        }


        //old display, grid directlly inside console
        // Affiche l'état actuel de la cellule.
        //    public void Display(bool gameOver = false)
        //    {
        //        if (IsRevealed || (gameOver && IsMine))
        //        {
        //            if (IsMine)
        //            {
        //                //Console.Write("💣");
        //                Console.Write("M");

        //            }
        //            else
        //            {
        //                Console.Write(AdjacentMines > 0 ? AdjacentMines.ToString() : " ");
        //            }
        //        }
        //        else
        //        {
        //            if (IsFlagged)
        //            {
        //                //Console.Write("🚩");
        //                Console.Write("F");

        //            }
        //            else
        //            {
        //                //Console.Write("⬛");
        //                Console.Write("X");

        //            }
        //        }
        //    }
        }
    }
