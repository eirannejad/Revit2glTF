using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Autodesk.Revit.DB;

using GLTFRevitExport.Build;
using GLTFRevitExport.GLTF.Extensions.BIM.BaseTypes;

namespace GLTFRevitExport.GLTF.Extensions.BIM {
    class glTFBIMSceneExtension : glTFBIMPropertyExtension {
        public glTFBIMSceneExtension() : base() { }
        public glTFBIMSceneExtension(Element e, BuildContext ctx)
            : base(e, ctx) { }

        [JsonProperty("bounds", Order = 23)]
        public glTFBIMBounds Bounds { get; set; }
    }
}
