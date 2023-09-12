using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace SUP23_G4.Models
{
    public class Player
    {

        public string? Name { get; set; }
        public int Score { get; set; } 

        public Player(string name)
        {
            Name = name;
        }

    }
}
