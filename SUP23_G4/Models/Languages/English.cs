using Newtonsoft.Json;
using SUP23_G4.FileHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP23_G4.Models.Languages
{
    public class English : Language
    {
        public English()
        {
            if (File.Exists("English.json"))
            {
                var englishData = JsonFileHandler.Open<Language>("English.json");

                HomePage = englishData.HomePage;
                Player1 = englishData.Player1;
                Player2 = englishData.Player2;
                Score = englishData.Score;
                Round = englishData.Round;
                MyTurn = englishData.MyTurn;
                GameRuleBtn = englishData.GameRuleBtn;
                ThrowDiceBtn = englishData.ThrowDiceBtn;
                ConfirmMove = englishData.ConfirmMove;
                LanguageName = englishData.LanguageName;
                Flag = englishData.Flag;
                SelectLanguage = englishData.SelectLanguage;
                StartGame = englishData.StartGame;
                GameRules = englishData.GameRules;
                DecidePoints = englishData.DecidePoints;
                PlayerWinner = englishData.PlayerWinner;
                BonusGame = englishData.BonusGame;
                BonusGameWon = englishData.BonusGameWon;
                Over45Player1 = englishData.Over45Player1;
                PlayerTurn = englishData.PlayerTurn;
                BonusGameTurn = englishData.BonusGameTurn;
                valueTooLow = englishData.valueTooLow;
            }
        }



       



    }
}
