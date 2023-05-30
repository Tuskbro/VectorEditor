using System.Windows.Forms;
using System.Windows.Media;


namespace VectorEditor
{
    public class ColorPicker
    {
        public static Brush ShowDialog()
        {
            var dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var color = dialog.Color;
                var brush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                return brush;
            }

            return null;
        }
    }
}
