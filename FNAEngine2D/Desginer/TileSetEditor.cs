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
    public class TileSetEditor : System.Drawing.Design.UITypeEditor
    {
        public TileSetEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            //Must be of type TileSet
            if (value != null && value.GetType() != typeof(TileSet))
                return value;

            if (value == null)
                value = new TileSet();

            if (TileSetEditorForm.Current != null && TileSetEditorForm.Current.TileSet == (TileSet)value)
            {
                //Reopening the save...
                if (TileSetEditorForm.Current.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                    TileSetEditorForm.Current.WindowState = System.Windows.Forms.FormWindowState.Normal;

                TileSetEditorForm.Current.Show();
                TileSetEditorForm.Current.Focus();
            }
            else
            {
                //New form...
                TileSetEditorForm newForm = new TileSetEditorForm((TileSet)value);
                newForm.Show();
            }
                

            //No changes...
            return value;
        }

    }

}
