using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.Properties;
using GLTFRevitExport.GLTF.BufferSegments;
using GLTFRevitExport.GLTF.Package;

namespace GLTFRevitExport.GLTF {
    sealed partial class GLTFBuilder {
        uint AppendNodeToScene(uint idx) {
            if (PeekScene() is glTFScene scene) {
                if (!_gltf.Nodes.IsOpen())
                    scene.Nodes.Add(idx);
                return idx;
            }
            else
                throw new Exception(StringLib.NoParentScene);
        }
    }
}
