using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.GLTF.Extensions.BIM.Properties;
using GLTFRevitExport.GLTF.Package;
using GLTFRevitExport.GLTF.Package.BaseTypes;

namespace GLTFRevitExport.Build {
    class BuildContext {
        public GLTFBuilder Builder { get; }
        public GLTFExportConfigs Configs { get; }
        public glTFBIMAssetExtension AssetExtension { get; }
        public glTFBIMPropertyContainer PropertyContainer { get; }

        public BuildContext(string name, Document doc, GLTFExportConfigs cfgs) {
            // create main gltf builder
            Builder = new GLTFBuilder(name);
            Configs = cfgs;

            // build asset extension and property source (if needed)
            if (cfgs.EmbedParameters)
                AssetExtension = new glTFBIMAssetExtension(doc, this);
            else {
                PropertyContainer = new glTFBIMPropertyContainer($"{name}-properties.json");
                AssetExtension = new glTFBIMAssetExtension(doc, this);
            }

            Builder.SetAsset(
                generatorId: cfgs.GeneratorId,
                copyright: cfgs.CopyrightMessage,
                exts: new glTFExtension[] { AssetExtension },
                extras: cfgs.BuildExtras(doc)
            );
        }

        public List<GLTFPackageItem> Pack(GLTFExportConfigs cfgs) {
            var gltfPack = new List<GLTFPackageItem>();

            // pack the glTF data and get the container
            gltfPack.AddRange(
                Builder.Pack(singleBinary: cfgs.UseSingleBinary)
            );

            if (PropertyContainer != null && PropertyContainer.HasPropertyData)
                gltfPack.Add(
                    new GLTFPackageJsonItem(PropertyContainer.Uri, PropertyContainer.Pack())
                );

            return gltfPack;
        }
    }
}