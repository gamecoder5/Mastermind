using System.Collections.Generic;

namespace Mastermind.Common.Models
{
    public class MastermindModel : NewGameModel
    {
        public string Answer { get; set; }

        public List<string> History { get; set; }

        public bool GameOver { get; set; }

        public bool IsWinner { get; set; }
    }
}
