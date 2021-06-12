using System;

using Autodesk.Revit.DB;

using GLTFRevitExport.Extensions;

namespace GLTFRevitExport.GLTF.Extensions.BIM {
    [Serializable]
    class glTFBIMVector {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public glTFBIMVector(XYZ pt) {
            float[] vector = pt.ToGLTF();
            X = vector[0];
            Y = vector[1];
            Z = vector[2];
        }

        public glTFBIMVector(float x, float y, float z) {
            X = x; Y = y; Z = z;
        }

        public glTFBIMVector(glTFBIMVector vector) {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
        }

        public void ContractTo(glTFBIMVector other) {
            X = other.X < X ? other.X : X;
            Y = other.Y < Y ? other.Y : Y;
            Z = other.Z < Z ? other.Z : Z;
        }

        public void ExpandTo(glTFBIMVector other) {
            X = other.X > X ? other.X : X;
            Y = other.Y > Y ? other.Y : Y;
            Z = other.Z > Z ? other.Z : Z;
        }
    }
}
