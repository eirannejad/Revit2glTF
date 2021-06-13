using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using GLTFRevitExport.Build;
using GLTFRevitExport.GLTF.Extensions.BIM.BaseTypes;

namespace GLTFRevitExport.GLTF.Extensions.BIM {
    class glTFBIMMaterialExtension : glTFBIMPropertyExtension {
        public glTFBIMMaterialExtension(Element e, BuildContext ctx)
            : base(e, ctx) { }
    }
}
