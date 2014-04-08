using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Tools;

namespace DataMining
{
    public class TreeItem : CountItem
    {
        public int color;

        public SortedDictionary<int, TreeItem> Childs;

        public TreeItem()
            : base()
        {
            Childs = new SortedDictionary<int, TreeItem>();
        }
    }

    public class CountCollectionTree : CountCollection
    {
        private SortedDictionary<int, TreeItem> items;
        public SortedDictionary<int, TreeItem> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        //public CountCollectionTree()
        //    : base()
        //{
        //}

        public CountCollectionTree(int depth)
            : base(depth)
        {
            this.items = new SortedDictionary<int, TreeItem>();
        }

        public override void AddItem(params int[] values)
        {
            if (values.Length != depth)
                return;

            //bubbleSort(ref values);
            addItem(items, null, values);
        }

        public override List<CountItem> FindItems(params int[][] values)
        {
            List<TreeItem> result = new List<TreeItem>();

            foreach (int[] value in values)
            {
                int[] qq = value;
                //bubbleSort(ref qq);
                TreeItem findResult = findLastTreeItem(qq);
                if (findResult == null)
                    continue;

                if (!result.Contains(findResult))
                    result.Add(findResult);
            }

            return result.ConvertAll<CountItem>(new Converter<TreeItem, CountItem>(delegate(TreeItem item) { return (CountItem)item; }));
        }

        public override int? FindColor(int[][] values, int[][] values5 = null, decimal limit = 0, bool wsm = false)
        {
            int count = 0;
            if (values5 != null)
            {
                var find5 = FindItems(values5);
                count = find5.Sum(i => i.count);
            }

            var find = FindItems(values);
            var filter = find.Where(i => limit == 0 || count / i.count >= limit / 100);
            var result = filter.OrderByDescending(i => i.count).ToArray();
            if (result != null && result.Length > 0)
            {
                if (wsm)
                {
                    double r = 0;
                    double g = 0;
                    double b = 0;
                    double c = 0;
                    foreach (var ress in result)
                        c += ((TreeItem)ress).count;
                    foreach (var ress in result)
                    {
                        var color = new MyColor(((TreeItem)ress).color);
                        r += color.R * ((TreeItem)ress).count / c;
                        g += color.G * ((TreeItem)ress).count / c;
                        b += color.B * ((TreeItem)ress).count / c;
                    }
                    return new MyColor((byte)r, (byte)g, (byte)b).Color;
                }
                else
                {
                    return ((TreeItem)result[0]).color;
                }
            }
            return null;
        }

        private void addItem(SortedDictionary<int, TreeItem> root, TreeItem parentItem, params int[] values)
        {
            if (values.Length == 0)
                return;

            for (int i = 0; i < values.Length; i++)
            {
                if (!root.Keys.Contains(values[i]))
                    root.Add(values[i], new TreeItem { color = values[i], count = 1 });
                else
                    root[values[i]].count++;

                addItem(root[values[i]].Childs, root[values[i]], cutArray(values, i));
            }
        }

        private int[] cutArray(int[] sourceArray, int skipNumber)
        {
            int j = 0;
            int[] newArray = new int[sourceArray.Length - 1];
            for (int i = 0; i < sourceArray.Length; i++)
                if (i != skipNumber)
                    newArray[j++] = sourceArray[i];
            return newArray;
        }

        private TreeItem findLastTreeItem(params int[] values)
        {
            TreeItem lastItem = null;

            SortedDictionary<int, TreeItem> currentItem = items;
            for (int i = 0; i < values.Length; i++)
            {
                if (!currentItem.Keys.Contains(values[i]))
                    return null;

                lastItem = currentItem[values[i]];
                currentItem = currentItem[values[i]].Childs;
            }
            return lastItem;
        }

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (ColorItem colorItem in items)
        //    {
        //        sb.AppendLine(colorItem.ToString());
        //    }
        //    return sb.ToString();
        //}

        //public static void Save(string fileName, SortedDictionary<int, TreeItem> countCollection)
        //{
        //    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
        //    {
        //        XmlSerializer xml = new XmlSerializer(typeof(SortedDictionary<int, TreeItem>));
        //        xml.Serialize(fileStream, countCollection);
        //    }
        //}

        //public static SortedDictionary<int, TreeItem> Load(string fileName)
        //{
        //    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
        //    {
        //        XmlSerializer xml = new XmlSerializer(typeof(SortedDictionary<int, TreeItem>));
        //        return (SortedDictionary<int, TreeItem>)xml.Deserialize(fileStream);
        //    }
        //}
    }
}
