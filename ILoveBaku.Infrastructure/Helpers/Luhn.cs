using System;
using System.Linq;
using System.Text;

namespace ILoveBaku.Infrastructure.Helpers
{
    public static class Luhn
    {
        public static string Generate()
        {
            int[] checkArray = new int[15];

            var cardNum = new int[16];

            for (int d = 14; d >= 0; d--)
            {
                cardNum[d] = new Random().Next(0, 9);
                checkArray[d] = (cardNum[d] * (((d + 1) % 2) + 1)) % 9;
            }

            cardNum[15] = (checkArray.Sum() * 9) % 10;

            StringBuilder stringBuilder = new StringBuilder();

            for (int d = 0; d < 16; d++) stringBuilder.Append(cardNum[d].ToString());

            return stringBuilder.ToString();
        }

        public static bool Check(string ccNumber)
        {
            int sum = 0;

            bool alternate = false;

            char[] nx = ccNumber.ToArray();

            for (int i = ccNumber.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(nx[i].ToString());

                if (alternate)
                {
                    n *= 2;

                    if (n > 9) n = (n % 10) + 1;
                }

                sum += n;

                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }
    }
}
