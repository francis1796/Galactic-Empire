using System;

namespace XRECodingChallenge
{
    class Pack
    {
        public string ProductName { get; }
        public string ProductCode { get; }
        public int Size { get; }
        public decimal Cost { get; }

        public Pack(string productName, string productCode,string size, string cost)
        {
            string message = PerformValidation(size, cost);
            if (message != "")
            {
                throw new Exception(message);
            }

                ProductName = productName;
                ProductCode = productCode;
                Size = Int32.Parse(size);
                Cost = Decimal.Parse(cost);
        }

        string PerformValidation(string size, string cost)
        {
            string message = "";

            try
            {
                int value = Int32.Parse(size);
                if (value < 1)
                {
                    message += Environment.NewLine;
                    message += "Invalid pack size:" + size;
                }
            }
            catch
            {
                message += Environment.NewLine;
                message += "Invalid pack size:" + size;
            }
            try
            {
                Decimal.Parse(cost);
            }
            catch
            {
                message += Environment.NewLine;
                message += "Invalid pack cost:" + cost;
            }
            return message;
        }
    }
}