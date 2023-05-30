using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace VectorEditor
{
    public class VeRectangle : BaseShape
    {
        
        public VeRectangle() { }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // Создание прямоугольника
            var rect = new Rect(X, Y, Width, Height);

            // Заливка прямоугольника
            drawingContext.DrawRectangle(FillBrush, new Pen(StrokeBrush, StrokeThickness), rect);
        }

        public override string Serialize()
        {
            
            return ("<rect x=\"" + (X) + "\" y=\"" + (Y) + "\" width=\"" + Width + 
                "\" height=\"" + Height + "\" fill=\"" + GetBrushHexColor(FillBrush) + 
                "\" stroke=\"" + GetBrushHexColor(StrokeBrush) + "\" stroke-width=\"" + StrokeThickness + "\"/>");
        }

    }
}
