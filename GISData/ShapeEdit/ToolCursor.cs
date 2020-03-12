namespace ShapeEdit
{
    using ShapeEdit.Properties;
    using System;
    using System.IO;
    using System.Windows.Forms;

    internal class ToolCursor
    {
        private static Cursor _Add = new Cursor(new MemoryStream(Resources.Editor_Add));
        private static Cursor _Combine = new Cursor(new MemoryStream(Resources.Editor_Combine));
        private static Cursor _Cross = new Cursor(new MemoryStream(Resources.Cross));
        private static Cursor _Cut = new Cursor(new MemoryStream(Resources.Editor_Cut));
        private static Cursor _Delete = new Cursor(new MemoryStream(Resources.Editor_Delete));
        private static Cursor _DeleteVertex = new Cursor(new MemoryStream(Resources.DeleteVertexCursor));
        private static Cursor _Editing = new Cursor(new MemoryStream(Resources.Edit));
        private static Cursor _Erase2 = new Cursor(new MemoryStream(Resources.Editor_Erase2));
        private static Cursor _Erase22 = new Cursor(new MemoryStream(Resources.Editor_Erase22));
        private static Cursor _FeatureQueryed = new Cursor(new MemoryStream(Resources.FeatureQueryed));
        private static Cursor _FeatureQuerying = new Cursor(new MemoryStream(Resources.FeatureQuerying));
        private static Cursor _FeatureSelecting = new Cursor(new MemoryStream(Resources.FeatureSelecting));
        private static Cursor _fix = new Cursor(new MemoryStream(Resources.fix));
        private static Cursor _InsertVertex = new Cursor(new MemoryStream(Resources.InsertVertexCursor));
        private static Cursor _Move = new Cursor(new MemoryStream(Resources.Editor_Move));
        private static Cursor _ParcelQueryed = new Cursor(new MemoryStream(Resources.ParcelQueryed));
        private static Cursor _ParcelQuerying = new Cursor(new MemoryStream(Resources.ParcelQuerying));
        private static Cursor _ParcelSelecting = new Cursor(new MemoryStream(Resources.ParcelSelecting));
        private static Cursor _SnapEx = new Cursor(new MemoryStream(Resources.Editor_SnapEx));
        private static Cursor _Vertex = new Cursor(new MemoryStream(Resources.Editor_Vertex));
        private static Cursor _VertexSelected = new Cursor(new MemoryStream(Resources.VertextSelected));
        private static Cursor _VertexSelected2 = new Cursor(new MemoryStream(Resources.VertextSelected2));
        private static Cursor _VertexSnaped = new Cursor(new MemoryStream(Resources.snap));

        internal static int Add
        {
            get
            {
                return _Add.Handle.ToInt32();
            }
        }

        internal static int Combine
        {
            get
            {
                return _Combine.Handle.ToInt32();
            }
        }

        internal static int Cross
        {
            get
            {
                return _Cross.Handle.ToInt32();
            }
        }

        internal static int Cut
        {
            get
            {
                return _Cut.Handle.ToInt32();
            }
        }

        internal static int Default
        {
            get
            {
                return Cursors.Default.Handle.ToInt32();
            }
        }

        internal static int Delete
        {
            get
            {
                return _Delete.Handle.ToInt32();
            }
        }

        internal static int DeleteVertex
        {
            get
            {
                return _DeleteVertex.Handle.ToInt32();
            }
        }

        internal static int Editing
        {
            get
            {
                return _Editing.Handle.ToInt32();
            }
        }

        internal static int Erase2
        {
            get
            {
                return _Erase2.Handle.ToInt32();
            }
        }

        internal static int Erase22
        {
            get
            {
                return _Erase22.Handle.ToInt32();
            }
        }

        internal static int FeatureQueryed
        {
            get
            {
                return _FeatureQueryed.Handle.ToInt32();
            }
        }

        internal static int FeatureQuerying
        {
            get
            {
                return _FeatureQuerying.Handle.ToInt32();
            }
        }

        internal static int FeatureSelecting
        {
            get
            {
                return _FeatureSelecting.Handle.ToInt32();
            }
        }

        internal static int Fix
        {
            get
            {
                return _fix.Handle.ToInt32();
            }
        }

        internal static int InsertVertex
        {
            get
            {
                return _InsertVertex.Handle.ToInt32();
            }
        }

        internal static int Move
        {
            get
            {
                return _Move.Handle.ToInt32();
            }
        }

        internal static int ParcelQueryed
        {
            get
            {
                return _VertexSelected.Handle.ToInt32();
            }
        }

        internal static int ParcelQuerying
        {
            get
            {
                return _ParcelQuerying.Handle.ToInt32();
            }
        }

        internal static int ParcelSelecting
        {
            get
            {
                return _ParcelSelecting.Handle.ToInt32();
            }
        }

        internal static int SnapEx
        {
            get
            {
                return _SnapEx.Handle.ToInt32();
            }
        }

        internal static int Vertex
        {
            get
            {
                return _Vertex.Handle.ToInt32();
            }
        }

        internal static int VertexSelected
        {
            get
            {
                return _VertexSelected.Handle.ToInt32();
            }
        }

        internal static int VertexSelected2
        {
            get
            {
                return _VertexSelected2.Handle.ToInt32();
            }
        }

        internal static int VertexSnaped
        {
            get
            {
                return _VertexSnaped.Handle.ToInt32();
            }
        }
    }
}

