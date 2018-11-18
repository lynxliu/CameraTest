using LFC.common;
using LynxCameraTest.Model;
using PhotoTestControl.Models;
using SilverlightDCTestLibrary;
using SLPhotoTest.PhotoTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace LynxCameraTest.ViewModel.ChartTestViewModel
{
    public class ISO12233ExTestViewModel : LynxChartTestViewModel
    {
        public ISO12233ExTestViewModel()
        {
            TestChart = new ISO12233ExChart();
            Test = TestISO12233Ex;
        }

        List<Result> TestISO12233Ex(WriteableBitmap chart)
        {
            var rl = new List<Result>();
            TestChart.setChart(chart);
            PreProcess();
            rl.Add(new Result()
            {
                Value = (TestChart as ISO12233ExChart).getLPResoveLines(),
                Name = "Raylei Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = (TestChart as ISO12233ExChart).getHEdgeResoveLines(),
                Name = "Horizontal Resolution(HMTF)",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = (TestChart as ISO12233ExChart).getVEdgeResoveLines(),
                Name = "Vertical Resolution(VMTF)",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = (TestChart as ISO12233ExChart).getHDispersiveness(),
                Name = "Horizontal Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });
            rl.Add(new Result()
            {
                Value = (TestChart as ISO12233ExChart).getVDispersiveness(),
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
