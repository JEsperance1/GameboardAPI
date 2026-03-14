using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleship
{
    public class GameState
    {
        public bool gameRunning;
        public bool isPlayerOneTurn;
        public int[,] opponentGameBoard;
        public int[,] myGameBoard;
        
        public GameState(bool gameRunning, bool isPlayerOneTurn)
        {
            this.gameRunning = gameRunning;
            this.isPlayerOneTurn = isPlayerOneTurn;
            this.opponentGameBoard = new int[10, 10];
            this.myGameBoard = new int[10, 10];

            placeShip(this.opponentGameBoard, Destroyer);
            placeShip(this.opponentGameBoard, Submarine);
            placeShip(this.opponentGameBoard, Cruiser);
            placeShip(this.opponentGameBoard, Battleship);
            placeShip(this.opponentGameBoard, Carrier);

            placeShip(this.myGameBoard, Destroyer);
            placeShip(this.myGameBoard, Submarine);
            placeShip(this.myGameBoard, Cruiser);
            placeShip(this.myGameBoard, Battleship);
            placeShip(this.myGameBoard, Carrier);
        }

        Ship Destroyer = new Ship("Destroyer", 2);
        Ship Submarine = new Ship("Submarine", 3);
        Ship Cruiser = new Ship("Cruiser", 3);
        Ship Battleship = new Ship("Battleship", 4);
        Ship Carrier = new Ship("Carrier", 5);
        Random rand = new Random();

        public void checkBotTurn(bool isPlayerOneTurn)
        {
            if (!isPlayerOneTurn)
            {
                Thread.Sleep(3000);
                this.botPlays(myGameBoard);
                //Console.Write("\n\nbot plays logic has been triggered");
            }
        }

        public void botPlays(int[,] myGameboard)
        {
            bool validShot = false;
            while (!validShot)
            {
                int row_index = rand.Next(0,10);
                int column_index = rand.Next(0, 10);

                if (myGameBoard[row_index, column_index] == 3 || myGameBoard[row_index, column_index] == 2)
                {
                    //Console.Write("Cannot use this spot. it has already been attacked\n");
                }
                else if (myGameBoard[row_index, column_index] == 1)
                {
                    Console.Write("Nice Hit\n");
                    myGameBoard[row_index, column_index] = 3;
                    validShot = true;
                }
                else if (myGameBoard[row_index, column_index] == 0)
                {
                    //Console.Write("You Missed\n");
                    myGameBoard[row_index, column_index] = 2;
                    validShot = true;
                }
                else
                {
                    //Console.Write("Invalid input, try again.\n");
                }
                this.isPlayerOneTurn = !isPlayerOneTurn;
            }
        }
        public int[,] placeShip(int[,] board, Ship shipToBePlaced )
        {
            bool placed = false;

            while (!placed)
            {
                int row = rand.Next(0, 10);
                int col = rand.Next(0, 10);
                bool isHorizontal = rand.Next(2) == 0;

                if (isHorizontal && col + shipToBePlaced.spaces > 10)
                {
                    continue;
                }
                if (!isHorizontal && row + shipToBePlaced.spaces > 10)
                {
                    continue;
                }
                bool valid = true;
                for (int i = 0; i < shipToBePlaced.spaces; i++)
                {
                    if (isHorizontal)
                    {
                        int r = row;
                        int c = col + i;
                        if (board[r, c] != 0)
                        {
                            valid = false;
                            break;
                        }
                    }
                    else
                    {
                        int r = row + i;
                        int c = col;
                        if (board[r, c] != 0)
                        {
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid)
                {
                    shipToBePlaced.startCoordinate = (row, col);
                    shipToBePlaced.isHorizontal = isHorizontal;

                    for (int i = 0; i < shipToBePlaced.spaces; i++)
                    { 
                    if (isHorizontal)
                        {
                            int r = row;
                            int c = col + i;
                            board[r, c] = 1;
                        }
                    else
                        {
                            int r = row + i;
                            int c = col;
                            board[r, c] = 1;
                        }
                    }
                    placed = true;
                }
            }
            return board;
        }
    }
}
