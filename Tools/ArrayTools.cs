using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public static class ArrayTools
    {
        public static T[,] CopyArray<T>(T[,] input)
        {
            if (input == null)
                return null;

            T[,] output = new T[input.GetLength(0), input.GetLength(1)];

            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    output[i, j] = input[i, j];

            return output;
        }

        public static T[] CopyArray<T>(T[] input)
        {
            if (input == null)
                return null;

            T[] output = new T[input.GetLength(0)];

            for (int i = 0; i < input.GetLength(0); i++)
                output[i] = input[i];

            return output;
        }

        public static int[,] SumArrays(int[,] p1, int[,] p2)
        {
            if (p1 == null || p2 == null)
                return null;

            int[,] output = new int[p1.GetLength(0), p1.GetLength(1)];

            for (int i = 0; i < p1.GetLength(0); i++)
                for (int j = 0; j < p1.GetLength(1); j++)
                    output[i, j] = p1[i, j] ^ p2[i, j];

            return output;
        }
    }
}
