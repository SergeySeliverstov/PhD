using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace DataMining
{
    public class ColorItem : CountItem
    {
        public int[] colors;
    }

    public class CountCollectionList : CountCollection
    {
        private List<ColorItem> items;
        public List<ColorItem> Items
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

        //public CountCollectionList()
        //    : base()
        //{
        //}

        public CountCollectionList(int depth)
            : base(depth)
        {
            this.items = new List<ColorItem>();
        }

        public override void AddItem(params int[] newItem)
        {
            if (newItem.Length != depth)
                return;

            ColorItem colorItem = items.Find(delegate(ColorItem innerItem)
            {
                return compareArrays(innerItem.colors, newItem);
            });
            if (colorItem != null)
                colorItem.count++;
            else
                items.Add(new ColorItem { colors = newItem, count = 1 });
        }

        public override List<CountItem> FindItems(params int[][] values)
        {
            var result = new List<ColorItem>();
            foreach (int[] value in values)
            {
                var subResult = items.FindAll(delegate(ColorItem innerItem)
                {
                    return compareArrays(innerItem.colors, value);
                });
                result.AddRange(subResult);
            }
            return result.ConvertAll<CountItem>(new Converter<ColorItem, CountItem>(delegate(ColorItem item) { return (CountItem)item; }));
        }

        public override int? FindColor(params int[][] values)
        {
            var result = FindItems(values).OrderByDescending(i => i.count).FirstOrDefault();
            if (result == null)
                return null;

            foreach (var value in values)
            {
                List<int> tmpList = new List<int>(((ColorItem)result).colors);
                for (int i = 0; i < value.Length; i++)
                {
                    if (tmpList.Contains(value[i]))
                        tmpList.Remove(value[i]);
                }
                if (tmpList.Count == 1)
                    return tmpList[0];
            }
            return null;
        }

        private bool compareArrays(int[] arrray1, int[] array2)
        {
            // Быстрая проверка, если один из элементов не найден
            for (int i = 0; i < array2.Length; i++)
                if (!arrray1.Contains(array2[i]))
                    return false;

            // Более сложная проверка для отсеивания вариантов 1 = {1,2,3}; 2 = {1,2,1}
            List<int> tmpList = new List<int>(arrray1);
            for (int i = 0; i < array2.Length; i++)
            {
                if (!tmpList.Contains(array2[i]))
                    return false;
                tmpList.Remove(array2[i]);
            }

            // Если проверка прошла - массив входит в массив или совпадает
            return true;
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

        //public static void Save(string fileName, CountCollectionList countCollection)
        //{
        //    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
        //    {
        //        XmlSerializer xml = new XmlSerializer(typeof(CountCollectionList));
        //        xml.Serialize(fileStream, countCollection);
        //    }
        //}

        //public static CountCollectionList Load(string fileName)
        //{
        //    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
        //    {
        //        XmlSerializer xml = new XmlSerializer(typeof(CountCollectionList));
        //        return (CountCollectionList)xml.Deserialize(fileStream);
        //    }
        //}
    }
}
