using System;

namespace BarcodeChecker
{   
    public class BarcodeCheck
    {
        public string barcode { get; set; }

        public bool CheckBarcode(string barcode)
        {
            if (barcode.StartsWith("BA"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
