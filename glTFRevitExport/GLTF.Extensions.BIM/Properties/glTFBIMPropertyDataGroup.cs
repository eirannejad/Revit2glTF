using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace GLTFRevitExport.GLTF.Extensions.BIM.Properties {
    [Serializable]
    class glTFBIMPropertyDataGroup {
        [JsonProperty("keys", Order = 1)]
        public List<uint> Keys { get; set; } = new List<uint>();

        [JsonProperty("values", Order = 2)]
        public List<uint> Values { get; set; } = new List<uint>();

        public override bool Equals(object obj) {
            if (obj is glTFBIMPropertyDataGroup other)
                return Keys.Equals(other.Keys) && Values.Equals(other.Values);
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
