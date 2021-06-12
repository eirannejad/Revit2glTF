using System;

using Autodesk.Revit.DB;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.Extensions;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.Build.Actions.BaseTypes;

namespace GLTFRevitExport.Build.Actions {
    class ElementBeginAction : BuildBeginAction {
        readonly ElementType _elementType;

        public string Uri { get; set; } = null;

        public ElementBeginAction(Element element, ElementType type) : base(element) {
            _elementType = type;
        }

        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfgs) {
            // open a new node and store its id
            Logger.Log("+ element begin");

            // node filter to pass to gltf builder
            string targetId = string.Empty;
            bool nodeFilter(glTFNode node) {
                if (node.Extensions != null) {
                    foreach (var ext in node.Extensions)
                        if (ext.Value is glTFBIMNodeExtension nodeExt)
                            return nodeExt.Id == targetId;
                }
                return false;
            }

            // create a node for its type
            // attemp at finding previously created node for this type
            // but only search children of already open node
            if (cfgs.ExportHierarchy) {
                targetId = _elementType.GetId();
                var typeNodeIdx = gltf.FindChildNode(nodeFilter);

                if (typeNodeIdx >= 0) {
                    gltf.OpenExistingNode((uint)typeNodeIdx);
                }
                // otherwise create and open a new node for this type
                else {
                    var bimExt = new glTFBIMNodeExtension(
                        e: _elementType,
                        includeParameters: cfgs.ExportParameters,
                        propContainer: PropertyContainer
                    );

                    gltf.OpenNode(
                        name: _elementType.Name,
                        matrix: null,
                        exts: new glTFExtension[] { bimExt },
                        extras: cfgs.BuildExtras(_elementType)
                    );
                }
            }

            // create a node for this instance
            // attemp at finding previously created node for this instance
            // but only search children of already open type node
            targetId = element.GetId();
            var instNodeIdx = gltf.FindChildNode(nodeFilter);

            if (instNodeIdx >= 0) {
                gltf.OpenExistingNode((uint)instNodeIdx);
            }
            // otherwise create and open a new node for this type
            else {
                var bimExt = new glTFBIMNodeExtension(
                    e: element,
                    includeParameters: cfgs.ExportParameters,
                    propContainer: PropertyContainer
                ) {
                    Uri = Uri
                };

                var newNodeIdx = gltf.OpenNode(
                    name: element.Name,
                    matrix: null,
                    exts: new glTFExtension[] { bimExt },
                    extras: cfgs.BuildExtras(element)
                );
            }
        }
    }
}
