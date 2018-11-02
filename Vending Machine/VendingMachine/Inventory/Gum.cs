using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Gum : Inventory
    {
        public Gum(string slot, string name, double price) : base(slot, name, price)
        {

        }

        public override string ConsumeNoise { get; } = "Chew Chew, Yum!";
    }
}
