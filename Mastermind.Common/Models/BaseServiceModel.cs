using Mastermind.Common.Enums;

namespace Mastermind.Common.Models
{
    public abstract class BaseServiceModel
    {
        public bool IsError => !string.IsNullOrWhiteSpace(ErrorMessage);

        public string ErrorMessage { get; set; }

        public MastermindErrorType ErrorType { get; set; }
    }
}
