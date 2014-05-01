using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class FuncTools
    {
        public static int[,] FuncsToBytes(double delta, params Func[] funcs)
        {
            if (funcs.Length == 0)
                throw new ArgumentException("You need more functions!");

            var colors = new int[] { 0x00FF00, 0x0000FF, 0xFF0000, 0xFFFF00, 0xFF00FF, 0x00FFFF, 0xFFFFFF };

            var size = funcs[0].x.Length;

            var xMax = double.MinValue;
            var yMax = double.MinValue;
            var xMin = double.MaxValue;
            var yMin = double.MaxValue;
            foreach (var func in funcs)
            {
                if (xMax < func.x.Max())
                    xMax = func.x.Max();
                if (yMax < func.y.Max())
                    yMax = func.y.Max();
                if (xMin > func.x.Min())
                    xMin = func.x.Min();
                if (yMin > func.y.Min())
                    yMin = func.y.Min();
            }

            var pic = new int[size + 1, size + 1];

            //Белый фон
            for (var i = 0; i < pic.GetLength(0); i++)
                for (var j = 0; j < pic.GetLength(1); j++)
                    pic[i, j] = 0xFFFFFF;

            var coords = new Coord(xMin, xMax, yMin, yMax, size, size);

            //График
            for (var i = 0; i < funcs.Length; i++)
                for (var j = 0; j < funcs[i].x.Length; j++)
                    pic[coords.GetX(funcs[i].x[j]), coords.GetY(funcs[i].y[j])] = colors[i % colors.Length];

            double xi;
            double yi;
            int axisColor = 0x000000;
            int subAxisColor = 0xAAAAAA;

            // Вертикальные линии
            xi = 0;
            while (xi <= xMax)
            {
                for (var i = 0; i < funcs[0].x.Length; i++)
                    pic[coords.GetX(xi), i] = subAxisColor;
                xi += delta;
            }

            xi = 0;
            while (xi > xMin)
            {
                for (var i = 0; i < funcs[0].x.Length; i++)
                    pic[coords.GetX(xi), i] = subAxisColor;
                xi -= delta;
            }

            // Горизонтальные линии
            yi = 0;
            while (yi <= yMax)
            {
                for (var i = 0; i < funcs[0].y.Length; i++)
                    pic[i, coords.GetY(yi)] = subAxisColor;
                yi += (yMax - yMin) / 5;
            }

            yi = 0;
            while (yi > yMin)
            {
                for (var i = 0; i < funcs[0].y.Length; i++)
                    pic[i, coords.GetY(yi)] = subAxisColor;
                yi -= (yMax - yMin) / 5;
            }

            // Оси координат
            for (var i = 0; i < funcs[0].x.Length; i++)
            {
                pic[i, coords.Y0] = axisColor;
                pic[coords.X0, i] = axisColor;
            }

            return pic;
        }

        //public static int[,] FuncToBytesSum(double[] x, double[] y, double[] x3, double[] y3, double delta)
        //{
        //    var size = x.Length;

        //    var xMax = Math.Max(x.Max(), x3.Max());
        //    var xMin = Math.Min(x.Min(), x3.Min());

        //    var yMax = Math.Max(y.Max(), y3.Max());
        //    var yMin = Math.Min(y.Min(), y3.Min());

        //    var pic = new int[size + 1, size + 1];

        //    for (var i = 0; i < pic.GetLength(0); i++)
        //        for (var j = 0; j < pic.GetLength(1); j++)
        //            pic[i, j] = 0xFFFFFF;

        //    var coords = new Coord(xMin, xMax, yMin, yMax, size, size);

        //    // График
        //    for (var i = 0; i < x.Length; i++)
        //    {
        //        pic[coords.GetX(x[i]), coords.GetY(y[i])] = 0x00FF00;
        //        pic[coords.GetX(x3[i]), coords.GetY(y3[i])] = 0x0000FF;
        //    }

        //    double xi;
        //    double yi;
        //    int axisColor = 0x000000;
        //    int subAxisColor = 0xAAAAAA;

        //    // Вертикальные линии
        //    xi = 0;
        //    while (xi <= xMax)
        //    {
        //        for (var i = 0; i < x.Length; i++)
        //            pic[coords.GetX(xi), i] = subAxisColor;
        //        xi += delta;
        //    }

        //    xi = 0;
        //    while (xi > xMin)
        //    {
        //        for (var i = 0; i < x.Length; i++)
        //            pic[coords.GetX(xi), i] = subAxisColor;
        //        xi -= delta;
        //    }

        //    // Горизонтальные линии
        //    yi = 0;
        //    while (yi <= yMax)
        //    {
        //        for (var i = 0; i < y.Length; i++)
        //            pic[i, coords.GetY(yi)] = subAxisColor;
        //        yi += (yMax - yMin) / 5;
        //    }

        //    yi = 0;
        //    while (yi > yMin)
        //    {
        //        for (var i = 0; i < y.Length; i++)
        //            pic[i, coords.GetY(yi)] = subAxisColor;
        //        yi -= (yMax - yMin) / 5;
        //    }

        //    // Оси координат
        //    for (var i = 0; i < x.Length; i++)
        //    {
        //        pic[i, coords.Y0] = axisColor;
        //        pic[coords.X0, i] = axisColor;
        //    }

        //    return pic;
        //}

        public static int[,] FuncToBytes(double[] x, double[] y, double shift)
        {
            var size = x.Length;

            var xScale = (x.Max() - x.Min()) / size;
            var yScale = (y.Max() - y.Min()) / size;

            var pic = new int[size + 1, size + 1];

            for (var i = 0; i < pic.GetLength(0); i++)
                for (var j = 0; j < pic.GetLength(1); j++)
                    pic[i, j] = 0xFFFFFF;

            for (var i = 0; i < pic.GetLength(0); i += size / 20)
                for (var j = 0; j < pic.GetLength(1); j++)
                    pic[i, j] = 0x888888;

            for (var i = 0; i < pic.GetLength(0); i++)
                for (var j = 0; j < pic.GetLength(1); j += size / 20)
                    pic[i, j] = 0x888888;

            var xMin = x.Min();
            var yMax = y.Max();

            for (var i = 0; i < x.Length; i++)
                pic[(int)Math.Floor((x[i] - xMin) / xScale), (int)Math.Floor((yMax - y[i]) / yScale)] = 0;

            return pic;
        }
    }
}
