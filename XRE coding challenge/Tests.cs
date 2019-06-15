using System;
using System.Collections.Generic;

namespace XRECodingChallenge
{
    class Test
    {
        public static void RunTests()
        {
            int numSuccess = 0;
            int numTests = 0;

            List<Pack> packSH3 = new List<Pack>();
            packSH3.Add(new Pack("Sliced Ham", "SH3", "3", "2.99"));
            packSH3.Add(new Pack("Sliced Ham", "SH3", "5", "4.49"));

            List<Pack> packYT2 = new List<Pack>();
            packYT2.Add(new Pack("Yoghurt", "YT2", "4", "4.95"));
            packYT2.Add(new Pack("Yoghurt", "YT2", "10", "9.95"));
            packYT2.Add(new Pack("Yoghurt", "YT2", "15", "13.95"));

            List<Pack> packTR = new List<Pack>();
            packTR.Add(new Pack("Toilet Rolls", "TR", "3", "2.95"));
            packTR.Add(new Pack("Toilet Rolls", "TR", "5", "4.45"));
            packTR.Add(new Pack("Toilet Rolls", "TR", "9", "7.99"));

            Console.WriteLine("Test pack breakdown");

            numSuccess += RunPackTest(30, packYT2, "30 YT2 (2 packs)",
                new List<int> { 15, 15 });
            numTests += 1;

            numSuccess += RunPackTest(28, packYT2, "28 YT2 (4 packs)",
                new List<int> { 10, 10, 4, 4 });
            numTests += 1;

            numSuccess += RunPackTest(13, packYT2, "13 YT2 (0 packs)",
                null);
            numTests += 1;

            Console.WriteLine();
            Console.WriteLine("Test final output");

            string expectedOutput = "10 SH3 $8.98" + Environment.NewLine;
            expectedOutput += "  2 x 5 $4.49";
            Packs requiredPacks = Program.MinimumRequiredPacks(10, packSH3);
            numSuccess += RunOutputTest(expectedOutput, requiredPacks, "SH3","10 SH3");
            numTests += 1;

            expectedOutput = "28 YT2 $29.80" + Environment.NewLine;
            expectedOutput += "  2 x 10 $9.95" + Environment.NewLine;
            expectedOutput += "  2 x 4 $4.95";
            requiredPacks = Program.MinimumRequiredPacks(28, packYT2);
            numSuccess += RunOutputTest(expectedOutput, requiredPacks, "YT2","28 YT2");
            numTests += 1;

            expectedOutput = "12 TR $10.94" + Environment.NewLine;
            expectedOutput += "  1 x 9 $7.99" + Environment.NewLine;
            expectedOutput += "  1 x 3 $2.95";
            requiredPacks = Program.MinimumRequiredPacks(12, packTR);
            numSuccess += RunOutputTest(expectedOutput, requiredPacks, "TR","12 TR ");
            numTests += 1;

            Console.WriteLine();
            Console.WriteLine(numSuccess + "/" + numTests + " successful");
        }

        static int RunPackTest(int qty, List<Pack> packList,
            string testName, List<int> expectedSizes)
        {
            try
            {
                Packs requiredPacks =
                        Program.MinimumRequiredPacks(qty, packList);

                if (expectedSizes == null)
                {
                    if (requiredPacks != null)
                    {
                        throw new Exception("Result is not null.");
                    }
                }
                else
                {
                    foreach (Pack pack in requiredPacks.PackList)
                    {
                        if (!expectedSizes.Remove(pack.Size))
                        {
                            throw new Exception("Unexpected size returned.");
                        }
                    }

                    if (expectedSizes.Count > 0)
                    {
                        throw new Exception("Incorrect number of packs.");
                    }
                }

                Console.WriteLine(testName + ": Test Success");
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(testName + ": Test Fail - " + e.Message);
                return 0;
            }
        }

        static int RunOutputTest(string expectedOutput, Packs requiredPacks,
            string productCode, string testName)
        {
            string output = Program.CostPackBreakdown(requiredPacks,
                productCode);

            if (output.Equals(expectedOutput))
            {
                Console.WriteLine(testName + ": Test Success");
                return 1;
            }
            else
            {
                Console.WriteLine(testName + ": Test Fail");
                Console.WriteLine("Expected:");
                Console.WriteLine(expectedOutput);
                Console.WriteLine("Actual:");
                Console.WriteLine(output);
                return 0;
            }
        }
    }
}