using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using FNAEngine2D.GameObjects;
using FNAEngine2D.TileSets;

namespace FNAEngine2D.Desginer
{
    // This UITypeEditor can be associated with Int32, Double and Single
    // properties to provide a design-mode angle selection interface.
    public class TileSetUITypeEditor : System.Drawing.Design.UITypeEditor
    {
        private EditModeService _editModeService;

        public TileSetUITypeEditor()
        {
            _editModeService = ServiceProvider.GetService<EditModeService>();

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

            if(_editModeService.SelectedGameObject == null || !(_editModeService.SelectedGameObject is TileSetGrid))
                return value;

            TileSetGrid tileSetRender = (TileSetGrid)_editModeService.SelectedGameObject;

            if (tileSetRender.TileSet == null)
                tileSetRender.TileSet = new TileSet();

            _editModeService.ShowTileSetEditor(tileSetRender, true);


            //No changes...
            return value;
        }

    }

}
