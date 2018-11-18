using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using System.Text;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SilverlightEduProcessManagerUI.Note
{
    public partial class LynxNotepad : UserControl
    {
        public LynxNotepad()
        {
            InitializeComponent();
            am = new ActionMove(this, Frame);
            ar = new ActionResize(NoteFrame, NoteFrame,Resized);
        }
        ActionMove am;
        ActionResize ar;
        void Resized()
        {
            gridTitle.Width = Width;
            note.Width = Width - 20;
            note.Height = Height - 35;
            Frame.Width = Width;
            Frame.Height = Height;
        }
        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
                Panel p = this.Parent as Panel;
                LynxNotepad np = new LynxNotepad();
                p.Children.Add(np);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var xen = SilverlightLFC.common.Environment.getEnvironment();
            xen.SaveFileString(note.Text);
        }

        private void buttonBig_Click(object sender, RoutedEventArgs e)
        {
            
            note.FontSize = note.FontSize + 2;

        }

        private void buttonSmall_Click(object sender, RoutedEventArgs e)
        {
            note.FontSize = note.FontSize - 2;
        }
    }
}
