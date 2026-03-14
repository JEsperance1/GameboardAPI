using Battleship;
using Microsoft.AspNetCore.Mvc;

namespace Battleship_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimpleGameboardController : ControllerBase
    {
        private static GameState game = new GameState(isPlayerOneTurn: true, gameRunning: true);

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
            Console.Write("the request has reached the server");
            game = new GameState(true, true);
            return Ok();
        }

        [HttpPost("fire")]
        public IActionResult Fire([FromBody] FireRequest request)
        {
            if (game.isPlayerOneTurn == true)
            {


                if (request.Row < 0 || request.Row > 9 || request.Col < 0 || request.Col > 9)
                {
                    return BadRequest("Invalid coordinates");
                }

                if (game.opponentGameBoard[request.Row, request.Col] == 1)
                {
                    game.opponentGameBoard[request.Row, request.Col] = 2; 

                    game.isPlayerOneTurn = !game.isPlayerOneTurn;
                    Task.Run(() => game.checkBotTurn(game.isPlayerOneTurn));
                    return Ok('X');
                    
                }
                else
                {
                    game.opponentGameBoard[request.Row, request.Col] = -1; 

                    game.isPlayerOneTurn = !game.isPlayerOneTurn;
                    Task.Run(() => game.checkBotTurn(game.isPlayerOneTurn));
                    return Ok('O');
                }
            } 
            else
            {
                return Forbid();
            }
        }
    }

    public class FireRequest
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }
}