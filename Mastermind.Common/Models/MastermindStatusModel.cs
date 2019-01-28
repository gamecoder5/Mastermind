using System;
using System.Collections.Generic;

namespace Mastermind.Common.Models
{
    public class MastermindStatusModel : BaseServiceModel
    {
        public Guid GameToken { get; set; }

        public int Attempts { get; set; }

        public int MaxAttempts { get; set; }

        public List<DigitStatusModel> DigitStatuses { get; set; }

        public bool GameOver { get; set; }

        public bool IsWinner { get; set; }
    }
}
