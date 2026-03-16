using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MySql.Data.MySqlClient;


namespace Battleship
{
    public class GameState
    {
        public bool gameRunning;
        public bool isPlayerOneTurn;
        public int[,] opponentGameBoard;
        public int[,] myGameBoard;
        public int totalMoves;

        Random rand = new Random();
        GameRepository gameRepository = new GameRepository();

        Ship Destroyer = new Ship("Destroyer", 2);
        Ship Submarine = new Ship("Submarine", 3);
        Ship Cruiser = new Ship("Cruiser", 3);
        Ship Battleship = new Ship("Battleship", 4);
        Ship Carrier = new Ship("Carrier", 5);

        public GameState(bool isPlayerOneTurn)
        {
            this.isPlayerOneTurn = isPlayerOneTurn;
            this.opponentGameBoard = new int[10, 10];
            this.myGameBoard = new int[10, 10];
            this.totalMoves = 0;

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


        public void checkBotTurn(bool isPlayerOneTurn)
        {
            if (!isPlayerOneTurn)
            {
                Thread.Sleep(3000);
                this.botPlays(myGameBoard);
            }
        }

        public bool IsOver(int[,] board)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (board[i, j] == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void endGame(int[,] losingBoard, bool gameIsOver)
        {

            if (gameIsOver)
            {
                if (losingBoard == this.opponentGameBoard)
                {
                    gameRepository.saveGame("Player", this.totalMoves);
                }
                else
                {
                    gameRepository.saveGame("CPU", this.totalMoves);
                }
            }
        }

        public void botPlays(int[,] myGameBoard)
        {
            bool validShot = false;

            while (!validShot)
            {
                int row = rand.Next(0, 10);
                int col = rand.Next(0, 10);
                int cell = myGameBoard[row, col];

                if (cell == 0)
                {
                    myGameBoard[row, col] = 2; 
                    validShot = true;
                }
                else if (cell == 1)
                {
                    myGameBoard[row, col] = 3; 
                    validShot = true;
                }
                
            }

            this.isPlayerOneTurn = true;
        }

        public int[,] placeShip(int[,] board, Ship shipToBePlaced )
        {
            bool placed = false;
            while (!placed)
            {
                //randomly selects a spot
                int row = rand.Next(0, 10);
                int col = rand.Next(0, 10);
                bool isHorizontal = rand.Next(2) == 0;

                //checks if it exceeds the board boundary
                if (isHorizontal && col + shipToBePlaced.spaces > 10)
                {
                    continue;
                }
                if (!isHorizontal && row + shipToBePlaced.spaces > 10)
                {
                    continue;
                }
                bool valid = true;

                // checks for ship collisions
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
                //places ship at the verified spots on the board
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
