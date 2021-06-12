using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using Autodesk.Revit.DB;

using GLTFRevitExport.Extensions;
using GLTFRevitExport.GLTF.Extensions.BIM.BaseTypes;

namespace GLTFRevitExport.GLTF.Extensions.BIM {
    class glTFBIMNodeExtension : glTFBIMPropertyExtension {
        public glTFBIMNodeExtension(Element e,
                                    bool includeParameters,
                                    glTFBIMPropertyContainer propContainer)
            : base(e, includeParameters, propContainer)
        {
            // set level
            if (e.Document.GetElement(e.LevelId) is Level level)
                Level = level.GetId();
        }

        [JsonProperty("level", Order = 21)]
        public string Level { get; set; }

        [JsonProperty("bounds", Order = 23)]
        public glTFBIMBounds Bounds { get; set; }
    }
}
