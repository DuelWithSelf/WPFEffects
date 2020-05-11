using _3DTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WPFEffects.Core.CommonFrms;

namespace WPFEffects.Modules.Carousel
{
    public class InteractivePanel3D : InteractiveVisual3D, IDisposable
    {
        public double Degree = 0;

        public string FileSrc = "";
        private int Index = 0;

        private bool _IsVisible = false;

        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                _IsVisible = value;
                if (value && Visual != null)
                {
                    AnimImage oPersonItem = this.Visual as AnimImage;
                    if (oPersonItem != null)
                        oPersonItem.LoadUiImmediate();
                }
            }
        }

        public InteractivePanel3D(string sFile, int nIndex)
        {
            this.FileSrc = sFile;
            this.Index = nIndex;

            Geometry = CreateModel();
            Transform = this.CreateTransform();
            Visual = this.CreateVisual();
            IsBackVisible = true;
        }

        private Transform3D CreateTransform()
        {
            var trans3DGroup = new Transform3DGroup();
            var scale3D = new ScaleTransform3D();
            trans3DGroup.Children.Add(scale3D);
            return trans3DGroup;
        }

        internal const double y = 2.38;
        internal const double x = 3.8;

        private MeshGeometry3D CreateModel()
        {
            var geometry3D = new MeshGeometry3D();
            geometry3D.Positions.Add(new Point3D(-x, y, 0));
            geometry3D.Positions.Add(new Point3D(-x, -y, 0));
            geometry3D.Positions.Add(new Point3D(x, -y, 0));
            geometry3D.Positions.Add(new Point3D(x, y, 0));

            var iCol = new[] { 0, 1, 2, 0, 2, 3 };
            var pCol = new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 0) };
            var vCol = new[]
            {new Vector3D(0, 1, 0), new Vector3D(0, 1, 0), new Vector3D(0, 1, 0), new Vector3D(0, 1, 0)};

            geometry3D.TriangleIndices = new Int32Collection(iCol);
            geometry3D.TextureCoordinates = new PointCollection(pCol);
            geometry3D.Normals = new Vector3DCollection(vCol);

            return geometry3D;
        }

        private Visual CreateVisual()
        {
            if (this.FileSrc == "")
                return null;
            AnimImage oItem = new AnimImage(this.FileSrc);
            return oItem;
        }

        public void Dispose()
        {
            Geometry = null;
            AnimImage oPersonItem = this.Visual as AnimImage;
            if (oPersonItem != null)
                oPersonItem.Dispose();
            Visual = null;
        }
    }
}
