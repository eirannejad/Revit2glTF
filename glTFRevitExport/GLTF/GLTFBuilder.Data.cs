using System;
using System.Collections.Generic;
using System.Linq;

using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.BufferSegments.BaseTypes;

namespace GLTFRevitExport.GLTF {
    sealed partial class GLTFBuilder {
        const int maxBufferSize = int.MaxValue;

        string _name;
        readonly glTF _gltf = null;

        readonly List<BufferSegment> _bufferSegments = new List<BufferSegment>();
        readonly Queue<glTFMeshPrimitive> _primQueue = new Queue<glTFMeshPrimitive>();
    }
}
