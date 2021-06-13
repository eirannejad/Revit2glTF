using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Autodesk.Revit.DB;

using GLTFRevitExport.Build;
using GLTFRevitExport.Extensions;
using GLTFRevitExport.GLTF.Extensions.BIM.BaseTypes;
using GLTFRevitExport.GLTF.Extensions.BIM.Properties;

namespace GLTFRevitExport.GLTF.Extensions.BIM {
    [Serializable]
    class glTFBIMAssetExtension : glTFBIMExtension {
        public glTFBIMAssetExtension(Document d, BuildContext ctx) : base() {
            App = GetAppName(d);
            Id = GetDocumentId(d).ToString();
            Title = d.Title;
            Source = d.PathName;
            if (ctx.Configs.ExportParameters) {
                if (ctx.PropertyContainer is glTFBIMPropertyContainer) {
                    // record properties
                    ctx.PropertyContainer.Record(Id, GetProjectInfo(d));
                    // ensure property sources list is initialized
                    if (Containers is null)
                        Containers = new List<glTFBIMPropertyContainer>();
                    // add the new property source
                    if (!Containers.Contains(ctx.PropertyContainer))
                        Containers.Add(ctx.PropertyContainer);
                }
                else
                    // embed properties
                    Properties = GetProjectInfo(d);
            }
        }

        static string GetAppName(Document doc) {
            var app = doc.Application;
            var hostName = app.VersionName;
            hostName = hostName.Replace(app.VersionNumber, app.SubVersionNumber);
            return $"{hostName} {app.VersionBuild}";
        }

        static Guid GetDocumentId(Document doc) {
            if (doc?.IsValidObject != true)
                return Guid.Empty;
            return ExportUtils.GetGBXMLDocumentId(doc);
        }

        static Dictionary<string, object> GetProjectInfo(Document doc) {
            var docProps = new Dictionary<string, object>();
            if (doc != null) {
                var pinfo = doc.ProjectInformation;

                foreach (BuiltInParameter paramId in new BuiltInParameter[] {
                    BuiltInParameter.PROJECT_ORGANIZATION_NAME,
                    BuiltInParameter.PROJECT_ORGANIZATION_DESCRIPTION,
                    BuiltInParameter.PROJECT_NUMBER,
                    BuiltInParameter.PROJECT_NAME,
                    BuiltInParameter.CLIENT_NAME,
                    BuiltInParameter.PROJECT_BUILDING_NAME,
                    BuiltInParameter.PROJECT_ISSUE_DATE,
                    BuiltInParameter.PROJECT_STATUS,
                    BuiltInParameter.PROJECT_AUTHOR,
                    BuiltInParameter.PROJECT_ADDRESS,
                }) {
                    var param = pinfo.get_Parameter(paramId);
                    if (param != null) {
                        var paramValue = param.ToGLTF();
                        if (paramValue != null)
                            docProps.Add(param.Definition.Name, paramValue);
                    }
                }

                foreach (Parameter param in pinfo.Parameters)
                    if (param.Id.IntegerValue > 0) {
                        var paramValue = param.ToGLTF();
                        if (paramValue != null)
                            docProps.Add(param.Definition.Name, paramValue);
                    }
            }
            return docProps;
        }

        [JsonProperty("id", Order = 1)]
        public string Id { get; set; }

        [JsonProperty("application", Order = 2)]
        public string App { get; set; }

        [JsonProperty("title", Order = 3)]
        public string Title { get; set; }

        [JsonProperty("source", Order = 4)]
        public string Source { get; set; }

        [JsonProperty("levels", Order = 5)]
        public List<uint> Levels { get; set; }

        [JsonProperty("grids", Order = 6)]
        public List<uint> Grids { get; set; }

        [JsonProperty("bounds", Order = 7)]
        public glTFBIMBounds Bounds { get; set; }

        [JsonProperty("containers", Order = 8)]
        public List<glTFBIMPropertyContainer> Containers { get; set; }

        [JsonProperty("properties", Order = 9)]
        public Dictionary<string, object> Properties { get; set; }
    }
}
