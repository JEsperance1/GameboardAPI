using MySql.Data.MySqlClient;
using System.Data;

public class GameRepository
{
    private string _connStr = "server=localhost;user=root;password=Gamr3344!;database=battleship;";

    public void saveGame(string winner, int moves)
    {
        using var conn = new MySqlConnection(_connStr);
        conn.Open();

        using var cmd = new MySqlCommand("SaveGameResult", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@p_winner", winner);
        cmd.Parameters.AddWithValue("@p_moves", moves);

        cmd.ExecuteNonQuery();
    }

    public (int playerWins, int CPUWins) RetrieveRecord()
    {
        using var conn = new MySqlConnection(_connStr);
        conn.Open();

        using var cmd = new MySqlCommand("RetrieveRecord", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        var pWins = new MySqlParameter("@playerWins", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
        var cWins = new MySqlParameter("@CPUWins", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
        cmd.Parameters.Add(pWins);
        cmd.Parameters.Add(cWins);

        cmd.ExecuteNonQuery();

        // Handle DBNull safely
        int playerWins = pWins.Value != DBNull.Value ? Convert.ToInt32(pWins.Value) : 0;
        int CPUWins = cWins.Value != DBNull.Value ? Convert.ToInt32(cWins.Value) : 0;

        return (playerWins, CPUWins);
    }
}