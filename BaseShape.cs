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

namespace VectorEditor
{
    public class BaseShape  : FrameworkElement
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Brush FillBrush { get; set; }
        public Brush StrokeBrush { get; set; }
        public int StrokeThickness { get; set; }

        public BaseShape()
        {
            MouseRightButtonDown += OnStartScaling;
            MouseRightButtonUp += OnStopScaling;
            MouseMove += OnScaling;
        }

        public virtual string Serialize() {
            return "";
        }

        public static string GetBrushHexColor(Brush brush)
        {
            if (brush is SolidColorBrush solidColorBrush)
            {
                var color = solidColorBrush.Color;
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }

            return string.Empty;
        }

        private bool isDragging;
        private Point mouseOffset;

        private Point _previousMousePosition;
        private bool _isScaling;


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                mouseOffset = e.GetPosition(this);
                this.CaptureMouse();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging)
            {
                Point newPos = (Point)(e.GetPosition(this.Parent as UIElement) - mouseOffset);
                Canvas.SetLeft(this, newPos.X);
                Canvas.SetTop(this, newPos.Y);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.LeftButton == MouseButtonState.Released)
            {
                isDragging = false;
                this.ReleaseMouseCapture();
            }
        }

        private void OnStartScaling(object sender, MouseButtonEventArgs e)
        {
            _previousMousePosition = e.GetPosition(this);
            _isScaling = true;
        }

        private void OnStopScaling(object sender, MouseButtonEventArgs e)
        {
            _previousMousePosition = new Point();
            _isScaling = false;
        }

        private void OnScaling(object sender, MouseEventArgs e)
        {
            if (_isScaling && e.RightButton == MouseButtonState.Pressed)
            {
                Point currentMousePosition = e.GetPosition(this);
                double deltaX = currentMousePosition.X - _previousMousePosition.X;
                double deltaY = currentMousePosition.Y - _previousMousePosition.Y;
                
                {
                    Width += deltaX;
                    Height += deltaY;

                    _previousMousePosition = currentMousePosition;
                }
            }
        }

        public void ChangeFillColor(Brush brush)
        {
            
            if (brush != null)
            {
                FillBrush = brush;
                InvalidateVisual();
            }
        }

        public void ChangeStrokeColor(Brush brush)
        {
            
            if (brush != null)
            {
                StrokeBrush = brush;
                InvalidateVisual();
            }
        }

        public void ChangeStrokeThickness(int thickness)
        {
            if (thickness > 0)
            {
                StrokeThickness = thickness;
                InvalidateVisual();
            }
        }

    }
}
