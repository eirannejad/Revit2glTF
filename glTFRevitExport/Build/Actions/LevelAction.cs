using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.Extensions;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.Build.Actions.BaseTypes;

namespace GLTFRevitExport.Build.Actions {
    class LevelAction : BuildBeginAction {
        BoundingBoxXYZ _extentsBbox;

        public LevelAction(Element element, BoundingBoxXYZ extents) : base(element) {
            _extentsBbox = extents;
        }

        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfgs) {
            Logger.Log("> level");

            Level level = (Level)element;

            // make a matrix from level elevation
            float elev = level.Elevation.ToGLTFLength();
            float[] elevMatrix = null;
            // no matrix is specified for a level at elev 0
            if (elev != 0f) {
                elevMatrix = new float[16] {
                            1f,   0f,   0f,   0f,
                            0f,   1f,   0f,   0f,
                            0f,   0f,   1f,   0f,
                            0f,   elev, 0f,   1f
                        };
            }

            // create level node
            var levelNodeIdx = gltf.OpenNode(
                name: level.Name,
                matrix: elevMatrix,
                exts: new glTFExtension[] {
                        new glTFBIMNodeExtension(level, cfgs.ExportParameters, PropertyContainer)
                },
                extras: cfgs.BuildExtras(level)
            );

            // set level bounds
            if (_extentsBbox != null) {
                var bounds = new glTFBIMBounds(_extentsBbox);
                glTFNode node = gltf.GetNode(levelNodeIdx);
                if (node.Extensions != null) {
                    foreach (var ext in node.Extensions) {
                        if (ext.Value is glTFBIMNodeExtension nodeExt) {
                            if (nodeExt.Bounds != null)
                                nodeExt.Bounds.Union(bounds);
                            else
                                nodeExt.Bounds = new glTFBIMBounds(bounds);
                        }
                    }
                }
            }

            gltf.CloseNode();

            // record the level in asset
            if (AssetExt != null) {
                if (AssetExt.Levels is null)
                    AssetExt.Levels = new List<uint>();
                AssetExt.Levels.Add(levelNodeIdx);
            }

            // not need to do anything else
            return;
        }
    }
}