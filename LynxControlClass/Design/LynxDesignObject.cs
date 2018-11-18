using System;
using System.Windows;
using System.Windows.Input;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Data;

namespace SilverlightLynxControls.Design
{
    public class LynxDesignObject : UserControl, IDesignObject
    {
        public LynxDesignObject()
        {
            PointerPressed += new PointerEventHandler(LynxDesignObject_MouseRightButtonDown);
        }
        public bool IsRightButtonDelete = true;
        void LynxDesignObject_MouseRightButtonDown(object sender, PointerRoutedEventArgs e)
        {
            if (IsRightButtonDelete)
            {
                DeleteDesignObject();
            }
        }
        public string ObjectID { get { return DataObject.ObjectID; } }

        AbstractLFCDataObject _DataObject = null;
        void DataObject_ObjctChanged(object sender, LFCObjectChangedArgs e)
        {
            ReadFromObject();
        }
        public virtual AbstractLFCDataObject DataObject
        {
            get { return _DataObject; }
            set
            {
                if ((value is AbstractLFCDataObject) && (value != _DataObject))
                {
                    if (_DataObject != null)
                    {
                        _DataObject.ObjctChanged -= new LFCObjectChanged(DataObject_ObjctChanged);
                    }
                    if (TitleBlock != null)
                    {
                        TitleBlock.DataContext = value;
                        TitleBlock.SetBinding(TextBox.TextProperty, new Binding() { Path=new PropertyPath("Name")});
                        tp.SetBinding(ToolTip.ContentProperty, new Binding() { Path = new PropertyPath("Memo") });
                        tp.PlacementTarget = TitleBlock;
                    }


                    _DataObject = value;
                    _DataObject.ObjctChanged += new LFCObjectChanged(DataObject_ObjctChanged);
                }
            }
        }
        ToolTip tp = new ToolTip();
        public virtual void ReadFromObject(string id) { }//需要子类实现的方法,从ID恢复对象数据
        public virtual void ReadFromObject() { }

        public virtual void ClearAllEvent()
        {
            PointerPressed -= new PointerEventHandler(LynxDesignObject_MouseRightButtonDown);
        }//需要子类实现

        public Brush DesignObjectBorderBrush { get; set; }
        public Brush DesignObjectBackBrush { get; set; }
        public Brush DesignObjectHoverBrush { get; set; }
        public Brush DesignObjectActiveBrush { get; set; }

        public TextBox TitleBlock;

        public virtual string ControlMemo
        {
            get;
            set;
        }

        public virtual string ControlName
        {
            get;
            set;
        }

        public FrameworkElement getControl()
        {
            return this;
        }

        public virtual void LoadLogicViewInfor(){}

        public virtual void SaveLogicViewInfor() { }

        public IDesignCanvas designCanvas
        {
            get;
            set;
        }

        public virtual void WriteToObject() { }

        public virtual void Active(){}


        public virtual void DeActive() { }


        bool _IsErrorData = false;
        public bool IsErrorData
        {
            get { return _IsErrorData; }
            set
            {
                _IsErrorData = value;
                if (value)
                {
                    //Background = new SolidColorBrush(Color.FromArgb(150, 170, 0, 0));//设置特殊的背景色
                    Opacity = 0.37;//增加透明度
                }
                else
                {
                    Opacity = 1;
                }
            }
        }

        public async void DeleteDesignObject()//右键一般用来删除
        {
            var msd = new MessageDialog("确定要删除选中的对象：" + Name + "？", "删除确认");
            msd.Commands.Add(
                new UICommand("OK", (o) =>
                    {
                        designCanvas.RemoveControl(this);
                    }));
            msd.Commands.Add(
                new UICommand("Cancel", (o) =>
                {        
                }));
            await msd.ShowAsync();
            
        }
    }
}
