using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VectorEditor
{
    public class VePath : VeLine
    {
        private bool _isDrawing;
        MouseButtonEventHandler _AddOrSelectItems;
        //private List<Point> _points = new List<Point>();
        public VePath(Canvas canvas, MouseButtonEventHandler AddOrSelectItems) : base(canvas)
        {
            _AddOrSelectItems= AddOrSelectItems;
            _canvas.MouseLeftButtonDown -= AddOrSelectItems;
        }

        protected override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _isDrawing = false;
                _canvas.MouseMove -= OnMouseMove;
                _canvas.MouseDown -= OnMouseDown;
                
                _canvas.MouseLeftButtonDown += _AddOrSelectItems;
                return;
            }

            base.OnMouseDown(sender, e);
            _isDrawing = true;
            //_points.Add(e.GetPosition(_canvas));
        }

        protected override void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _isDrawing = false;
                _canvas.MouseMove -= OnMouseMove;
                
                //_canvas.MouseLeftButtonDown += _AddOrSelectItems;
                return;
            }

            base.OnMouseUp(sender, e);

            if (_isDrawing)
            {
                _startPoint = _lastPoint;
                _canvas.MouseMove += OnMouseMove;
            //    _points.Add(e.GetPosition(_canvas));
            }
        }
        

        
    }
}
