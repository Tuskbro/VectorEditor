using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace VectorEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum Shape
        {
            Line,
            Path,
            Rectangle,
            Ellipse,
            Polygon
            
        }
        BaseShape SelectedShape;
        Shape CurrentShapeTools;
        private bool isDrawing = false;
        Brush DefFillBrush = Brushes.White;
        Brush DefStrokeBrush = Brushes.Black;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void AddOrSelectItems(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing) return;
            if (e.OriginalSource is BaseShape)
            {
                SelectedShape = (BaseShape)e.OriginalSource;

                //myCanvas.Children.Remove(SelectedShape);
                WidthTextBox.Text= SelectedShape.ActualWidth.ToString();
                HeightTextBox.Text = SelectedShape.ActualHeight.ToString();
                Fill_ColorDialog.Background = SelectedShape.FillBrush;
                StrokeThickness.Text = SelectedShape.StrokeThickness.ToString();
                StrokeThickness_Slider.Value = SelectedShape.StrokeThickness;
                Stroke_ColorDialog.Background = SelectedShape.StrokeBrush;
                Disply_Selected.Text = SelectedShape.ToString();

            }
            else
            {
                switch (CurrentShapeTools)
                {
                    case Shape.Ellipse:
                        {
                            var ellipse = new VeEllipse
                            {
                                FillBrush = Fill_ColorDialog.Background == DefFillBrush ? Fill_ColorDialog.Background : DefFillBrush,
                                StrokeBrush = Stroke_ColorDialog.Background == DefStrokeBrush ? Stroke_ColorDialog.Background : DefStrokeBrush,
                                StrokeThickness = (int)StrokeThickness_Slider.Value,
                                Width = WidthTextBox.Text.Length > 0 ? Convert.ToDouble(WidthTextBox.Text) : 50,
                                Height = HeightTextBox.Text.Length > 0 ? Convert.ToDouble(HeightTextBox.Text) : 50,
                                X = Mouse.GetPosition(myCanvas).X,
                                Y = Mouse.GetPosition(myCanvas).Y,
                            };

                            myCanvas.Children.Add(ellipse);
                            break;
                        }
                    case Shape.Rectangle:
                        {
                            var myRectangle = new VeRectangle
                            {
                                FillBrush = Fill_ColorDialog.Background == DefFillBrush ? Fill_ColorDialog.Background : DefFillBrush,
                                StrokeBrush = Stroke_ColorDialog.Background == DefStrokeBrush ? Stroke_ColorDialog.Background : DefStrokeBrush,
                                StrokeThickness = (int)StrokeThickness_Slider.Value,
                                Width = WidthTextBox.Text.Length>0 ? Convert.ToDouble(WidthTextBox.Text): 50,
                                Height = HeightTextBox.Text.Length > 0 ? Convert.ToDouble(HeightTextBox.Text) : 50,
                                X = Mouse.GetPosition(myCanvas).X,
                                Y = Mouse.GetPosition(myCanvas).Y,
                            };

                            myCanvas.Children.Add(myRectangle);

                            break;
                        }
                    case Shape.Line:
                        {
                            
                            var line = new VeLine(myCanvas);
                            
                            break;
                        }
                    case Shape.Polygon:
                        {
                            VePolygon polygon = new VePolygon(myCanvas, AddOrSelectItems)
                            {
                                FillBrush = Brushes.Yellow,
                                StrokeBrush = Brushes.Black,
                                StrokeThickness = 2,
                            };
                            myCanvas.Children.Add(polygon);
                            

                            break;
                        }
                    case Shape.Path:
                        {
                            
                            var Path = new VePath(myCanvas,AddOrSelectItems);
                            
                            break;
                        }
                    default: 
                        {
                            break;
                        }
                }

                
            }

        }



        private void svg_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "SVG files (*.svg)|*.svg";
            saveFileDialog.Title = "Save SVG File";
            saveFileDialog.FileName = "canvas.svg";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                // Создание файла SVG
                CreateSvgFile(filePath);

                MessageBox.Show("SVG file created successfully.");
            }
        }
        private void CreateSvgFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {

                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>");
                writer.WriteLine("<svg width=\"" + myCanvas.ActualWidth + "\" height=\"" + myCanvas.ActualHeight + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">");



                foreach (var shape in myCanvas.Children)
                {
                    if (shape is VectorEditor.BaseShape baseShape)
                    {

                        var element = shape as VectorEditor.BaseShape;
                        writer.WriteLine(element.Serialize());

                    }
                    else if (shape is Line line)
                    {
                        var element = shape as Line;
                        writer.WriteLine(SerializeLine(element));
                    }
                }


                writer.WriteLine("</svg>");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var line = new VeLine(myCanvas);
            
        }

        private void Path_Click(object sender, RoutedEventArgs e)
        {
            //var Path = new VePath(myCanvas);
        }

        private void Tools_Selected(object sender, SelectionChangedEventArgs e)
        {
            CurrentShapeTools = (Shape)Tools.SelectedIndex;
            isDrawing = false;
        }

        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }

        private void WidthTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((SelectedShape != null) && (WidthTextBox.Text.Length > 0))
            {
                SelectedShape.Width = Convert.ToDouble(WidthTextBox.Text);
            }
        }

        private void WidthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((SelectedShape != null)&&(WidthTextBox.Text.Length>0))
            {
                SelectedShape.Width = Convert.ToDouble(WidthTextBox.Text);
            }
        }

        private void HeightTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((SelectedShape != null) && (HeightTextBox.Text.Length > 0))
            {
                SelectedShape.Height = Convert.ToDouble(HeightTextBox.Text);
            }
        }

        private void HeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((SelectedShape != null) && (HeightTextBox.Text.Length > 0))
            {
                SelectedShape.Height = Convert.ToDouble(HeightTextBox.Text);
            }
        }

        private void Fill_ColorDialog_Click(object sender, RoutedEventArgs e)
        {
            Brush TempBrush = ColorPicker.ShowDialog();
            if (SelectedShape != null && SelectedShape.FillBrush != TempBrush)
            {
                SelectedShape.ChangeFillColor(TempBrush);
                Fill_ColorDialog.Background = TempBrush;
            }
        }

        private void Stroke_ColorDialog_Click(object sender, RoutedEventArgs e)
        {
            Brush TempBrush = ColorPicker.ShowDialog();
            if (SelectedShape != null && SelectedShape.StrokeBrush != TempBrush)
            {
                SelectedShape.ChangeStrokeColor(TempBrush);
                Stroke_ColorDialog.Background = TempBrush;
                
            }
        }

        private void StrokeThickness_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int thickness = (int)StrokeThickness_Slider.Value;
            if (SelectedShape != null && SelectedShape.StrokeThickness != thickness)
            {
                SelectedShape.ChangeStrokeThickness(thickness);
            }
        }

        private void StrokeThickness_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void StrokeThickness_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public  string SerializeLine(Line line)
        {
            return ("<line x1=\"" + line.X1 + "\" y1=\"" + line.Y1 + "\" x2=\"" + line.X2 + "\" y2=\"" + line.Y2 + "\" stroke=\"" + BaseShape.GetBrushHexColor(line.Stroke) + "\" stroke-width=\"" + line.StrokeThickness + "\"/>");
        }



        private void NewCanvas_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
