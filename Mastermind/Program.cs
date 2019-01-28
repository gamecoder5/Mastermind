using Mastermind.Common.Enums;
using Mastermind.Common.Models;
using Mastermind.Service.Services;
using System;

namespace Mastermind
{
    /// <summary>
    /// Mastermind game interface 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("WELCOME TO MASTERMIND!");

                ConsoleKeyInfo playKey;
                MastermindStatusModel gameStatus = new MastermindStatusModel() { GameOver = true };
                var gameOver = true;
                NewGameModel newGame = null;
                var service = new MastermindService();

                do
                {
                    while (!gameOver)
                    {
                        Console.WriteLine("Please enter a combination:");
                        var combination = Console.ReadLine();

                        gameStatus = service.SetCombination(newGame.GameToken, combination);
                        gameOver = gameStatus.GameOver;

                        if (gameStatus.IsError)
                            Console.WriteLine($"{gameStatus.ErrorType.ToString()}: {gameStatus.ErrorMessage}");
                        else if (gameStatus.GameOver)
                        {

                            if (gameStatus.IsWinner)
                                Console.WriteLine("WINNER WINNER CHICKEN DINNER! YOU ENTERED THE CORRECT COMBINATION!");
                            else
                                Console.WriteLine($"GAME OVER! You failed to enter the right combination within {gameStatus.MaxAttempts} max tries :(");
                        }
                        else
                        {
                            Console.Write("Combination Results:");
                            gameStatus.DigitStatuses.ForEach(ds =>
                            {
                                Console.Write(ds.DigitStatus == DigitStatusType.Correct? "-" : (ds.DigitStatus == DigitStatusType.Incorrect? " " : "+"));
                            });
                            Console.WriteLine("");
                            Console.WriteLine($"Attempt {gameStatus.Attempts} of {gameStatus.MaxAttempts} made. Please try again");

                        }

                    }

                    Console.WriteLine("Would you like to play a new game of Mastermind?");
                    Console.WriteLine("(Press 'y' for yes, any other key to exit the application");
                    playKey = Console.ReadKey();
                    Console.WriteLine("");
                    if (playKey.KeyChar == 'y')
                    {
                        newGame = service.GetNewGame();
                        gameOver = false;
                    }
                   
                } while (!gameOver);
            }
            catch(Exception ex)
            {
                Console.WriteLine("An unexpected error occured");
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"Press any key to exit the program");
                Console.ReadKey();
            }
        }
    }
}
