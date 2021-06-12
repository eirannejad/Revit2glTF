using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using Autodesk.Revit.DB;

namespace GLTFRevitExport.GLTF.Extensions.BIM {
    [Serializable]
    class glTFBIMBounds : ISerializable {
        public glTFBIMBounds(BoundingBoxXYZ bbox) {
            Min = new glTFBIMVector(bbox.Min);
            Max = new glTFBIMVector(bbox.Max);
        }

        public glTFBIMBounds(XYZ min, XYZ max) {
            Min = new glTFBIMVector(min);
            Max = new glTFBIMVector(max);
        }

        public glTFBIMBounds(float minx, float miny, float minz,
                             float maxx, float maxy, float maxz) {
            Min = new glTFBIMVector(minx, miny, minz);
            Max = new glTFBIMVector(maxx, maxy, maxz);
        }

        public glTFBIMBounds(glTFBIMBounds bounds) {
            Min = new glTFBIMVector(bounds.Min);
            Max = new glTFBIMVector(bounds.Max);
            if (bounds.LinkHostBounds != null)
                LinkHostBounds = new glTFBIMBounds(bounds.LinkHostBounds);
        }

        public glTFBIMBounds(SerializationInfo info, StreamingContext context) {
            var min = (float[])info.GetValue("min", typeof(float[]));
            Min = new glTFBIMVector(min[0], min[1], min[2]);
            var max = (float[])info.GetValue("max", typeof(float[]));
            Max = new glTFBIMVector(max[0], max[1], max[2]);
        }

        [JsonProperty("min")]
        public glTFBIMVector Min { get; set; }

        [JsonProperty("max")]
        public glTFBIMVector Max { get; set; }

        public glTFBIMBounds LinkHostBounds { get; set; } = null;

        public void Union(glTFBIMBounds other) {
            Min.ContractTo(other.Min);
            Max.ExpandTo(other.Max);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("min", new double[] { Min.X, Min.Y, Min.Z });
            info.AddValue("max", new double[] { Max.X, Max.Y, Max.Z });
        }
    }
}
