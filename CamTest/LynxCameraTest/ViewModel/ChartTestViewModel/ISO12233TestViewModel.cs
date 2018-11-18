using DCTestLibrary;
using PhotoTestControl.Models;
using SLPhotoTest.PhotoTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace LynxCameraTest.ViewModel.ChartTestViewModel
{
    public class ISO12233TestViewModel : LynxChartTestViewModel
    {
        public ISO12233TestViewModel()
        {
            TestChart = new ISO12233Chart();
            Test = TestISO12233;
            Title = "ISO12233 Chart";
        }

        List<Result> TestISO12233(WriteableBitmap chart)
        {
            var rl = new List<Result>();
            TestChart.setChart(chart);
            var Chart = TestChart as ISO12233Chart;

            rl.Add(new Result()
            {
                Value = Chart.getLPResoveLines(),
                Name = "Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH",
            });

            rl.Add(new Result()
            {
                Value = Chart.getHEdgeResoveLines(),
                Name = "Horizontal Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH",
            });

            rl.Add(new Result()
            {
                Value = Chart.getVEdgeResoveLines(),
                Name = "Vertical Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = Chart.getHDispersiveness(),
                Name = "Horizontal Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });
            rl.Add(new Result()
            {
                Value = Chart.getVDispersiveness(),
                Name = "Vertical Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });

            return rl;
        }


        public override void ShowDetail(string id)
        {
            if (id.Contains("Raylei"))
                ShowRaleiView();
            if (id.Contains("HMTF"))
                ShowHMTF();
            if (id.Contains("VMTF"))
                ShowVMTF();
            if (id.Contains("Horizontal Dispersiveness"))
                ShowHDispersiveness();
            if (id.Contains("Vertical Dispersiveness"))
                ShowVDispersiveness();
        }

        void ShowRaleiView()
        {
            RayleiResolution v = new RayleiResolution();

            if (TestChart != null &&
                TestChart.ProcessInfor.ContainsKey("RayleiResolutionIsLeft")&& 
                Convert.ToBoolean(TestChart.ProcessInfor["RayleiResolutionIsLeft"]))
            {
                v.IsLeft = true;
            }
            else
            {
                v.IsLeft = false;
            }
            ShowParameterView(v, "RayleiResolutionLArea");
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

        void ShowHDispersiveness()
        {
            Dispersiveness v = new Dispersiveness();
            v.IsV = true;
            ShowParameterView(v, "AreaVEdge");
        }

        void ShowVDispersiveness()
        {
            Dispersiveness v = new Dispersiveness();
            v.IsV = false;
            ShowParameterView(v, "AreaHEdge");
        }
    }
}
