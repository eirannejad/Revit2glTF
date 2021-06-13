using System;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.Extensions;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.Build.Actions.BaseTypes;
using GLTFRevitExport.Properties;

namespace GLTFRevitExport.Build.Actions {
    class SceneEndAction : BuildEndAction {
        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfg) {
            Logger.Log("- view end");

            // close root node
            gltf.CloseNode();
            // close scene
            gltf.CloseScene();
        }
    }
}
