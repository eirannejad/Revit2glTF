using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.Build.Actions.BaseTypes;

namespace GLTFRevitExport.Build.Actions {
    class GridAction : BuildBeginAction {
        public GridAction(Element element) : base(element) { }

        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfgs) {
            Logger.Log("> grid");

            Grid grid = (Grid)element;

            // TODO: make a matrix from grid
            float[] gridMatrix = null;

            if (grid.Curve is Line gridLine) {
                // add gltf-bim extension data
                var gltfBim = new glTFBIMNodeExtension(grid, cfgs.ExportParameters, PropertyContainer);

                // grab the two ends of the grid line as grid bounds
                gltfBim.Bounds = new glTFBIMBounds(
                    gridLine.GetEndPoint(0),
                    gridLine.GetEndPoint(1)
                );

                // create level node
                var gridNodeIdx = gltf.OpenNode(
                    name: grid.Name,
                    matrix: gridMatrix,
                    exts: new glTFExtension[] { gltfBim },
                    extras: cfgs.BuildExtras(grid)
                );

                gltf.CloseNode();

                // record the grid in asset
                if (AssetExt != null) {
                    if (AssetExt.Grids is null)
                        AssetExt.Grids = new List<uint>();
                    AssetExt.Grids.Add(gridNodeIdx);
                }
            }

            // not need to do anything else
            return;
        }
    }
}