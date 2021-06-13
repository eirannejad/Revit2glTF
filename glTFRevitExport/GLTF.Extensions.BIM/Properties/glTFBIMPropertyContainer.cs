using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using GLTFRevitExport.GLTF.Extensions.BIM.BaseTypes;

namespace GLTFRevitExport.GLTF.Extensions.BIM.Properties {
    [Serializable]
    class glTFBIMPropertyContainer : glTFBIMContainer {
        string _uri;
        glTFBIMPropertyData _propData = new glTFBIMPropertyData();

        public glTFBIMPropertyContainer(string uri) {
            _uri = uri;
        }


        [JsonProperty("$type")]
        public override string Type => "properties";

        [JsonProperty("uri")]
        public override string Uri => _uri;

        public bool HasPropertyData => _propData.Records.Count > 0;

        public void Record(string id, Dictionary<string, object> props) => _propData.Record(id, props);

        public string Pack() {
            return JsonConvert.SerializeObject(
                    _propData,
                    new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    }
                );
        }
    }
}
