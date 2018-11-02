using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Report
    {

        private Dictionary<string, int> _salesReport = new Dictionary<string, int>();

        private string _salesReportFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\etc\SalesReport.txt");
        private decimal _salesReportTotal = 0.00M;

        public void UpdateSalesReport(Dictionary<string, int> vmSoldInventory, List<Inventory> vmMachineInventory)
        {
            bool mustWriteReport = false;
            if (File.Exists(_salesReportFilePath))
            {
                // Get existing sales report from file
                using (StreamReader sr = new StreamReader(_salesReportFilePath))
                {
                    try
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (line.Trim().Length > 0)
                            {
                                string[] itemArray = line.Split('|');
                                _salesReport.Add(itemArray[0], int.Parse(itemArray[1]));
                            }
                            else
                            {
                                // next line holds totals:  **TOTAL SALES** $9,999.99
                                line = sr.ReadLine();
                                if (line.Length > 17)
                                {
                                    line = line.Substring(17).Replace(",", "");
                                    _salesReportTotal = decimal.Parse(line);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log log = new Log();
                        log.WriteErrorLog(DateTime.Now, ex.Message, ex.StackTrace);
                    }
                }
            }
            else
            {
                // create initial sales report dictionary from MachineInventory
                mustWriteReport = true;
                foreach (Inventory item in vmMachineInventory)
                {
                    _salesReport.Add(item.Name, 0);
                }
                _salesReportTotal = 0.00M;
            }

            // Update _salesReport with items sold amounts

            foreach (KeyValuePair<string, int> thisKey in vmSoldInventory)
            {
                if (thisKey.Value != 0)
                {
                    mustWriteReport = true;
                    if (_salesReport.ContainsKey(thisKey.Key))
                    {
                        _salesReport[thisKey.Key] += thisKey.Value;
                    }
                    else
                    {
                        // add item (possibly new) to sales report
                        _salesReport.Add(thisKey.Key, thisKey.Value);
                    }

                    //now write code for increasing _salesReportTotal
                    foreach (Inventory item in vmMachineInventory)
                    {
                        if (item.Name == thisKey.Key)
                        {
                            //string xValue = item.Price.ToString();
                            _salesReportTotal += thisKey.Value * (decimal)item.Price;// decimal.Parse(xValue);
                        }
                    }
                }
            }

            // Write _salesReport to file
            if (mustWriteReport)
            {
                using (StreamWriter sw = new StreamWriter(_salesReportFilePath)) // creates or overwrites
                {
                    try
                    {
                        foreach (KeyValuePair<string, int> thisKey in _salesReport)
                        {
                            sw.WriteLine($"{thisKey.Key}|{thisKey.Value.ToString()}");
                        }
                        sw.WriteLine();
                        sw.WriteLine($"**TOTAL SALES** {_salesReportTotal.ToString("C")}");
                    }

                    catch (Exception ex)
                    {
                        Log log = new Log();
                        log.WriteErrorLog(DateTime.Now, ex.Message, ex.StackTrace);
                    }
                }
            }
        }
    }
}