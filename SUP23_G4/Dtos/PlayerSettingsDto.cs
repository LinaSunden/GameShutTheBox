using SUP23_G4.Models;
using SUP23_G4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP23_G4.Dto
{
    public class PlayerSettingsDto : BaseViewModel
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int TargetPoints { get; set; }

        public PlayerSettingsDto(Player player1, Player player2, int targetPoints)
        {
            Player1 = player1;
            Player2 = player2;
            TargetPoints = targetPoints;
        }

    }
}
