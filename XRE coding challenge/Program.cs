using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XRECodingChallenge
{
    class Program
    {
        static void Main(string[] args)
        {

            // Read the packs.txt file
            Dictionary<string, List<Pack>> packCodeDict = new Dictionary<string, List<Pack>>();
            try
            {
                packCodeDict = GetPackData();
            }
            catch (Exception e)
            {
                PressEnterToExit("Error reading Packs file: " + e.Message);
            }

            Console.WriteLine("************Online Grocery Store************");
            Console.WriteLine("Name            Code            Packs\n" +
             "Sliced Ham      SH3        3 @ $2.99, 5 @ $4.49\n" +
              "Yoghurt         YT2        4 @ $4.95, 10 @ $9.95, 15 @ $13.95\n" +
             "Toilet Rolls    TR         3 @ $2.95, 5 @ $4.45, 9 @ $ 7.99\n");
            Console.WriteLine("Cart:");
            string output = "";

            // Read cart and display output breakdown
            while (1 == 1)
            {
                string input = Console.ReadLine();

                // Press enter to display total price
                if (input == "")
                {
                    FullCart(output);
                    output = "";
                    continue;
                }

                string message = ValidateCartInput(input, packCodeDict);
                if (message != "")
                {
                    FullCart("Error: " + message);
                    output = "";
                    continue;
                }

                string[] inputCode = input.Split(' ');
                int qty = Int32.Parse(inputCode[0]);
                string code = inputCode[1];

                Packs requiredPacks = MinimumRequiredPacks(qty, packCodeDict[code]);

                if (requiredPacks == null)
                {
                    FullCart("Error: Input Valid Quantity");
                    output = "";
                    continue;
                }
                else
                {
                    output += CostPackBreakdown(requiredPacks, code);
                    output += Environment.NewLine;
                }
            }
        }

        static void PressEnterToExit(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
            Environment.Exit(0);
        }

        static void FullCart(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Cart:");
        }

        static string ValidateCartInput(string input, Dictionary<string, List<Pack>> packCodeDict)
        {
            string message = "";

            string[] inputCode = input.Split(' ');
            string qtyPart = "";
            string codePart = "";

            // Validate input format
            try
            {
                qtyPart = inputCode[0];
                codePart = inputCode[1];
            }
            catch (Exception e)
            {
                message += Environment.NewLine;
                message += "Invalid input: " + e.Message;
                return message;
            }

            // Validate quantity
            try
            {
                int qty = Int32.Parse(qtyPart);
                if (qty < 1)
                {
                    message += Environment.NewLine;
                    message += "Invalid quantity:" + qtyPart;
                }
            }
            catch
            {
                message += Environment.NewLine;
                message += "Invalid quantity:" + qtyPart;
            }

            // Validate product code
            if (!packCodeDict.ContainsKey(codePart))
            {
                message += Environment.NewLine;
                message += "Product code not found:" + codePart;
            }

            return message;
        }

        // Get the pack.txt file from the console
        static Dictionary<string, List<Pack>> GetPackData()
        {
            Console.WriteLine("Enter the path to the packs.txt file:");
            string packsPath = Console.ReadLine();
            StreamReader reader = new StreamReader(packsPath);

            Dictionary<string, List<Pack>> packCodeDict =
                new Dictionary<string, List<Pack>>();
            List<Pack> packList = new List<Pack>();

            try
            {
                if (reader.Peek() == -1)
                {
                    throw new Exception("File is empty.");
                }

                while (reader.Peek() != -1)
                {
                    string input = reader.ReadLine();
                    string[] values = input.Split('|');
                    string productCode = values[1];

                    if (packCodeDict.ContainsKey(productCode))
                    {
                        packList = packCodeDict[productCode];
                    }
                    else
                    {
                        packList = new List<Pack>();
                    }

                    packList.Add(new Pack(values[0], values[1],
                        values[2], values[3]));
                    packCodeDict[productCode] = packList;
                }
            }
            finally
            {
                reader.Close();
            }

            return packCodeDict;
        }

        // Identify minimum required packs for specified quantity
        public static Packs MinimumRequiredPacks(int requiredQty, List<Pack> packsWithCode)
        {
            List<Packs> tryPacksList = new List<Packs>();
            Packs tryPacks = new Packs();
            foreach (Pack pack in packsWithCode)
            {
                tryPacks.TotalSize = pack.Size;

                List<Pack> newPackList = new List<Pack>();
                newPackList.Add(pack);
                tryPacks.PackList = newPackList;

                tryPacksList.Add(tryPacks);
                tryPacks = new Packs();
            }
            
            tryPacks = MinimumPacksCalculation(tryPacksList,
                packsWithCode, requiredQty);

            return tryPacks;
        }
        
        // Packs calculation
        static Packs MinimumPacksCalculation(List<Packs> tryPacksList, List<Pack> packsWithCode, int requiredQty)
        {
            while (tryPacksList.Count > 0)
            {
                List<Packs> toRemove = new List<Packs>();

                // Try all pack combination from list
                foreach (Packs tryPacks in tryPacksList)
                {
                    if (tryPacks.TotalSize == requiredQty)
                    {
                        return tryPacks;
                    }
                    else if (tryPacks.TotalSize > requiredQty)
                    {
                        toRemove.Add(tryPacks);
                    }
                }

                // Remove impossible combination from list
                foreach (Packs tryPacks in toRemove)
                {
                    tryPacksList.Remove(tryPacks);
                }

                if (tryPacksList.Count < 1)
                {
                    break;
                }

                // Add each type of pack to each pack list
                List<Packs> newTryPacksList = new List<Packs>();
                foreach (Pack pack in packsWithCode)
                {
                    foreach (Packs tryPacks in tryPacksList)
                    {
                        Packs newTryPacks = new Packs();
                        newTryPacks.TotalSize = tryPacks.TotalSize + pack.Size;

                        List<Pack> newPackList = new List<Pack>();
                        foreach (Pack oldPack in tryPacks.PackList)
                        {
                            newPackList.Add(oldPack);
                        }
                        newPackList.Add(pack);
                        newTryPacks.PackList = newPackList;

                        newTryPacksList.Add(newTryPacks);
                    }
                }
                tryPacksList = newTryPacksList;
            }
            return null;
        }

        // Breakdown of products inside the cart
        public static string CostPackBreakdown(Packs requiredPacks, string productCode)
        {
            decimal totalCost = TotalCost(requiredPacks);

            StringBuilder output = new StringBuilder();
            output.Append(requiredPacks.TotalSize + " ");
            output.Append(productCode + " $" + totalCost);

            // Sort size by descending order
            requiredPacks.PackList.Sort(
                (x, y) => -1 * x.Size.CompareTo(y.Size));

            // Generate line of pack type and quantity
            int prevSize = 0;
            int packQty = 0;
            decimal prevCost = 0;
            foreach (Pack pack in requiredPacks.PackList)
            {
                if (prevSize != pack.Size && prevSize != 0)
                {
                    output.Append(Environment.NewLine + "  ");
                    output.Append(packQty + " x ");
                    output.Append(prevSize + " $" + prevCost);
                    packQty = 1; // Current pack is different size
                }
                else
                {
                    packQty += 1;
                }
                prevSize = pack.Size;
                prevCost = pack.Cost;
            }
            output.Append(Environment.NewLine + "  ");
            output.Append(packQty + " x ");
            output.Append(prevSize + " $" + prevCost);

            return output.ToString();
        }

        // Return total cost of pack list.
        static decimal TotalCost(Packs requiredPacks)
        {
            decimal totalCost = 0;

            foreach (Pack pack in requiredPacks.PackList)
            {
                totalCost += pack.Cost;
            }

            return totalCost;
        }
    }
}