using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class MyColor
    {
        private int color;
        public int Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public byte R
        {
            get
            {
                return (byte)((color >> 16) & 0xFF);
            }
            set
            {
                color = color & 0x00FFFF | (value << 16);
            }
        }

        public byte G
        {
            get
            {
                return (byte)((color >> 8) & 0xFF);
            }
            set
            {
                color = color & 0xFF00FF | (value << 8);
            }
        }

        public byte B
        {
            get
            {
                return (byte)(color & 0xFF);
            }
            set
            {
                color = color & 0xFFFF00 | value;
            }
        }

        public MyColor(int color)
        {
            this.color = color;
        }

        public MyColor(byte R, byte G, byte B)
        {
            this.color = (R << 16) | (G << 8) | B;
        }
    }
}
