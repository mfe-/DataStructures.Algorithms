using System;

namespace BarcodeChecker
{   
    public class BarcodeCheck
    {
        public string barcode { get; set; }

        public bool CheckBarcode()
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
