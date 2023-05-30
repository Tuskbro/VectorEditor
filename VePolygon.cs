using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VectorEditor
{
    public class VePolygon : BaseShape
    {

        protected Canvas _canvas;
        MouseButtonEventHandler _AddOrSelectItems;
        private readonly List<Point> _points = new List<Point>();
        public VePolygon(Canvas canvas, MouseButtonEventHandler AddOrSelectItems) 
        {
            _canvas= canvas;
            _canvas.MouseLeftButtonDown += OnMouseDown;
            _AddOrSelectItems = AddOrSelectItems;
            _canvas.MouseLeftButtonDown -= _AddOrSelectItems;
        }
        public void AddPoint(Point point)
        {
            _points.Add(point);
            InvalidateVisual();
        }

        public void RemoveLastPoint()
        {
            if (_points.Count > 0)
            {
                _points.RemoveAt(_points.Count - 1);
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (_points.Count > 1)
            {
                var streamGeometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

                using (var context = streamGeometry.Open())
                {
                    context.BeginFigure(_points[0], true, true);

                    for (var i = 1; i < _points.Count; i++)
                    {
                        context.LineTo(_points[i], true, false);
                    }
                }

                drawingContext.DrawGeometry(FillBrush, new Pen(StrokeBrush, StrokeThickness), streamGeometry);
            }
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                AddPoint(e.GetPosition(_canvas));
            }
            else if (e.ClickCount == 2)
            {
                _canvas.MouseLeftButtonDown -= OnMouseDown;
                _canvas.MouseLeftButtonDown += _AddOrSelectItems;
            }
        }
        public override string Serialize()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("<polygon points=\"");

            foreach (var point in _points)
            {
                sb.AppendFormat("{0},{1} ", point.X, point.Y);
            }

            sb.AppendFormat("\" fill=\"{0}\" stroke=\"{1}\" stroke-width=\"{2}\" />",
                            GetBrushHexColor(FillBrush), GetBrushHexColor(StrokeBrush), StrokeThickness);

            return sb.ToString();
        }

    }
}
