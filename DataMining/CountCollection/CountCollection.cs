using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining
{
    public class CountItem
    {
        public int count;
    }

    public abstract class CountCollection
    {
        protected int depth;
        public int Depth
        {
            get
            {
                return depth;
            }
            set
            {
                depth = value;
            }
        }

        public CountCollection(int depth)
        {
            this.depth = depth;
        }

        public abstract void AddItem(params int[] values);
        public abstract List<CountItem> FindItems(params int[][] values);
        public abstract int? FindColor(params int[][] values);

        protected void bubbleSort(ref int[] A)
        {
            int z;
            for (int i = 0; i < A.Length; i++)
                for (int j = 0; j < A.Length - 1; j++)
                    if (A[j] > A[j + 1])
                    {
                        z = A[j];
                        A[j] = A[j + 1];
                        A[j + 1] = z;
                    }
        }
    }
}
