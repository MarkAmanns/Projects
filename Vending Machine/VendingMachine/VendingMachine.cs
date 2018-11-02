using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class VendingMachine
    {
        public List<Inventory> machineInventory = new List<Inventory>();
        public Dictionary<string, int> soldInventory = new Dictionary<string, int>();
        
        public decimal balance = 0;

        public VendingMachine()
        {
            fillMachine();
        }

        #region Fill Machine
        public List<Inventory> fillMachine()
        {
            string fullPath = @"C:\Workspace\team\week-4-pair-exercises-team-1\c#-capstone\etc\vendingmachine.csv";                       
 
            using (StreamReader sr = new StreamReader(fullPath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    try
                    {
                        string[] item = line.Split('|');
                        if (item[3] == "Chip")
                        {
                            machineInventory.Add(new Chip(item[0], item[1], double.Parse(item[2])));
                        }
                        if (item[3] == "Candy")
                        {
                            machineInventory.Add(new Candy(item[0], item[1], double.Parse(item[2])));
                        }
                        if (item[3] == "Drink")
                        {
                            machineInventory.Add(new Drink(item[0], item[1], double.Parse(item[2])));
                        }
                        if (item[3] == "Gum")
                        {
                            machineInventory.Add(new Gum(item[0], item[1], double.Parse(item[2])));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log log = new Log();
                        log.WriteErrorLog(DateTime.Now, ex.Message, ex.StackTrace);
                     }
                }   
            }
            foreach(Inventory item in machineInventory)
            {
                soldInventory[item.Name] = 0;
            }
            return machineInventory;
        }
        #endregion

        #region Feed Money
        public bool FeedMoney(decimal bill)
        {
            // Write FEED MONEY to log
            Log log = new Log();
            //log.WriteLog(string logDesc, decimal logStartBalance, decimal logEndBalance);
            bool result = log.WriteLog("FEED MONEY:", bill, balance);

            if (result)  // only add money to balance if feed amount is logged
            {
                balance += bill;
            }
            return result;
        }
        #endregion

        #region Select Item From Machine
        public List<Inventory> SelectItemFromMachine(string itemCodeInput, List<Inventory> itemSelected)
        {
            foreach (Inventory item in machineInventory)
            {
                if (item.Slot == itemCodeInput)
                {
                    bool itemFound = true;
                    itemSelected.Add(item);
                    break;
                }
            }
            return itemSelected;
        }
        #endregion
    }
}
