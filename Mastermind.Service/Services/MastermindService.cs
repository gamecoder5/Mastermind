using Mastermind.Common.Enums;
using Mastermind.Common.Exceptions;
using Mastermind.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mastermind.Service.Services
{

    /// <summary>
    /// Service oriented approach to playing Mastermind
    /// </summary>
    public class MastermindService
    {
        private List<MastermindModel> Games { get; set; }


        public MastermindService()
        {
            Games = new List<MastermindModel>();
        }

        /// <summary>
        /// Registers new game with service. Returns a game instance's token guid that is used in each subsequent request
        /// in order to play that instance of the game.
        /// </summary>
        /// <returns></returns>
        public NewGameModel GetNewGame()
        {
            try
            {
                var game = new MastermindModel
                {
                    Attempts = 0,
                    MaxAttempts = 10,
                    GameToken = Guid.NewGuid(),
                    History = new List<string>(),

                    Answer = generateAnswer()
                };

                Games.Add(game);

                return game;
            }
            catch (Exception ex)
            {
                return new NewGameModel { ErrorType = MastermindErrorType.Exception, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Player's 4 digit combination, each digit a number from 1 through 6.
        /// Returns status of players combo, the game status, the total game attempts, the total attempts left to try
        /// </summary>
        public MastermindStatusModel SetCombination(Guid gameToken, string combination)
        {
            try
            {
                //find game based on game token
                var game = Games.FirstOrDefault(g => g.GameToken == gameToken);
                if (game == null)
                    throw new GameException(MastermindErrorType.InvalidGameToken, $"Game token '{gameToken}' not found");

                //validate input is 4 numbers, values 1 through 6 only
                if (string.IsNullOrWhiteSpace(combination))
                    throw new GameException(MastermindErrorType.CombinationInvalidEntry, $"Please enter a combination");

                //validate combo contains 4 digits
                //null conditional not needed here because of code above, added anyway for future precautionary measures
                if (combination?.Length != 4)
                    throw new GameException(MastermindErrorType.CombinationInvalidEntry, $"Combination must be 4 digits long, values 1 through 6 for each digit");

                //validate those digits are from 1 through 6 only
                if (!Regex.IsMatch(combination, "^[1-6]*$"))
                    throw new GameException(MastermindErrorType.CombinationInvalidEntry, $"Combination must be 4 digits long, values 1 through 6 for each digit");

                if (game.GameOver)
                    throw new GameException(MastermindErrorType.GameOver, $"This game has already ended");


                //increment the total combination unlock attempts
                //if max reached, game is over
                if (++game.Attempts >= game.MaxAttempts)
                    game.GameOver = true;

                //add to history of attempts
                game.History.Add(combination);


                //get status on each digit from the combination entered
                //this is returned for processing display output
                //NOTE: This takes into account position and if the digit exists
                //if there was multiple of the same digit entered in combo, and this digit occured less than
                //the number of times it exists in answer, since that digit does exist, it still reports as at least found (correct)
                //this could be improved apon to attempt to account for the totals and placements
                var digitStats = new List<DigitStatusModel>();
                int index = 0;
                combination.ToList().ForEach(c =>
                {
                    digitStats.Add(new DigitStatusModel()
                    {
                        Value = c,
                        DigitStatus = game.Answer.Contains(c) ? (game.Answer[index] == c ? DigitStatusType.PositionallyyCorrect : DigitStatusType.Correct) : DigitStatusType.Incorrect

                    });

                    index++;
                });

                //if all digitStats are positionally correct, set is winner and game over to true
                if (digitStats.Select(d => d.DigitStatus).All(digitStatus => digitStatus == DigitStatusType.PositionallyyCorrect))
                {
                    game.IsWinner = true;
                    game.GameOver = true;
                }

                return new MastermindStatusModel
                {
                    Attempts = game.Attempts,
                    GameToken = game.GameToken,
                    MaxAttempts = game.MaxAttempts,
                    DigitStatuses = digitStats,
                    GameOver = game.GameOver,
                    IsWinner = game.IsWinner
                };
            }
            catch (GameException gEx)
            {
                return new MastermindStatusModel { ErrorType = gEx.MastermindError, ErrorMessage = gEx.Message };
            }
            catch (Exception ex)
            {
                return new MastermindStatusModel { ErrorType = MastermindErrorType.Exception, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Generate a random string of 4 digits, with each digit being from 1 through 6 only
        /// </summary>
        /// <returns></returns>
        private string generateAnswer()
        {
            var answer = string.Empty;

            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            for (var i = 0; i < 4; i++)
                answer += rnd.Next(1, 6).ToString();

            return answer;
        }
    }

}
