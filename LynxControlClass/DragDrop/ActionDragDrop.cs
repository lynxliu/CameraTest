using SilverlightLFC.common;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace SilverlightLynxControls.DragDrop
{
    public delegate void Drag();
    public delegate void Drop();
    public delegate void Active();
    public delegate void DeActive();

    public class LynxDragDropData//包含所有拖放的图标和数据，也包括拖放的事件
    {
        #region DragDropEvent//拖放事件，标志拖放操作开始，Drop对象要开始Enable
        public static event EventHandler BeginDragDropEvent;
        public static event EventHandler EndDragDropEvent;

        public static void SendBeginDragEvent(object sender){
            if (BeginDragDropEvent != null)
            {
                BeginDragDropEvent(sender, new EventArgs());
            }
        }
        public static void SendEndDragEvent(object sender)
        {
            if (EndDragDropEvent != null)
            {
                EndDragDropEvent(sender, new EventArgs());
            }
        }
        #endregion

        #region Fields//支持拖放功能需要的字段信息
        public static bool IsActive = false;//默认是非拖放状态
        public static Image currentIcon = null;//拖放过程里面显示的图片
        public static object Data = null;//拖放过程里面附加的数据
        public static Panel DragDropCanvas;//支持拖放操作的桌面画布

        public static ActionDrag currentDragObject = null;//当前的dragObject
        public static ActionDrop currentDropObject = null;//当前的DropObject        
        #endregion
        
        public static void DragDropCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {//任何时候在画布上面进行MouseUp事件结束一次拖放。
            if (currentDropObject != null)
            {
                currentDropObject.DropPoint = e.GetCurrentPoint(currentDropObject.DropControl).Position;
                //currentDropObject.OnDrop();
            }
            EndDragDrop();
        }
        public static void DragDropCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if ((LynxDragDropData.DragDropCanvas != null) && currentDragObject!=null)
            {
                Canvas.SetZIndex(currentIcon, 225);//225为整个的拖放层
                if (!LynxDragDropData.DragDropCanvas.Children.Contains(LynxDragDropData.currentIcon))
                {
                    LynxDragDropData.DragDropCanvas.Children.Add(LynxDragDropData.currentIcon);
                }
                Canvas.SetLeft(LynxDragDropData.currentIcon, e.GetCurrentPoint(LynxDragDropData.DragDropCanvas).Position.X + 5);
                Canvas.SetTop(LynxDragDropData.currentIcon, e.GetCurrentPoint(LynxDragDropData.DragDropCanvas).Position.Y + 5);
                LynxDragDropData.currentIcon.Opacity = 0.7;
            }
        }

        public static void BeginDragDrop(ActionDrag ad)//通知开始拖放操作
        {
            if (DragDropCanvas == null) { return; }
            currentDragObject = ad;
            DragDropCanvas.PointerMoved += new PointerEventHandler(LynxDragDropData.DragDropCanvas_PointerMoved);
            DragDropCanvas.PointerReleased += new PointerEventHandler(DragDropCanvas_PointerReleased);
            SendBeginDragEvent(ad);
        }

        public static void EndDragDrop()//通知结束拖放操作
        {
            if (DragDropCanvas == null) { return; }
            DragDropCanvas.PointerMoved -= new PointerEventHandler(LynxDragDropData.DragDropCanvas_PointerMoved);
            DragDropCanvas.PointerReleased -= new PointerEventHandler(DragDropCanvas_PointerReleased);

            IsActive = false;
            if (DragDropCanvas.Children.Contains(LynxDragDropData.currentIcon))
            {
                DragDropCanvas.Children.Remove(LynxDragDropData.currentIcon);
            }
            currentIcon = null;
            currentDragObject = null;
            if (currentDropObject != null)
            {
                currentDropObject.OnDrop();
                SendEndDragEvent(currentDropObject);
            }
        }


    }

    public class ActionDrag//支持拖动操作，只要设置了包含，就可以支持拖放
    {
        Drag OnDrag;//自己定义拖的时候需要进行的操作

        public Active activeControl;
        public DeActive deactiveControl;
        public Point DragPoint = new Point(0, 0);//起始拖动的点，相对于控件的位置
        FrameworkElement DragControl;
        public bool Enable = false;

        //public bool Enable { get; set; }
        public ActionDrag(FrameworkElement dragControl,Image dataIcon,Drag dragAction)
        {
            DragControl = dragControl;
            OnDrag = dragAction;
        }

        public void EnableDrag()
        {
            Enable = true;
            DragControl.PointerPressed += new PointerEventHandler(DragControl_PointerPressed);
            DragControl.PointerReleased += new PointerEventHandler(DragControl_PointerReleased);
            DragControl.PointerEntered += new PointerEventHandler(DragControl_PointerEntered);
            DragControl.PointerExited += new PointerEventHandler(DragControl_PointerExited);
        }

        void DragControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (deactiveControl != null && Enable)
            {
                deactiveControl();
            }
        }

        void DragControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (activeControl != null && Enable)
            {
                activeControl();
            }
            
        }



        void DragControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (sender.Equals(DragControl))
            {
                LynxDragDropData.IsActive = false;
            }
        }

        async void DragControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            DragPoint = e.GetCurrentPoint(DragControl).Position;
            LynxDragDropData.BeginDragDrop(this);
            OnDrag();
            //if (LynxDragDropData.currentIcon == null)
            //{
            //    WriteableBitmap wb =await WriteableBitmapHelper.Snapshot(DragControl);
            //    LynxDragDropData.currentIcon = new Image();
            //    LynxDragDropData.currentIcon.Source = wb;
            //}
        }

        public void DisableDrag()
        {
            DragControl.PointerPressed -= new PointerEventHandler(DragControl_PointerPressed);
            DragControl.PointerReleased -= new PointerEventHandler(DragControl_PointerReleased);
            DragControl.PointerEntered -= new PointerEventHandler(DragControl_PointerEntered);
            DragControl.PointerExited -= new PointerEventHandler(DragControl_PointerExited);
            Enable = false;
        }

    }

    public class ActionDrop//支持拖动操作，只要设置了包含，就可以支持拖放
    {
        public Drop OnDrop;//自己定义拖的时候需要进行的操作

        public Active activeControl;
        public DeActive deactiveControl;

        public Point DropPoint = new Point(0, 0);//放置的点，相对于放置控件的位置

        public FrameworkElement DropControl;


        public bool Enable = false;
        public ActionDrop(FrameworkElement dropControl, Image dataIcon, Drop dropAction)
        {
            DropControl = dropControl;
            OnDrop = dropAction;
        }

        public void EnableDrop()
        {
            //DropControl.PointerPressed += new PointerEventHandler(DropControl_PointerPressed);
            //DropControl.PointerReleased += new PointerEventHandler(DropControl_PointerReleased);
            //LynxDragDropData.DragDropCanvas.PointerMoved += new PointerEventHandler(DragDropCanvas_PointerMoved);
            Enable = true;
            DropControl.PointerEntered += new PointerEventHandler(DropControl_PointerEntered);
            DropControl.PointerExited += new PointerEventHandler(DropControl_PointerExited);
            DropControl.PointerMoved += new PointerEventHandler(DropControl_PointerMoved);
        }

        void DropControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!Enable) { return; }
            LynxDragDropData.currentDropObject = this;
        }

        void DropControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!Enable) { return; }
            LynxDragDropData.currentDropObject = null;
            if (deactiveControl != null)
            {
                deactiveControl();
            }
        }

        void DropControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!Enable) { return; }
            LynxDragDropData.currentDropObject = this;
            if (activeControl != null)
            {
                activeControl();
            }

        }

        //void DragDropCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        //{
        //    if ((LynxDragDropData.DragDropCanvas != null) && LynxDragDropData.IsActive)
        //    {
        //        if (LynxDragDropData.DragDropCanvas.Children.Contains(LynxDragDropData.currentIcon))
        //        {

        //        }
        //        else
        //        {
        //            LynxDragDropData.DragDropCanvas.Children.Add(LynxDragDropData.currentIcon);
        //        }
        //        Canvas.SetLeft(LynxDragDropData.currentIcon, e.GetPosition(LynxDragDropData.DragDropCanvas).X - 5);
        //        Canvas.SetTop(LynxDragDropData.currentIcon, e.GetPosition(LynxDragDropData.DragDropCanvas).Y - 5);
        //        LynxDragDropData.currentIcon.Opacity = 0.7;
        //    }
        //}

        //void DropControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    DropPoint = e.GetPosition(DropControl);
        //    if (sender.Equals(DropControl))
        //    {
        //        LynxDragDropData.IsActive = false;
        //    }
        //    else
        //    {
        //        OnDrop();
        //    }
        //    FinishDrop();
        //}

        //public void FinishDrop()
        //{
        //    //DragControl.PointerPressed -= new PointerEventHandler(DragControl_PointerPressed);
        //    //DragControl.PointerReleased -= new PointerEventHandler(DragControl_PointerReleased);
        //    LynxDragDropData.DragDropCanvas.PointerMoved -= new PointerEventHandler(LynxDragDropData.DragDropCanvas_PointerMoved);
        //    //DragControl.PointerEntered -= new PointerEventHandler(DragControl_PointerEntered);
        //    //DragControl.PointerExited -= new PointerEventHandler(DragControl_PointerExited);
        //    LynxDragDropData.currentIcon = null;
        //    LynxDragDropData.Data = null;
        //    LynxDragDropData.IsActive = false;
        //}

    }


    //public class ActionDrag
    //{
    //    public Canvas MoveArea;

    //    public Panel SurportDragArea;
    //    IDragSurport SourceObj;
    //    public bool Enable;
    //    public ActionDrag(IDragSurport dragObj, Panel ActionArea,Canvas p)
    //    {
    //        MoveArea = p;
    //        SurportDragArea = ActionArea;
    //        SourceObj = dragObj;
    //        SurportDragArea.PointerPressed += new PointerEventHandler(SurportDragArea_PointerPressed);
    //        SurportDragArea.PointerMoved += new PointerEventHandler(SurportDragArea_PointerMoved);
    //        SurportDragArea.PointerReleased += new PointerEventHandler(SurportDragArea_PointerReleased);
    //        MoveArea.PointerMoved += new PointerEventHandler(MoveArea_PointerMoved);
    //    }

    //    void SurportDragArea_PointerReleased(object sender, PointerRoutedEventArgs e)
    //    {
    //        IsBeginDrag = false;
    //        if (dragObjControl != null)
    //        {
    //            if (MoveArea.Children.Contains(dragObjControl))
    //            {
    //                MoveArea.Children.Remove(dragObjControl);
    //                dragObjControl = null;
    //            }
    //        }
    //    }

    //    void SurportDragArea_PointerMoved(object sender, PointerRoutedEventArgs e)
    //    {

    //    }
    //    bool IsBeginDrag = false;
    //    FrameworkElement dragObjControl;
    //    void SurportDragArea_PointerPressed(object sender, PointerRoutedEventArgs e)
    //    {
    //        if (Enable)
    //        {
    //            FrameworkElement fe = SourceObj.getDragControl();
    //            if (MoveArea.Children.Contains(fe)) { }
    //            else
    //            {
    //                MoveArea.Children.Add(fe);
    //                ActionActive.Active(fe);
    //                Canvas.SetLeft(fe, e.GetPosition(MoveArea).X);
    //                Canvas.SetTop(fe, e.GetPosition(MoveArea).Y);
    //            }
    //            dragObjControl = fe;
    //            ActionDragDrop.DragdropObject = SourceObj.DragObject();
    //            IsBeginDrag = true;
    //        }
    //    }

    //    void MoveArea_PointerMoved(object sender, PointerRoutedEventArgs e)
    //    {
    //        if ((Enable)&&(dragObjControl!=null))
    //        {
    //            if (MoveArea.Children.Contains(dragObjControl)) { 
    //                MoveArea.Children.Add(dragObjControl);
    //                ActionActive.Active(dragObjControl);
    //                Canvas.SetLeft(dragObjControl, e.GetPosition(MoveArea).X);
    //                Canvas.SetTop(dragObjControl, e.GetPosition(MoveArea).Y);
    //            }

    //            IsBeginDrag = true;
    //        }
    //    }


    //}

    //    public class ActionDrop
    //{
    //    public Panel MoveArea;
    //    public IDropSurport SourceObj;
    //    public Panel SurportDropArea;
    //    public bool Enable;
    //    public ActionDrop(Panel p,IDropSurport o)
    //    {
    //        MoveArea = p;
    //        SourceObj = o;
    //        MoveArea.PointerReleased += new PointerEventHandler(MoveArea_PointerReleased);
    //        MoveArea.PointerEntered += new PointerEventHandler(MoveArea_PointerEntered);
    //        MoveArea.PointerExited += new PointerEventHandler(MoveArea_PointerExited);
    //    }

    //    void MoveArea_PointerExited(object sender, PointerRoutedEventArgs e)
    //    {
    //        SourceObj.DeActionDrop();
    //    }

    //    void MoveArea_PointerEntered(object sender, PointerRoutedEventArgs e)
    //    {
    //        SourceObj.ActionDrop();
    //    }

    //    void MoveArea_PointerReleased(object sender, PointerRoutedEventArgs e)
    //    {
    //        if (ActionDragDrop.DragdropObject != null)
    //        {
    //            SourceObj.DropObject(ActionDragDrop.DragdropObject);
                
    //        }
    //        SourceObj.DeActionDrop();
    //    }
    //}
}
