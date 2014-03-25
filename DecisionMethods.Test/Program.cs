using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionMethods.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[,] array;
            bool result = false;

            //array = new byte[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 1, 1 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 0, 0, 0 }, { 1, 0, 1 }, { 1, 1, 1 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 1, 1, 1 }, { 0, 0, 0 }, { 1, 1, 1 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 1, 1, 1 }, { 0, 1, 0 }, { 1, 1, 1 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 1, 1, 0 }, { 1, 0, 0 }, { 0, 0, 0 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 1, 1, 0 }, { 1, 1, 0 }, { 0, 0, 0 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 1, 0, 1 }, { 0, 1, 0 }, { 1, 0, 1 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            //array = new byte[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 1, 1 } };
            //result = foundpixels.PixelIsBroken(array, 1, 1);
            //ShowArray<byte>(array);
            //Console.WriteLine(result);

            array = new byte[,] { { 0xFF, 0x0F, 0xFF }, { 0x0F, 0, 0 }, { 0xFF, 0x0F, 0xFF } };
            result = FoundPixels.PixelIsBroken(array, 1, 1, 2, 2, 1, true);
            ShowArray<byte>(array);
            Console.WriteLine(result);

            Console.ReadLine();
        }

        static void ShowArray<T>(T[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                    Console.Write("\t" + array[i, j]);
                Console.Write("\n");
            }
        }
    }
}
