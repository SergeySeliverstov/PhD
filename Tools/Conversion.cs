using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Tools
{
    public class Conversion
    {
        public static T[] Copy<T>(T[] array)
        {
            T[] result = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[i];
            return result;
        }

        public static T[,] Copy<T>(T[,] array)
        {
            T[,] result = new T[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    result[i, j] = array[i, j];
            return result;
        }

        public static int[] ComplexToInt(Complex[] array)
        {
            int[] result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = (int)array[i].Magnitude;
            return result;
        }

        public static Complex[] IntToComplex(int[] array)
        {
            Complex[] result = new Complex[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = new Complex(array[i], 0);
            return result;
        }

        public static int[,] ComplexToInt(Complex[,] array)
        {
            int[,] result = new int[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    //result[i, j] = (int)array[i, j].Magnitude;
                    result[i, j] = (int)array[i, j].Real;
            return result;
        }

        public static Complex[,] IntToComplex(int[,] array)
        {
            Complex[,] result = new Complex[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    result[i, j] = new Complex(array[i, j], 0);
            return result;
        }
    }
}
