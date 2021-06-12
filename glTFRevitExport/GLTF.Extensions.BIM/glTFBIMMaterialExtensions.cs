using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using GLTFRevitExport.GLTF.Extensions.BIM.BaseTypes;

namespace GLTFRevitExport.GLTF.Extensions.BIM {
    class glTFBIMMaterialExtensions : glTFBIMPropertyExtension {
        public glTFBIMMaterialExtensions(Element e,
                                         bool includeParameters,
                                         glTFBIMPropertyContainer propContainer)
            : base(e, includeParameters, propContainer) { }
    }
}
