using DCTestLibrary;
using PhotoTestControl.Models;
using SLPhotoTest.PhotoTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace LynxCameraTest.ViewModel.ChartTestViewModel
{
    public class XRiteChartTestViewModel:LynxChartTestViewModel
    {
        public XRiteChartTestViewModel()
        {
            TestChart = new XRiteColorChart();
            Test = TestXRite;
        }
        List<Result> TestXRite(WriteableBitmap chart)
        {
            var rl = new List<Result>();
            TestChart.setChart(chart);
            var Chart = TestChart as XRiteColorChart;

            rl.Add(new Result()
            {
                Value = Chart.getWhiteBanlance() * 100,
                Name = "Write banlance error percent",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "%"
            });

            rl.Add(new Result()
            {
                Value = Chart.getNoiseNum() * 100,
                Name = "Noise percent",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "%"
            });

            rl.Add(new Result()
            {
                Value = Chart.getColorDistance(),
                Name = "Average color trend error",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "degree"
            });

            return rl;
        }

        public override void ShowDetail(string id)
        {
            if (id.Contains("Average color trend error"))
                ShowColorTrendError();
            if (id.Contains("Noise"))
                ShowNoise();
            if (id.Contains("White banlance"))
                ShowWhiteBanlance();
        }

        void ShowWhiteBanlance()
        {
            WhiteBanlance v = new WhiteBanlance();

            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            if (TestChart.mp.SelectedArea.ContainsKey("Area_4_1"))
            {
                bl.Add(TestChart.mp.SelectedArea["Area_4_1"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_2"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_3"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_4"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_5"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_6"]);
            }
            ShowParameterView(v, bl);
        }

        void ShowNoise()
        {
            Noise v = new Noise();

            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            if (TestChart.mp.SelectedArea.ContainsKey("Area_4_1"))
            {
                bl.Add(TestChart.mp.SelectedArea["Area_4_1"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_2"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_3"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_4"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_5"]);
                bl.Add(TestChart.mp.SelectedArea["Area_4_6"]);
            }
            ShowParameterView(v, bl);
        }

        void ShowColorTrendError()
        {
            ColorTrend v = new ColorTrend();
            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            List<Color> sl = new List<Color>();
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    if ((TestChart as XRiteColorChart).mp.SelectedArea.ContainsKey("Area_" + i.ToString() + "_" + j.ToString()))
                    {
                        bl.Add(TestChart.mp.SelectedArea["Area_" + i.ToString() + "_" + j.ToString()]);
                        sl.Add((TestChart as XRiteColorChart).ColorList[(i - 1) * 6 + j - 1]);
                    }
                }
            }
            v.setStandardColorList(sl);
            ShowParameterView(v, bl);
        }
    }
}
