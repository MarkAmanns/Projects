using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public abstract class Inventory
    {
        public string Slot { get; }

        public string Name { get; }

        public double Price { get; }

        public int Quantity { get; private set; } = 5;

        public int RemainingQuantity { get; private set; }

        public abstract string ConsumeNoise { get; }

        public Inventory(string slot, string name, double price)
        {
            Slot = slot;
            Name = name;
            Price = price;
        }

        public void RemoveItem()
        {
            if(Quantity > 0)
            {
                Quantity--;
            }
            else
            {
                throw new Exception($"Out of {Name}.");
            }
        }

        public string DisplayQty
        {
            get
            {
                if (Quantity > 0)
                {
                    return Quantity.ToString();
                }
                else
                {
                    return "SOLD OUT";
                }
            }
        }
    }
}
