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
    class SceneBeginAction : BuildBeginAction {
        public SceneBeginAction(View view) : base(view) { }

        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfgs) {
            // start a new gltf scene
            Logger.Log("+ view begin");
            gltf.OpenScene(
                name: element.Name,
                exts: new glTFExtension[] {
                    new glTFBIMNodeExtension(element, cfgs.ExportParameters, PropertyContainer)
                },
                extras: cfgs.BuildExtras(element)
                );

            // open a root node for the scene
            gltf.OpenNode(
                name: string.Format(StringLib.SceneRootNodeName, element.Name),
                matrix: Transform.CreateTranslation(new XYZ(0, 0, 0)).ToGLTF(),
                extras: null,
                exts: null
                );
        }
    }
}
