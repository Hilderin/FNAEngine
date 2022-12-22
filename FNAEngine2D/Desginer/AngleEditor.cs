using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace FNAEngine2D.Desginer
{
    // This UITypeEditor can be associated with Int32, Double and Single
    // properties to provide a design-mode angle selection interface.
    public class AngleEditor : System.Drawing.Design.UITypeEditor
    {
        public AngleEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Return the value if the value is not of type Int32, Double and Single.
            if (value.GetType() != typeof(double) && value.GetType() != typeof(float) && value.GetType() != typeof(int))
                return value;

            // Uses the IWindowsFormsEditorService to display a
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                // Display an angle selection control and retrieve the value.
                AngleControl angleControl = new AngleControl(Convert.ToDouble(value));
                edSvc.DropDownControl(angleControl);

                // Return the value in the appropraite data format.
                if (value.GetType() == typeof(double))
                    return angleControl.angle;
                else if (value.GetType() == typeof(float))
                    return (float)angleControl.angle;
                else if (value.GetType() == typeof(int))
                    return (int)angleControl.angle;
            }
            return value;
        }

        // Draws a representation of the property's value.
        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            int normalX = (e.Bounds.Width / 2);
            int normalY = (e.Bounds.Height / 2);

            // Fill background and ellipse and center point.
            e.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.FillEllipse(new SolidBrush(Color.White), e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 3, e.Bounds.Height - 3);
            e.Graphics.FillEllipse(new SolidBrush(Color.SlateGray), normalX + e.Bounds.X - 1, normalY + e.Bounds.Y - 1, 3, 3);

            // Draw line along the current angle.
            double radians = (Convert.ToDouble(e.Value) * Math.PI) / (double)180;
            e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Red), 1), normalX + e.Bounds.X, normalY + e.Bounds.Y,
                e.Bounds.X + (normalX + (int)((double)normalX * Math.Cos(radians))),
                e.Bounds.Y + (normalY + (int)((double)normalY * Math.Sin(radians))));
        }

        // Indicates whether the UITypeEditor supports painting a
        // representation of a property's value.
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    }

}
