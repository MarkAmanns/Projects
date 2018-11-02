using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {

            VendingMachine vm = new VendingMachine();
            VendingMenus cli = new VendingMenus(vm);
            cli.MainMenu();
        }
    }
}
