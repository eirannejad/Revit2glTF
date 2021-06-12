using System;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.Build.Actions.BaseTypes;

namespace GLTFRevitExport.Build.Actions {
    class ElementTransformAction : BaseAction {
        public float[] Matrix;

        public ElementTransformAction(float[] matrix) => Matrix = matrix;

        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfgs) {
            if (gltf.GetActiveNode() is glTFNode activeNode) {
                Logger.Log("> transform");
                activeNode.Matrix = Matrix;
            }
            else
                Logger.Log("x transform");
        }
    }
}
