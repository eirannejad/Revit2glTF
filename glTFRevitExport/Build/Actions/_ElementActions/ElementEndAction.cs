using System;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.Build.Actions.BaseTypes;

namespace GLTFRevitExport.Build.Actions {
    class ElementEndAction : BuildEndAction {
        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfgs) {
            Logger.Log("- element end");

            // close instance node
            gltf.CloseNode();
            // close type node
            if (cfgs.ExportHierarchy)
                gltf.CloseNode();
        }
    }
}
