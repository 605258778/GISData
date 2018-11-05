using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using System.Windows.Forms;
using System;

namespace GISData.File
{
    class CreateNewDocument : BaseCommand
    {
         private IHookHelper m_hookHelper = null;

        //constructor
        public CreateNewDocument()
        {
            //update the base properties
            base.m_category = ".NET Application";
            base.m_caption = "NewDocument";
            base.m_message = "Create a new map";
            base.m_toolTip = "Create a new map";
            base.m_name = "DotNetTemplate_NewDocumentCommand";
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            IMapControl3 mapControl = null;

            //get the MapControl from the hook in case the container is a ToolbarControl
            if (m_hookHelper.Hook is IToolbarControl)
            {
                mapControl = (IMapControl3)((IToolbarControl)m_hookHelper.Hook).Buddy;
            }
            //In case the container is MapControl
            else if (m_hookHelper.Hook is IMapControl3)
            {
                mapControl = (IMapControl3)m_hookHelper.Hook;
            }
            else
            {
                MessageBox.Show("Active control must be MapControl!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //check to see if there is an active edit session and whether edits have been made
            DialogResult result;
            IEngineEditor engineEditor = new EngineEditorClass();

            if ((engineEditor.EditState == esriEngineEditState.esriEngineStateEditing) && (engineEditor.HasEdits() == true))
            {
                result = MessageBox.Show("Would you like to save your edits", "Save Edits", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {

                    case DialogResult.Cancel:
                        return;

                    case DialogResult.No:
                        engineEditor.StopEditing(false);
                        break;

                    case DialogResult.Yes:
                        engineEditor.StopEditing(true);
                        break;

                }
            }

            //allow the user to save the current document
            DialogResult res = MessageBox.Show("Would you like to save the current document?", "AoView", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                //launch the save command
                ICommand command = new ControlsSaveAsDocCommandClass();
                command.OnCreate(m_hookHelper.Hook);
                command.OnClick();
            }

            //create a new Map
            IMap map = new MapClass();
            map.Name = "Map";

            //assign the new map to the MapControl
            mapControl.DocumentFilename = string.Empty;
            mapControl.Map = map;
        }

        #endregion

        private ESRI.ArcGIS.Controls.IMapControl4 m_mapControl = null; 
        private ESRI.ArcGIS.Controls.IPageLayoutControl3 m_pageLayoutControl = null;
        private string m_mapDocumentName = "";
        private void Save_Click(object sender, EventArgs e)
        {
            //execute Save Document command
            if (m_mapControl.CheckMxFile(m_mapDocumentName))
            {
                //create a new instance of a MapDocument
                IMapDocument mapDoc = new MapDocumentClass();
                mapDoc.Open(m_mapDocumentName, string.Empty);

                //Make sure that the MapDocument is not readonly
                if (mapDoc.get_IsReadOnly(m_mapDocumentName))
                {
                    MessageBox.Show("Map document is read only!");
                    mapDoc.Close();
                    return;
                }

            }

        }

        private void SavaAs_Click(object sender, EventArgs e)
        {
            //execute SaveAs Document command
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();

        }

        private void Open_Click(object sender, EventArgs e)
        {
            //execute Open Document command
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }

        private void NewDocTool_Click(object sender, EventArgs e)
        {
            //execute New Document command
            ICommand command = new CreateNewDocument();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();

        }
    }
}
