using System;
using System.Collections.Generic;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.Extensions;
using GLTFRevitExport.Build.Geometry;
using GLTFRevitExport.Build.Actions.BaseTypes;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace GLTFRevitExport.Build.Actions {
    class PartFromElementAction : BaseElementAction {
        View _view = null;
        public PartFromElementAction(View view, Element element) : base(element) { _view = view; }

        public override void Execute(GLTFBuilder gltf, GLTFExportConfigs cfgs) {
            // open a new node and store its id
            Logger.Log("> custom element");

            foreach (var geom in element.get_Geometry(new Options { View = _view })) {
                if (geom is Mesh mesh) {

                    gltf.OpenNode(
                        name: element.Name,
                        matrix: null,
                        exts: new glTFExtension[] {
                            new glTFBIMNodeExtension(element, cfgs.ExportParameters, PropertyContainer)
                        },
                        extras: cfgs.BuildExtras(element)
                        );

                    var vertices = new List<float>();
                    foreach (var vec in mesh.Vertices)
                        vertices.AddRange(vec.ToGLTF());

                    var faces = new List<uint>();
                    for (int i = 0; i < mesh.NumTriangles; i++) {
                        var t = mesh.get_Triangle(i);

                        // if element is a topography change associated with
                        // a building pad, the face normals need to be flipped for
                        // the side walls, but not for the base faces
                        if (element is TopographySurface tp
                                && tp.IsAssociatedWithBuildingPad) {
                            // if the vertices are horizontal (their Z are almost identical)
                            double zAvg = (t.get_Vertex(0).Z + t.get_Vertex(1).Z + t.get_Vertex(2).Z) / 3.0;
                            if (zAvg.AlmostEquals(t.get_Vertex(0).Z)) {
                                // then add the faces
                                faces.Add(t.get_Index(0));
                                faces.Add(t.get_Index(1));
                                faces.Add(t.get_Index(2));
                            }
                            // otherwise flip their normal
                            else {
                                faces.Add(t.get_Index(2));
                                faces.Add(t.get_Index(1));
                                faces.Add(t.get_Index(0));
                            }
                        }
                        else {
                            faces.Add(t.get_Index(0));
                            faces.Add(t.get_Index(1));
                            faces.Add(t.get_Index(2));
                        }
                    }

                    var primIndex = gltf.AddPrimitive(
                        vertices: vertices.ToArray(),
                        normals: null,
                        faces: faces.ToArray()
                        );

                    // if mesh has material
                    if (mesh.MaterialElementId != ElementId.InvalidElementId) {
                        Material material = element.Document.GetElement(mesh.MaterialElementId) as Material;
                        var existingMaterialIndex =
                            gltf.FindMaterial(
                                (mat) => {
                                    if (mat.Extensions != null) {
                                        foreach (var ext in mat.Extensions)
                                            if (ext.Value is glTFBIMMaterialExtensions matExt)
                                                return matExt.Id == material.UniqueId;
                                    }
                                    return false;
                                }
                            );

                        // check if material already exists
                        if (existingMaterialIndex >= 0) {
                            gltf.UpdateMaterial(
                                primitiveIndex: primIndex,
                                materialIndex: (uint)existingMaterialIndex
                            );
                        }
                        // otherwise make a new material and get its index
                        else {
                            gltf.AddMaterial(
                                primitiveIndex: primIndex,
                                name: material.Name,
                                color: material.Color.IsValid ? material.Color.ToGLTF() : cfgs.DefaultColor.ToGLTF(),
                                exts: new glTFExtension[] {
                        new glTFBIMMaterialExtensions(material, cfgs.ExportParameters, PropertyContainer)
                                },
                                extras: null
                            );
                        }
                    }

                    // TODO: otherwise grab the color from graphics styles?
                    else if (mesh.GraphicsStyleId != ElementId.InvalidElementId) {
                    }

                    gltf.CloseNode();
                }
            }
        }
    }
}
