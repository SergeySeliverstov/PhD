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
                var methodsCount = 6;
                var criteriesCount = 5;
                var statisticsFileName = "Statistics.xml";
                var optimizedStatisticsFileName = "OptimizedStatistics.xml";
                var criterionsFileName = "Criterions.xml";
                var optimizedCriterionsFileName = "OptimizedCriterions.xml";

                if (args[0] == "/m")
                {
                    var myImage = new MyImage();
                    myImage.Bitmap = new Bitmap(args[1]);
                    var dm = new DecisionMethods(myImage);

                    List<Matrix> statistics = XmlTools.Load<List<Matrix>>(statisticsFileName);
                    if (statistics == null)
                    {
                        statistics = new List<Matrix>();
                        for (int i = 0; i < criteriesCount; i++)
                            statistics.Add(new Matrix(methodsCount, methodsCount));
                    }
                    Matrix criterions = XmlTools.Load<Matrix>(criterionsFileName);
                    if (criterions == null)
                        criterions = new Matrix(criteriesCount, criteriesCount);

                    dm.SaveInMask = true;
                    dm.Pollute(decimal.Parse(args[2]));
                    for (int method = 0; method < methodsCount; method++)
                    {
                        var restoredImage = dm.RestoreImage(method);
                        for (int i = 1; i < dm.MyImage.ImageWidth - 1; i++)
                            for (int j = 1; j < dm.MyImage.ImageHeight - 1; j++)
                                if (dm.PollutedMask[i, j])
                                {
                                    var criterion = dm.CheckForCriterion(dm.MyImage.ImageBytes, i, j);
                                    var color = new MyColor(restoredImage.ImageBytes[i, j]);
                                    var origColor = new MyColor(restoredImage.OriginalImageBytes[i, j]);
                                    statistics[(int)criterion][method, method] += Math.Abs((decimal)(origColor.R + origColor.G + origColor.B) / (decimal)3 - (decimal)(color.R + color.G + color.B) / (decimal)3);
                                    criterions[(int)criterion, (int)criterion] += 1;
                                }
                    }

                    XmlTools.Save<List<Matrix>>(statisticsFileName, statistics);
                    var optimizedStatistics = MatrixTools.PrepareMatrix(statistics);
                    XmlTools.Save<List<Matrix>>(optimizedStatisticsFileName, optimizedStatistics);

                    XmlTools.Save<Matrix>(criterionsFileName, criterions);
                    var optimizedCriterions = MatrixTools.PrepareMatrix(criterions);
                    XmlTools.Save<Matrix>(optimizedCriterionsFileName, optimizedCriterions);
                }
                else if (args[0] == "/r")
                {
                    var myImage = new MyImage();
                    myImage.Bitmap = new Bitmap(args[1]);
                    var dm = new DecisionMethods(myImage);

                    var optimizedStatistics = XmlTools.Load<List<Matrix>>(optimizedStatisticsFileName);
                    var statistics = XmlTools.Load<List<Matrix>>(statisticsFileName);
                    var optimizedCriterions = XmlTools.Load<Matrix>(optimizedCriterionsFileName);
                    var criterions = XmlTools.Load<Matrix>(criterionsFileName);

                    var metricsOrig = dm.GetMetrics(MetricsMode.CSVSimple);

                    dm.SaveInMask = int.Parse(args[3]) == 1;
                    dm.Pollute(decimal.Parse(args[2]));
                    dm.MyImage.Bitmap.Save(args[1] + "_polluted.png", ImageFormat.Png);

                    var metricsPolluted = dm.GetMetrics(MetricsMode.CSVSimple);

                    if (int.Parse(args[3]) == 0)
                    {
                        dm.SaveInMask = true;
                        dm.FindPixels();
                    }

                    //var restoredImage = dm.RestoreImage(criterions, optimizedStatistics);
                    var restoredImage = dm.RestoreImageByStatistics(criterions, statistics);
                    restoredImage.Bitmap.Save(args[1] + "_restored.png", ImageFormat.Png);

                    var metricsRestored = Tools.Metrics.GetUnifiedMetrics(restoredImage, MetricsMode.CSVSimple);

                    bool addHeader = !File.Exists("Statistics.csv");

                    string log = string.Empty;
                    StreamWriter fs = new StreamWriter("Statistics.csv", true);
                    if (addHeader)
                        log = string.Join(Tools.Consts.CSVDivider, "File", "Pollution percent", "Use mask", "MM Orig", "MSE Orig", "DON Orig", "MM Pollute", "MSE Pollute", "DON Pollute", "MM Restored", "MSE Restored", "DON Restored") + "\n";
                    log += string.Join(Tools.Consts.CSVDivider, args[1], args[2], args[3]) + Tools.Consts.CSVDivider;
                    log += string.Join(Tools.Consts.CSVDivider, metricsOrig, metricsPolluted, metricsRestored);
                    fs.WriteLine(log);
                    fs.Close();
                }
            }
        }
    }
}
