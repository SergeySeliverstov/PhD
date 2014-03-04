using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public static class Tools
    {
        public static T[,] CopyArray<T>(T[,] input)
        {
            T[,] output = new T[input.GetLength(0), input.GetLength(1)];

            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    output[i, j] = input[i, j];

            return output;
        }

        public static T[] CopyArray<T>(T[] input)
        {
            T[] output = new T[input.GetLength(0)];

            for (int i = 0; i < input.GetLength(0); i++)
                output[i] = input[i];

            return output;
        }
    }
}
