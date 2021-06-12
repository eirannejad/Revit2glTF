using System;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.Build.Actions.BaseTypes;

namespace GLTFRevitExport.Build.Actions {
    class ElementBoundsAction : BaseAction {
        protected glTFBIMBounds _bounds;

        public ElementBoundsAction(glTFBIMBounds bounds) => _bounds = bounds;

        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfgs) {
            if (_bounds != null &&
                    gltf.GetActiveNode() is glTFNode activeNode) {
                Logger.Log("> bounds");
                UpdateBounds(
                    gltf,
                    gltf.GetNodeIndex(activeNode),
                    new glTFBIMBounds(_bounds)
                    );
            }
            else
                Logger.Log("x transform");
        }

        void UpdateBounds(GLTFBuilder gltf, uint idx, glTFBIMBounds bounds) {
            if (bounds != null) {
                glTFNode node = gltf.GetNode(idx);
                if (node.Extensions != null) {
                    foreach (var ext in node.Extensions) {
                        if (ext.Value is glTFBIMNodeExtension nodeExt) {
                            if (nodeExt.Bounds != null)
                                nodeExt.Bounds.Union(bounds);
                            else
                                nodeExt.Bounds = new glTFBIMBounds(bounds);

                            int parentIdx = gltf.FindParentNode(idx);
                            if (parentIdx >= 0)
                                UpdateBounds(gltf, (uint)parentIdx, nodeExt.Bounds);
                        }
                    }
                }
            }
        }
    }

}
