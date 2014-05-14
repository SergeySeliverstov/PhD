using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tools;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace DataMining
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                var myImage = new MyImage();
                myImage.Bitmap = new Bitmap(args[0]);

                var dataMining = new DataMining();
                //string log = string.Format("Файл: {0}\nМетод: {1}\nШаблон: {2}\nОбрезать пиксели: {3}\nДостоверность, %: {4}\nДлина шаблона: {5}\nПроцент загрязнения: {6}\nИспользование маски: {7}\n", args);                

                dataMining.CollectionMethod = int.Parse(args[1]);
                dataMining.Template = int.Parse(args[2]);
                dataMining.CropPixels = int.Parse(args[3]) == 1;
                dataMining.MaxAccuracy = int.Parse(args[4]);
                dataMining.MaxDepth = int.Parse(args[5]);
                int pollutedPercent = 0;
                if (int.TryParse(args[6], out pollutedPercent))
                    dataMining.PollutePercent = pollutedPercent;
                else
                    dataMining.PollutePercent = -1;
                dataMining.UseMask = int.Parse(args[7]) == 1;
                dataMining.WSM = int.Parse(args[8]) == 1;
                dataMining.UseLimit = int.Parse(args[9]) == 1;

                dataMining.MyImage = myImage;
                //dataMining.UpdateLog += (o, eo) =>
                //{
                //    log += eo.Object;
                //};

                string metricsOrig = dataMining.GetMetricsText(MetricsMode.CSVSimple);
                if (dataMining.PollutePercent != -1)
                {
                    dataMining.Pollute(int.Parse(args[10]) == 1);
                    dataMining.MyImage.Bitmap.Save(args[0] + "_polluted.png", ImageFormat.Png);
                }
                else
                {
                    var pollutedImage = new MyImage();
                    pollutedImage.Bitmap = new Bitmap(args[6]);
                    for (int i = 0; i < pollutedImage.ImageWidth; i++)
                        for (int j = 0; j < pollutedImage.ImageHeight; j++)
                            dataMining.MyImage.ImageBytes[i, j] = pollutedImage.ImageBytes[i, j];
                    dataMining.PollutedImage = Tools.ArrayTools.CopyArray<int>(pollutedImage.ImageBytes);
                    dataMining.PollutedMask = new bool[pollutedImage.ImageWidth, pollutedImage.ImageHeight];
                    dataMining.PollutedMaskOriginal = new bool[pollutedImage.ImageWidth, pollutedImage.ImageHeight];
                }

                string metricsPolluted = dataMining.GetMetricsText(MetricsMode.CSVSimple);
                dataMining.CreatePairs();

                if (!dataMining.UseMask)
                    dataMining.FindPixels();

                bool[,] maskOriginal;
                bool[,] maskStatistics;
                dataMining.GetMasks(out maskOriginal, out maskStatistics);

                bool[,] maskMiss = new bool[maskOriginal.GetLength(0), maskOriginal.GetLength(1)];
                bool[,] maskFalse = new bool[maskOriginal.GetLength(0), maskOriginal.GetLength(1)];
                for (int i = 0; i < maskOriginal.GetLength(0); i++)
                    for (int j = 0; j < maskOriginal.GetLength(1); j++)
                    {
                        maskMiss[i, j] = maskOriginal[i, j] && !maskStatistics[i, j];
                        maskFalse[i, j] = !maskOriginal[i, j] && maskStatistics[i, j];
                    }

                ImageTransform.BoolToBitmap(maskOriginal).Save(args[0] + "_maskStatistics.png", ImageFormat.Png);
                ImageTransform.BoolToBitmap(maskStatistics).Save(args[0] + "_maskOriginal.png", ImageFormat.Png);
                ImageTransform.BoolToBitmap(maskMiss).Save(args[0] + "_maskMiss.png", ImageFormat.Png);
                ImageTransform.BoolToBitmap(maskFalse).Save(args[0] + "_maskFalse.png", ImageFormat.Png);

                dataMining.RestoreImage();
                dataMining.MyImage.Bitmap.Save(args[0] + "_restored.png", ImageFormat.Png);
                string metricsRestored = dataMining.GetMetricsText(MetricsMode.CSVSimple);

                string log = string.Empty;

                if (!File.Exists("Statistics.csv"))
                {
                    log += string.Join(Tools.Consts.CSVDivider, "File", "Method", "Template", "Crop pixels", "Accuracy", "Transaction length", "Pollution percent", "Use mask", "WSM", "Limit", "SP") + Tools.Consts.CSVDivider;
                    log += string.Join(Tools.Consts.CSVDivider, "MM Orig", "MSE Orig", "DON Orig", "MM Pollute", "MSE Pollute", "DON Pollute", "MM Restored", "MSE Restored", "DON Restored", "Polluted count", "Find", "Miss", "False") + "\n";
                }
                log += string.Join(Tools.Consts.CSVDivider, args) + Tools.Consts.CSVDivider;
                log += string.Join(Tools.Consts.CSVDivider, metricsOrig, metricsPolluted, metricsRestored) + Tools.Consts.CSVDivider;
                log += dataMining.GetPollutionStatistics();

                StreamWriter fs = new StreamWriter("Statistics.csv", true);
                fs.WriteLine(log);
                fs.Close();

                //StreamWriter fs = new StreamWriter(args[0] + "_log.txt", false);
                //fs.WriteLine(log);
                //fs.Close();
            }
        }
    }
}
