using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tools;
using System.Drawing;
using System.Xml;
using System.Drawing.Imaging;
using System.IO;

namespace DecisionMethods
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                if (args[0] == "/s")
                {
                    var myImage = new MyImage();
                    myImage.Bitmap = new Bitmap(args[1]);
                    var dm = new DecisionMethods(myImage);

                    dm.SaveInMask = true;
                    dm.Pollute(decimal.Parse(args[2]), false);
                    var savedMask = Tools.ArrayTools.CopyArray<bool>(dm.PollutedMask);

                    dm.FindPixels(int.Parse(args[3]), double.Parse(args[4].Replace(".", ",")), double.Parse(args[5].Replace(".", ",")), int.Parse(args[6]) == 1);
                    var savedMask2 = Tools.ArrayTools.CopyArray<bool>(dm.PollutedMask);

                    bool addHeader = !File.Exists("Statistics_Mask.csv");

                    string log = string.Empty;
                    StreamWriter fs = new StreamWriter("Statistics_Mask.csv", true);
                    if (addHeader)
                        log += string.Join(Tools.Consts.CSVDivider, "File", "Pollution percent", "M", "N", "K", "Use color", "Use mask", "MR", "NR", "Broken", "Found", "Match", "Not found", "Wrong found", "MM Orig", "MSE Orig", "DON Orig", "MM Pollute", "MSE Pollute", "DON Pollute", "MM Restored", "MSE Restored", "DON Restored") + "\n";
                    log += string.Join(Tools.Consts.CSVDivider, args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]) + Tools.Consts.CSVDivider;
                    log += string.Join(Tools.Consts.CSVDivider, Tools.Metrics.MatrixDifference(savedMask, savedMask2, MetricsMode.CSVSimple));
                    fs.WriteLine(log);
                    fs.Close();
                }
                else if (args[0] == "/r")
                {
                    // Initialize
                    var myImage = new MyImage();
                    myImage.Bitmap = new Bitmap(args[1]);
                    var dm = new DecisionMethods(myImage);

                    var metricsOrig = dm.GetMetrics(MetricsMode.CSVSimple);

                    // Pollute
                    dm.SaveInMask = true;
                    dm.Pollute(decimal.Parse(args[2]), false);
                    var savedMask = Tools.ArrayTools.CopyArray<bool>(dm.PollutedMask);
                    dm.MyImage.Bitmap.Save(args[1] + "_polluted.png", ImageFormat.Png);

                    var metricsPolluted = dm.GetMetrics(MetricsMode.CSVSimple);

                    // Find Pixels
                    dm.SaveInMask = int.Parse(args[7]) == 1;
                    dm.FindPixels(int.Parse(args[3]), double.Parse(args[4].Replace(".", ",")), double.Parse(args[5].Replace(".", ",")), int.Parse(args[6]) == 1);
                    var savedMask2 = Tools.ArrayTools.CopyArray<bool>(dm.PollutedMask);
                    ImageTransform.BoolToBitmap(dm.PollutedMask).Save(args[1] + "_mask.png", ImageFormat.Png);

                    // Restore
                    var restoredImage = dm.RestorePixels(int.Parse(args[8]), int.Parse(args[9]));
                    var metricsRestored = Tools.Metrics.GetUnifiedMetrics(restoredImage, MetricsMode.CSVSimple);
                    restoredImage.Bitmap.Save(args[1] + "_restore.png", ImageFormat.Png);

                    var restoredImageOld = dm.RestorePixelsOld(4);
                    var metricsRestoredOld = Tools.Metrics.GetUnifiedMetrics(restoredImage, MetricsMode.CSVSimple);
                    restoredImageOld.Bitmap.Save(args[1] + "_restoreOld.png", ImageFormat.Png);

                    bool addHeader = !File.Exists("Statistics_Restore.csv");

                    string log = string.Empty;
                    StreamWriter fs = new StreamWriter("Statistics_Restore.csv", true);
                    if (addHeader)
                        log += string.Join(Tools.Consts.CSVDivider, "File", "Pollution percent", "M", "N", "K", "Use color", "Use mask", "MR", "NR", "Broken", "Found", "Match", "Not found", "Wrong found", "MM Orig", "MSE Orig", "DON Orig", "MM Pollute", "MSE Pollute", "DON Pollute", "MM Restored", "MSE Restored", "DON Restored", "MM Restored Old", "MSE Restored Old", "DON Restored Old") + "\n";
                    log += string.Join(Tools.Consts.CSVDivider, args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]) + Tools.Consts.CSVDivider;
                    log += string.Join(Tools.Consts.CSVDivider, Tools.Metrics.MatrixDifference(savedMask, savedMask2, MetricsMode.CSVSimple)) + Tools.Consts.CSVDivider;
                    log += metricsOrig + Tools.Consts.CSVDivider;
                    log += metricsPolluted + Tools.Consts.CSVDivider;
                    log += metricsRestored + Tools.Consts.CSVDivider;
                    log += metricsRestoredOld;
                    fs.WriteLine(log);
                    fs.Close();
                }
            }
        }
    }
}
