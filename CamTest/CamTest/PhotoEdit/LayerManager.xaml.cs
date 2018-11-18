using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoEdit
{
    public partial class LayerManager : UserControl
    {
        public LayerManager()
        {
            InitializeComponent();

            acm = new ActionMove(this, title);
        }
        ActionMove acm;
        public void setTarget(PhotoEditCanvas p)
        {
            pc = p;
            stackLayer.Children.Clear();
            ReadLayers();
        }
        LayerIcon _SelectLayerIcon;
        LayerIcon SelectLayerIcon
        {
            get
            {
                return _SelectLayerIcon;
            }
            set
            {
                _SelectLayerIcon = value;
                pc.SelectLayer = _SelectLayerIcon.getLayer();
            }
        }
        PhotoEditCanvas pc;

        //public void SelectLayer(LayerIcon li)
        //{
        //    pc.SelectLayer = li.getLayer();
            
        //}

        public void CutLayer()
        {

            pc.PhotoLayers.Children.Remove(SelectLayerIcon.getLayer());
            stackLayer.Children.Remove(SelectLayerIcon);
            if (stackLayer.Children.Count != 0)
            {
                SelectLayerIcon = stackLayer.Children[0] as LayerIcon;
                setLayersZIndex();
            }
            else
            {
                pc.SelectLayer = null;
            }
        }

        public void DeleteLayer()
        {
            
            pc.PhotoLayers.Children.Remove(SelectLayerIcon.getLayer());
            stackLayer.Children.Remove(SelectLayerIcon);
            SelectLayerIcon.selectLayer -= new LayerEventHandler(SelectLayerIcon_selectLayer);
            SelectLayerIcon.PointerPressed -= new PointerEventHandler(LayerIcon_PointerPressed);
            SelectLayerIcon.PointerReleased -= new PointerEventHandler(LayerIcon_PointerReleased);
            if (stackLayer.Children.Count != 0)
            {
                SelectLayerIcon = stackLayer.Children[0] as LayerIcon;
                setLayersZIndex();
            }
            else
            {
                pc.SelectLayer = null;
            }
        }

        public void ClearSelect()
        {
            for (int i = 0; i < stackLayer.Children.Count; i++)
            {
                LayerIcon li = stackLayer.Children[i] as LayerIcon;
                li.UnSelect();
            }

        }

        public void setLayersZIndex()
        {
            for (int i = 0; i < stackLayer.Children.Count; i++)
            {
                LayerIcon li = stackLayer.Children[i] as LayerIcon;
                PhotoLayer pl = li.getLayer();
                Canvas.SetZIndex(pl, stackLayer.Children.Count - stackLayer.Children.IndexOf(li));
            }
        }

        public void ReadLayers()
        {
            Canvas ls = pc.getLayers();
            stackLayer.Children.Clear();
            for (int i = 0; i < ls.Children.Count; i++)
            {
                PhotoLayer pl = ls.Children[i] as PhotoLayer;
                stackLayer.Children.Add(pl.li);
                pl.li.ReadInfor();
                LayerIcon li = stackLayer.Children[i] as LayerIcon;
                li.selectLayer += new LayerEventHandler(SelectLayerIcon_selectLayer);
                li.ReadInfor();
            }
        }

        public void AddLayer(PhotoLayer o)//层永远加在最上面
        {
            setLayersZIndex();
            Canvas.SetZIndex(o, pc.getLayers().Children.Count);
            SelectLayerIcon = new LayerIcon(o);
            SelectLayerIcon.selectLayer += new LayerEventHandler(SelectLayerIcon_selectLayer);
            o.Selected();
            //li.Init(o);
            SelectLayerIcon.PointerPressed+=new PointerEventHandler(LayerIcon_PointerPressed);
            SelectLayerIcon.PointerReleased += new PointerEventHandler(LayerIcon_PointerReleased);
            stackLayer.Children.Add(SelectLayerIcon);
            pc.PhotoLayers.Children.Add(SelectLayerIcon.getLayer());
        }

        void SelectLayerIcon_selectLayer(object sender, LynxPhotoLayerEventArgs e)
        {
            if (e.IsSelected)
            {
                ClearSelect();
                e.currentIcon.Select();
                SelectLayerIcon = e.currentIcon;
            }
            else
            {
                if (e.currentIcon != SelectLayerIcon)
                {
                    if (stackLayer.Children.Contains(SelectLayerIcon)) { stackLayer.Children.Remove(SelectLayerIcon); }
                    stackLayer.Children.Insert(stackLayer.Children.IndexOf(e.currentIcon), SelectLayerIcon);
                    setLayersZIndex();
                }
            }
        }

        private void LayerIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ClearSelect();
            SelectLayerIcon = sender as LayerIcon;
            SelectLayerIcon.Select();
        }

        private void LayerIcon_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            LayerIcon tli = sender as LayerIcon;
            if (tli != SelectLayerIcon)
            {
                if (stackLayer.Children.Contains(SelectLayerIcon)) { stackLayer.Children.Remove(SelectLayerIcon); }
                stackLayer.Children.Insert(stackLayer.Children.IndexOf(tli), SelectLayerIcon);
                setLayersZIndex();
            }
        }
        private void buttonNewLayer_Click(object sender, RoutedEventArgs e)
        {
            PhotoLayer pl = new PhotoLayer();
            AddLayer(pl);
        }

        private void buttonDeleteLayer_Click(object sender, RoutedEventArgs e)
        {
            DeleteLayer();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }

        private void buttonRefreshLayer_Click(object sender, RoutedEventArgs e)
        {
            ReadLayers();
        }

        private void stackLayer_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender as LayerIcon != null)
            {
                SelectLayerIcon = sender as LayerIcon;
            }
        }

        private void stackLayer_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            LayerIcon tli = sender as LayerIcon;
            if (tli == null) { return; }
            if (tli != SelectLayerIcon)
            {
                if (stackLayer.Children.Contains(SelectLayerIcon)) { stackLayer.Children.Remove(SelectLayerIcon); }
                stackLayer.Children.Insert(stackLayer.Children.IndexOf(tli), SelectLayerIcon);
                setLayersZIndex();
            }
        }
    }
}
