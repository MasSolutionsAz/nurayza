using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Infrastructure.Helpers
{
    public static class EAN13
    {
        public static int ChecksumEan13(string data)
        {
            int iSum = 0;
            int iDigit = 0;

            // Calculate the checksum digit here.
            for (int i = data.Length; i >= 1; i--)
            {
                iDigit = Convert.ToInt32(data.Substring(i - 1, 1));
                // This appears to be backwards but the 
                // EAN-13 checksum must be calculated
                // this way to be compatible with UPC-A.
                if (i % 2 == 0)
                { // odd  
                    iSum += iDigit * 3;
                }
                else
                { // even
                    iSum += iDigit * 1;
                }
            }

            return (10 - (iSum % 10)) % 10;
        }

        public static bool CheckBarcode(string barcode)
        {
            string lastDigit = barcode.Substring(barcode.Length - 1, 1);
            string twelve = barcode.Substring(0, barcode.Length - 1);
            int result= ChecksumEan13(twelve);

            return result == Convert.ToInt32(lastDigit);
        }
    }
}
