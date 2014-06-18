using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Tools
{
    public class Matrix : IXmlSerializable
    {
        decimal[,] matrix;

        [Obsolete("Этот конструктор используется для " +
                  "сериализации и не может быть вызван " +
                  "явно.", true)]
        public Matrix()
        {
        }

        public Matrix(int rows, int columns)
        {
            matrix = new decimal[rows, columns];
        }

        public Matrix(decimal[,] array)
        {
            matrix = array;
        }

        public decimal this[int row, int column]
        {
            get
            {
                return matrix[row, column];
            }
            set
            {
                matrix[row, column] = value;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("rows", Rows.ToString());
            writer.WriteAttributeString("columns", Columns.ToString());

            for (int row = 0; row < Rows; ++row)
            {
                writer.WriteStartElement("Row");
                for (int column = 0; column < Columns; ++column)
                {
                    writer.WriteElementString("Column", matrix[row, column].ToString().Replace(",", "."));
                }
                writer.WriteEndElement();
            }
        }

        public int Rows
        {
            get
            {
                return matrix.GetLength(0);
            }
        }

        public int Columns
        {
            get
            {
                return matrix.GetLength(1);
            }
        }

        public void ReadXml(XmlReader reader)
        {
            int rows = XmlConvert.ToInt32(reader.GetAttribute("rows"));
            int columns = XmlConvert.ToInt32(reader.GetAttribute("columns"));
            reader.ReadStartElement("Matrix");

            matrix = new decimal[rows, columns];
            for (int row = 0; row < rows; ++row)
            {
                reader.ReadStartElement("Row");
                for (int column = 0; column < columns; ++column)
                {
                    matrix[row, column] = reader.ReadElementContentAsDecimal("Column", "");
                }
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        public XmlSchema GetSchema()
        {
            return (null);
        }
    }
}
