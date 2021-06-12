using System;

using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.Properties;

namespace GLTFRevitExport.GLTF.Extensions.BIM.BaseTypes {
    [Serializable]
    abstract class glTFBIMExtension: glTFExtension {
        public glTFBIMExtension() { }

        public override string Name => StringLib.GLTFExtensionName;
    }
}
