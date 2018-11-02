using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class VendingMenus
    {
        private VendingMachine _vm = null;

        public VendingMenus(VendingMachine vm)
        {
            _vm = vm;
        }

        #region MainMenu
        public void MainMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("(1) Display Vending Machine Items");
                Console.WriteLine("(2) Purchase");
                Console.WriteLine("(Q) Exit");
                char input = Console.ReadKey().KeyChar;
                if (input == '1')
                {
                    DisplayMenu();
                }
                
                else if (input == '2')
                {
                    PurchaseMenu();
                }
                else if (input == 'Q' || input == 'q')
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection");
                    Console.ReadKey();
                }
            }
            Report report = new Report();
            report.UpdateSalesReport(_vm.soldInventory, _vm.machineInventory);
        }
        #endregion

        #region DisplayMenu
        public void DisplayMenu()
        {
            bool exit = false;
            while (!exit)
            {
                // display vending machine screen
                DisplayVendingMachine();
                Console.WriteLine("\n(Q) Exit");
                char input = Console.ReadKey().KeyChar;
                if (input == 'Q' || input == 'q')
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection");
                    Console.ReadKey();
                }
            }
        }
        #endregion

        #region PurchaseMenu
        public void PurchaseMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("(1) Feed Money");
                Console.WriteLine("(2) Select Product");
                Console.WriteLine("(3) Finish Transaction");
                Console.WriteLine("(Q) Quit");
                Console.WriteLine();
                Console.WriteLine($"Current Money Provided: {_vm.balance.ToString("C")}");
                char input = Console.ReadKey().KeyChar;

                if (input == 'Q' || input == 'q')
                {
                    exit = true;
                }
                else if (input == '1')
                {
                    Console.WriteLine();
                    Console.WriteLine("Please select the whole dollar amount ($1, $2, $5, $10) to feed into the machine:");
                    string dollarInput = Console.ReadLine();
                    decimal decDollar = decimal.Parse(dollarInput.Replace('$', ' ').Trim());
                    decimal rndDollar = Math.Round(decDollar, 0, 0);

                    if (decDollar == rndDollar && (decDollar == (decimal)1 || decDollar == (decimal)2 ||
                                                   decDollar == (decimal)5 || decDollar == (decimal)10))
                    {
                        if (!_vm.FeedMoney(decDollar))
                        {
                            Console.WriteLine("The bill was rejected.  The vendor has been informed in the exception log.");
                            Console.ReadKey();
                        }
                    }
                     else
                    {
                        // removed "$" if entered but either not 1-2-5-10 or is not whole numbers
                        Console.WriteLine("Invalid selection. Only $1, $2, $5, or $10 bills are valid.\nPress any key to continue.");
                        Console.ReadKey();
                    }
                }
                else if (input == '2')
                {
                    // display vending machine screen
                    DisplayVendingMachine();
                    Console.WriteLine();
                    Console.WriteLine($"Current Money Provided: {_vm.balance.ToString("C")}");
                    Console.WriteLine();
                    Console.WriteLine("Please enter the product code (slot) for the item you'd like to purchase:");
                    string itemCodeInput = Console.ReadLine().ToUpper();
                    List<Inventory> itemSelected = new List<Inventory>();  // list will contain successful selection
                    itemSelected = _vm.SelectItemFromMachine(itemCodeInput, itemSelected);

                    bool itemFound = (itemSelected.Count > 0 ? true : false);

                    // test lines below for display when debugging...
                    //Console.WriteLine($"Item found: {itemFound}  Item: {itemCodeInput}={itemSelected[0].Slot}  Count: {itemSelected.Count}");
                    //Console.ReadKey();

                    if (!itemFound)
                    {
                        Console.WriteLine("Item not found, press Q to return to the main menu.");
                        char menuInput = Console.ReadKey().KeyChar;
                        if (menuInput == 'Q' || menuInput == 'q')
                        {
                            // exit = true;
                        }
                    }
                    else
                    {
                        if (_vm.balance >= decimal.Parse(itemSelected[0].Price.ToString()) &&
                        itemSelected[0].Slot == itemCodeInput.ToUpper() && itemSelected[0].Quantity > 0)
                        {
                        itemSelected[0].RemoveItem();
                            _vm.soldInventory[itemSelected[0].Name]++;
                            decimal balanceSavedForLog = _vm.balance;
                            _vm.balance -= decimal.Parse(itemSelected[0].Price.ToString());
                            Console.Clear();
                            //ASCII art here???
                            Console.WriteLine($"{itemSelected[0].ConsumeNoise}\nPress any key to continue.");
                            Console.ReadKey();

                            // Write item sold to log
                            Log log = new Log();
                            //log.WriteLog(string logDesc, decimal logStartBalance, decimal logEndBalance);
                            bool result = log.WriteLog($"{itemSelected[0].Name} {itemSelected[0].Slot}", balanceSavedForLog, _vm.balance);
                        }
                        else if (_vm.balance >= decimal.Parse(itemSelected[0].Price.ToString()) && 
                                    itemSelected[0].Slot == itemCodeInput.ToUpper() && itemSelected[0].Quantity == 0)
                        {
                            Console.WriteLine($"{itemSelected[0].Name} is out of stock, please make another selection.");
                            Console.ReadKey();
                            exit = true;
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds for purchase, please insert sufficient money to purchase an item.");
                            Console.ReadKey();
                            //char menuInput = Console.ReadKey().KeyChar;
                            //if (menuInput == 'Q' || menuInput == 'q')
                            //{
                            //    // exit = true;
                            //    //break;
                            //}
                        }
                    }
                }
                else if (input == '3')
                {
                    if (_vm.balance > 0.00M)
                    {
                        Console.Clear();
                        int changeBack = Convert.ToInt32(_vm.balance * 100);
                        int qRemainder = changeBack % 25;
                        int quartersBack = changeBack / 25;
                        int dRemainder = qRemainder % 10;
                        int dimesBack = qRemainder / 10;
                        int nickelsBack = dRemainder / 5;

                        Console.WriteLine($"Transaction complete!\nYour change is {_vm.balance.ToString("C")}. You received {quartersBack} quarters, {dimesBack} dimes, and {nickelsBack} nickels." +
                            $"\nPress any key to return to the purchase menu or press Q to exit.");
                        decimal balanceSavedForLog = _vm.balance;
                        _vm.balance = 0.00M;

                        // Write change given to log
                        Log log = new Log();
                        //log.WriteLog(string logDesc, decimal logStartBalance, decimal logEndBalance);
                        bool result = log.WriteLog("GIVE CHANGE:", balanceSavedForLog, _vm.balance);
                    }
                    else
                    {
                        Console.WriteLine($"\nNo change possible, your balance is ${_vm.balance}." +
                            $"\nPress any key to return to the purchase menu or press Q to exit.");
                    }

                    char finishedInput = Console.ReadKey().KeyChar;                    
                    if (finishedInput == 'Q' || finishedInput == 'q')
                    {
                        exit = true;
                    }

                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection");
                    Console.ReadKey();
                }
            }
        }
        #endregion

        #region Display Vending Machine
        void DisplayVendingMachine()
        {
            //Displays the machineInventory, one row for each item, with a header row up top
            //The formatting here isn't optimal; it looks fine when displayed in the Windows console,
            //but we tried to get the same spacing by doing things such as using .PadLeft and
            //.PadRight simultaneously; basically the spacing/alignment is done manually here 
            //even though we tried to find another way.
            Console.Clear();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("| Slot ".PadRight(5, ' ') + "|        Item".PadRight(24, ' ') + "| Price |".PadRight(4, ' ') + " Quantity | ".PadLeft(9, ' '));
            Console.WriteLine("---------------------------------------------------");
            foreach (Inventory item in _vm.machineInventory)
            {
                Console.WriteLine($"|  {item.Slot}".PadRight(7, ' ') + $"|  {item.Name}".PadRight(24, ' ') + $"| {item.Price.ToString("C")} |".PadRight(6, ' ') + $"{item.DisplayQty} |".PadLeft(11, ' '));
            }
            Console.WriteLine("---------------------------------------------------");
            return;
        }
        #endregion

    }
}

