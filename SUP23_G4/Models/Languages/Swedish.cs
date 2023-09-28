using SUP23_G4.FileHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP23_G4.Models.Languages
{
    public class Swedish : Language
    {
        public Swedish()
        {
            if (File.Exists("Swedish.json"))
            {
               var swedishData = JsonFileHandler.Open<Language>("Swedish.json");

               HomePage = swedishData.HomePage;
               Player1 = swedishData.Player1;
                Player2 = swedishData.Player2;
                Score = swedishData.Score;
                Round = swedishData.Round;
                MyTurn = swedishData.MyTurn;
                GameRuleBtn = swedishData.GameRuleBtn;
                ThrowDiceBtn = swedishData.ThrowDiceBtn;
                ConfirmMove = swedishData.ConfirmMove;
                LanguageName = swedishData.LanguageName;
                Flag = swedishData.Flag;
                SelectLanguage = swedishData.SelectLanguage;
                StartGame = swedishData.StartGame;
                GameRules = swedishData.GameRules;
                DecidePoints = swedishData.DecidePoints;
                PlayerWinner = swedishData.PlayerWinner;
                BonusGame = swedishData.BonusGame;
                BonusGameWon = swedishData.BonusGameWon;
                Over45Player1 = swedishData.Over45Player1;
                PlayerTurn = swedishData.PlayerTurn;
                BonusGameTurn = swedishData.BonusGameTurn;
                valueTooLow = swedishData.valueTooLow;
            }
        }

    }
}

