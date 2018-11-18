using System;
using System.Net;
using System.Windows;



using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;




namespace SilverlightLynxControls
{
    public class UILogic
    {
        public static Rect getPathArea(Path p)
        {
            PathGeometry pg = p.Data as PathGeometry;
            PathFigure pf = pg.Figures[0];
            double minx, miny, maxx, maxy;
            minx=maxx = pf.StartPoint.X;
            miny=maxy = pf.StartPoint.Y;

            foreach (LineSegment ls in pf.Segments)
            {
                if (ls.Point.X < minx)
                {
                    minx = ls.Point.X;
                }
                if (ls.Point.X > maxx)
                {
                    maxx = ls.Point.X;
                }
                if (ls.Point.Y < miny)
                {
                    miny = ls.Point.Y;
                }
                if (ls.Point.Y > maxy)
                {
                    maxy = ls.Point.Y;
                }
            }

            return new Rect(minx, miny, maxx - minx, maxy - miny);
        }

        public static Rect getPathArea(PathGeometry p)
        {

            PathFigure pf = p.Figures[0];
            double minx, miny, maxx, maxy;
            minx = maxx = pf.StartPoint.X;
            miny = maxy = pf.StartPoint.Y;

            foreach (LineSegment ls in pf.Segments)
            {
                if (ls.Point.X < minx)
                {
                    minx = ls.Point.X;
                }
                if (ls.Point.X > maxx)
                {
                    maxx = ls.Point.X;
                }
                if (ls.Point.Y < miny)
                {
                    miny = ls.Point.Y;
                }
                if (ls.Point.Y > maxy)
                {
                    maxy = ls.Point.Y;
                }
            }

            return new Rect(minx, miny, maxx - minx, maxy - miny);
        }

    }
}
