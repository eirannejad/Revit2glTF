using System;
using System.Linq;

using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.BufferSegments.BaseTypes;

namespace GLTFRevitExport.GLTF.BufferSegments {
    class BufferScalar4Segment : BufferSegment<uint> {
        public override glTFAccessorType Type => glTFAccessorType.SCALAR;
        public override glTFAccessorComponentType DataType => glTFAccessorComponentType.UNSIGNED_INT;
        public override glTFBufferViewTargets Target => glTFBufferViewTargets.ELEMENT_ARRAY_BUFFER;

        public BufferScalar4Segment(uint[] scalars) {
            Data = scalars;
            _min = new uint[] { Data.Min() };
            _max = new uint[] { Data.Max() };
        }
    }
}