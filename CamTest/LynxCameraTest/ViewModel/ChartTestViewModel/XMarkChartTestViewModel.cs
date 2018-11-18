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
    public class XMarkChartTestViewModel : LynxChartTestViewModel
    {
        public XMarkChartTestViewModel()
        {
            TestChart = new XMarkChart();
            Test = TestXMark;
        }

        List<Result> TestXMark(WriteableBitmap chart)
        {
            var rl = new List<Result>();
            TestChart.setChart(chart);
            var Chart = TestChart as XMarkChart;

            rl.Add(new Result()
            {
                Value = Chart.getAberration() * 100,
                Name = "Aberration",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "%"
            });

            rl.Add(new Result()
            {
                Value = Chart.getBrightChanges() * 100,
                Name = "Bright Changes",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "%"
            });

            rl.Add(new Result()
            {
                Value = Chart.getColorDis(),
                Name = "Average color trend error",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "degree"
            });

            rl.Add(new Result()
            {
                Value = Chart.getHEdgeDispersiveness(),
                Name = "Center Horizontal Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });
            rl.Add(new Result()
            {
                Value = Chart.getVEdgeDispersiveness(),
                Name = "Border Vertical Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });

            rl.Add(new Result()
            {
                Value = Chart.getHEdgeResoveLines(),
                Name = "Center Horizontal Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = Chart.getVEdgeResoveLines(),
                Name = "Border Vertical Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = Chart.getLatitude(),
                Name = "Photo Latitude",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "degree"
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
                Value = Chart.getPurplePercent() * 100,
                Name = "Purple pixel percent",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "%"
            });

            rl.Add(new Result()
            {
                Value = Chart.getWaveQ() * 100,
                Name = "Wave bright error percent",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "%"
            });

            rl.Add(new Result()
            {
                Value = Chart.getWhiteBanlance() * 100,
                Name = "White banlance error percent",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "%"
            });

            return rl;
        }


        public override void ShowDetail(string id)
        {
            if (id.Contains("Aberration"))
                ShowAberration();
            if (id.Contains("Bright Changes"))
                ShowBrightChanges();
            if (id.Contains("Average color trend error"))
                ShowColorTrendError();
            if (id.Contains("Horizontal Dispersiveness"))
                ShowHDispersiveness();
            if (id.Contains("Vertical Dispersiveness"))
                ShowVDispersiveness();
            if (id.Contains("Horizontal Resolution"))
                ShowHDispersiveness();
            if (id.Contains("Vertical Resolution"))
                ShowVDispersiveness();
            if (id.Contains("Latitude"))
                ShowLatitude();
            if (id.Contains("Noise"))
                ShowNoise();
            if (id.Contains("Purple"))
                ShowPurple();
            if (id.Contains("Wave"))
                ShowWaveQ();
            if (id.Contains("White banlance"))
                ShowWhiteBanlance();
        }

        void ShowAberration()
        {
            Aberration v = new Aberration();
            ShowParameterView(v, TestChart.getCorrectPhoto());
        }

        void ShowBrightChanges()
        {
            BrightDistance v = new BrightDistance();
            List<WriteableBitmap> bl = new List<WriteableBitmap>();

            bl.Add((TestChart as XMarkChart).CropPhoto);
            if (TestChart.mp.SelectedArea.ContainsKey("AreaHBright"))
            {
                bl.Add(TestChart.mp.SelectedArea["AreaHBright"]);
                bl.Add(TestChart.mp.SelectedArea["AreaVBright"]);
            }
            ShowParameterView(v, bl);

        }

        void ShowColorTrendError()
        {
            ColorTrend v = new ColorTrend();
            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            List<Color> sl = new List<Color>();
            if (TestChart.mp.SelectedArea.ContainsKey("AreaColor_7"))
            {
                for (int i = 7; i < 19; i++)
                {
                    bl.Add(TestChart.mp.SelectedArea["AreaColor_" + i.ToString()]);
                    sl.Add((TestChart as XMarkChart).ColorList[i - 1]);
                }
            }
            v.setStandardColorList(sl);
            ShowParameterView(v, bl);
        }

        void ShowHDispersiveness()
        {
            Dispersiveness v = new Dispersiveness();
            v.IsV = false;
            ShowParameterView(v, "AreaHEdge");
        }

        void ShowVDispersiveness()
        {
            Dispersiveness v = new Dispersiveness();
            v.IsV = true;
            ShowParameterView(v, "AreaVEdge");
        }

        void ShowHMTF()
        {
            SFRResolution v = new SFRResolution();
            v.IsV = true;
            ShowParameterView(v, "AreaVEdge");
        }

        void ShowVMTF()
        {
            SFRResolution v = new SFRResolution();
            v.IsV = false;
            ShowParameterView(v, "AreaHEdge");
        }

        void ShowLatitude()
        {
            Latitude v = new Latitude();

            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            if (TestChart.mp.SelectedArea.ContainsKey("AreaBW_1"))
            {
                for (int i = 1; i < 23; i++)
                {
                    bl.Add(TestChart.mp.SelectedArea["AreaBW_" + i.ToString()]);
                }
            }
            ShowParameterView(v, bl);
        }

        void ShowNoise()
        {
            Noise v = new Noise();

            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            if (TestChart.mp.SelectedArea.ContainsKey("AreaBW_1"))
            {
                for (int i = 1; i < 23; i++)
                {
                    bl.Add(TestChart.mp.SelectedArea["AreaBW_" + i.ToString()]);
                }
            }
            ShowParameterView(v, bl);
        }

        void ShowPurple()
        {
            PurplePercent v = new PurplePercent();

            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            if (TestChart.mp.SelectedArea.ContainsKey("AreaPurple_1"))
            {
                bl.Add(TestChart.mp.SelectedArea["AreaPurple_1"]);
                bl.Add(TestChart.mp.SelectedArea["AreaPurple_2"]);
                bl.Add(TestChart.mp.SelectedArea["AreaPurple_3"]);
                bl.Add(TestChart.mp.SelectedArea["AreaPurple_4"]);
            }
            ShowParameterView(v, bl);
        }

        void ShowWaveQ()
        {
            Latitude v = new Latitude();

            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            if (TestChart.mp.SelectedArea.ContainsKey("AreaWave_1"))
            {
                bl.Add(TestChart.mp.SelectedArea["AreaWave_1"]);
                bl.Add(TestChart.mp.SelectedArea["AreaWave_2"]);
                bl.Add(TestChart.mp.SelectedArea["AreaWave_3"]);
                bl.Add(TestChart.mp.SelectedArea["AreaWave_4"]);
                bl.Add(TestChart.mp.SelectedArea["AreaWave_5"]);
            }
            ShowParameterView(v, bl);
        }

        void ShowWhiteBanlance()
        {
            WhiteBanlance v = new WhiteBanlance();

            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            if (TestChart.mp.SelectedArea.ContainsKey("AreaBW_1"))
            {
                for (int i = 1; i < 23; i++)
                {
                    bl.Add(TestChart.mp.SelectedArea["AreaBW_" + i.ToString()]);
                }
            }
            ShowParameterView(v, bl);
        }
    }
}
