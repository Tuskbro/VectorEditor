using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media.Media3D;

namespace VectorEditor
{
    public class VeLine : BaseShape
    {
        protected Point _startPoint;
        protected Point _endPoint;
        protected Canvas _canvas;
        protected Line _line;
        protected Point _lastPoint;

        public VeLine(Canvas canvas)
        {
            _canvas = canvas;
            _canvas.MouseDown += OnMouseDown;
            _canvas.MouseUp += OnMouseUp;
        }
        

        protected virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(_canvas);
            _line = new Line()
            {
                Stroke = Brushes.Black,
                X1 = _startPoint.X,
                Y1 = _startPoint.Y,
                X2 = _startPoint.X,
                Y2 = _startPoint.Y
            };

            _canvas.Children.Add(_line);

            _canvas.MouseMove += OnMouseMove;
        }

        protected virtual void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _endPoint = e.GetPosition(_canvas);
            _lastPoint = _endPoint;
            _canvas.MouseMove -= OnMouseMove;
        }

        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            _endPoint = e.GetPosition(_canvas);
            _line.X2 = _endPoint.X;
            _line.Y2 = _endPoint.Y;
        }
        public Point GetLastPoint()
        {
            return _lastPoint;
        }

        public override string Serialize()
        {
            return ("<line x1=\"" + _startPoint.X + "\" y1=\"" + _startPoint.Y + "\" x2=\"" + _lastPoint.X + "\" y2=\"" + _lastPoint.Y + "\" stroke=\"" + GetBrushHexColor(StrokeBrush) + "\" stroke-width=\"" + StrokeThickness + "\"/>");
        }
    }
}
