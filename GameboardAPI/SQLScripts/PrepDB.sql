/*CREATE DATABASE IF NOT EXISTS battleship;
USE battleship;

CREATE TABLE GameResults (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Winner ENUM('Player','CPU') NOT NULL,
    Moves INT NOT NULL,
    PlayedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

DELIMITER //

CREATE PROCEDURE SaveGameResult(
    IN p_winner ENUM('Player','CPU'),
    IN p_moves INT
)
BEGIN
    INSERT INTO GameResults (Winner, Moves) VALUES (p_winner, p_moves);
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE RetrieveRecord(
    OUT playerWins INT,
    OUT CPUWins INT
)
BEGIN
    SELECT 
        SUM(Winner='Player') INTO playerWins,
        SUM(Winner='CPU') INTO CPUWins
    FROM GameResults;
END //

DELIMITER ;*/