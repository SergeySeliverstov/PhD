using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace DecisionMethods
{
    public class MatrixTools
    {
        public static Matrix PrepareMatrix(Matrix matrix)
        {
            decimal[,] array = new decimal[matrix.Rows, matrix.Columns];

            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Columns; j++)
                    if (i != j)
                        array[i, j] = matrix[j, j] != 0 ? matrix[i, i] / matrix[j, j] : 0;
                    else
                        array[i, j] = matrix[j, j] != 0 ? 1 : 0;

            return new Matrix(array);
        }

        public static List<Matrix> PrepareMatrix(List<Matrix> matrix)
        {
            var result = new List<Matrix>();
            foreach (var matrixItem in matrix)
                result.Add(PrepareMatrix(matrixItem));

            return result;
        }

        public static decimal FindMax(Matrix matrix, out int maxRow, out int maxColumn)
        {
            decimal max = decimal.MinValue;
            maxRow = 0;
            maxColumn = 0;
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Columns; j++)
                    if (max < matrix[i, j])
                    {
                        max = matrix[i, j];
                        maxRow = i;
                        maxColumn = j;
                    }
            return max;
        }
    }
}
