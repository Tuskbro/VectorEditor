using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Globalization;

namespace VectorEditor
{
    public class VeEllipse : BaseShape
    {
        

        public VeEllipse() { }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // Создание эллипса
            var ellipse = new EllipseGeometry(new Rect(X, Y, Width, Height));

            // Заливка эллипса
            drawingContext.DrawGeometry(FillBrush, new Pen(StrokeBrush, StrokeThickness), ellipse);
        }

        public override string Serialize()
        {
            return "<ellipse cx=\"" + (X + Width / 2).ToString(CultureInfo.InvariantCulture) + "\" cy=\"" + (Y + Height / 2).ToString(CultureInfo.InvariantCulture) + "\" rx=\"" + (Width / 2).ToString(CultureInfo.InvariantCulture) + "\" ry=\"" + (Height / 2).ToString(CultureInfo.InvariantCulture) + "\" fill=\"" + GetBrushHexColor(FillBrush) + "\" stroke=\"" + GetBrushHexColor(StrokeBrush) + "\" stroke-width=\"" + StrokeThickness.ToString(CultureInfo.InvariantCulture) + "\"/>";
        }
    }
}
