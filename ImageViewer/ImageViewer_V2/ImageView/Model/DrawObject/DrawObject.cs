using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageView.Model.DrawObject
{
    internal class DrawObject
    {
        public DrawObject()
        {
            drawEllipses = new Queue<DrawEllipse>();
            drawLines = new Queue<DrawLine>();
            drawRects = new Queue<DrawRect>(); 
        }
        public Queue<DrawEllipse> drawEllipses;
        public Queue<DrawLine> drawLines;
        public Queue<DrawRect> drawRects;

        public void DeleteAllDrawObject() 
        {
            if(drawEllipses.Count != 0)
                drawEllipses.Clear();

            if (drawLines.Count != 0)
                drawLines.Clear();

            if (drawRects.Count != 0)
                drawRects.Clear();
        }
    }
}
