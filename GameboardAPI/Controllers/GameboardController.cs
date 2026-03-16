using Battleship;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Battleship_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimpleGameboardController : ControllerBase
    {
        private static GameState game = new GameState(isPlayerOneTurn: true);
        GameRepository gameRepository = new GameRepository();

        [HttpGet("opponent")]
        public IActionResult GetOpponentBoard()
        {
            int[][] opponentBoard = new int[10][];

            for (int i = 0; i < 10; i++)
            {
                opponentBoard[i] = new int[10];
                for (int j = 0; j < 10; j++)
                {
                    opponentBoard[i][j] = game.opponentGameBoard[i, j];
                }
            }

            return Ok(opponentBoard);
        }

        [HttpGet("my")]
        public IActionResult GetMyBoard()
        {
            int[][] myBoard = new int[10][];

            for (int k = 0; k < 10; k++)
            {
                myBoard[k] = new int[10];
                for (int l = 0; l < 10; l++)
                {
                    myBoard[k][l] = game.myGameBoard[k, l];
                }
            }

            return Ok(myBoard);
        }

        [HttpPost("reset")]
        public IActionResult ResetGame()
        {
            game = new GameState(true);

            return Ok();
        }

        [HttpPost("fire")]
        public IActionResult Fire([FromBody] FireRequest request)
        {
            if (!game.isPlayerOneTurn)
            {
                return Conflict("Not your turn!");
            }

            if (request.Row < 0 || request.Row > 9 || request.Col < 0 || request.Col > 9)
            {
                return BadRequest("Invalid coordinates");
            }

            if (game.opponentGameBoard[request.Row, request.Col] == 2 ||
                game.opponentGameBoard[request.Row, request.Col] == -1)
            {
                return Conflict("Cell already used");
            }

            char result;

            if (game.opponentGameBoard[request.Row, request.Col] == 1)
            {
                game.opponentGameBoard[request.Row, request.Col] = 2;
                result = 'X';
            }
            else
            {
                game.opponentGameBoard[request.Row, request.Col] = -1;
                result = 'O';
            }

            if (game.IsOver(game.opponentGameBoard))
            {
                game.endGame(game.opponentGameBoard, true);
                return Ok(result);
            }

            game.totalMoves++;
            game.isPlayerOneTurn = !game.isPlayerOneTurn;

            Task.Run(() => game.checkBotTurn(game.isPlayerOneTurn));

            return Ok(result);
        }

        [HttpGet("isOver")]
        public IActionResult GetIsOver()
        {
            if (game.IsOver(game.opponentGameBoard))
            {
                return Ok(new { gameOver = true, winner = "player" });
            }
            else if (game.IsOver(game.myGameBoard))
            {
                return Ok(new { gameOver = true, winner = "CPU" });
            }

            return Ok(new { gameOver = false, winner = "" });
        }

        [HttpGet("getRecord")]
        public IActionResult getRecord()
        {
            var record = gameRepository.RetrieveRecord();
            Console.WriteLine($"DB accessed: PlayerWins={record.playerWins}, CPUWins={record.CPUWins}");
            return Ok(new { playerWins = record.playerWins, CPUWins = record.CPUWins });
        }
    }

    public class FireRequest
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }
}