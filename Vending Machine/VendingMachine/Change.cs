using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Change
    {
        public decimal VMBalance { get; private set; } = 0;

        public Change(decimal vmbalance)
        {
            VMBalance = vmbalance;
        }

        public decimal GiveChange(decimal balance)
        {
            return balance;
        }
    }
}
