using System;

namespace Mastermind.Common.Models
{
    public class NewGameModel : BaseServiceModel
    {
        public Guid GameToken { get; set; }

        public int Attempts { get; set; }

        public int MaxAttempts { get; set; }
    }
}
