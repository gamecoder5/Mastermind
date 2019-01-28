using Mastermind.Common.Enums;
using System;

namespace Mastermind.Common.Exceptions
{
    public class GameException : Exception
    {
        public GameException(MastermindErrorType error, string message) : base(message)
        {
            MastermindError = error;
        }

        public MastermindErrorType MastermindError { get; set; }
    }
}
