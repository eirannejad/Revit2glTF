using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

using GLTFRevitExport.Extensions;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.Build.Actions.BaseTypes;
using GLTFRevitExport.Build.Geometry;

namespace GLTFRevitExport.Export {
    sealed partial class ExportContext : IExportContext {
        /// <summary>
        /// Configurations for the active export
        /// </summary>
        readonly GLTFExportConfigs _cfgs = new GLTFExportConfigs();

        /// <summary>
        /// Document stack to hold the documents being processed.
        /// A stack is used to allow processing nested documents (linked docs)
        /// </summary>
        readonly Stack<Document> _docStack = new Stack<Document>();

        /// <summary>
        /// Transform of the embedded document
        /// </summary>
        float[] _linkMatrix = null;

        /// <summary>
        /// View stack to hold the view being processed.
        /// A stack is used to allow referencing view when needed.
        /// It is not expected for this stack to hold more than one view,
        /// however stack has been used for consistency
        /// </summary>
        readonly Stack<View> _viewStack = new Stack<View>();

        /// <summary>
        /// Queue of actions collected during export. These actions are then
        /// played back on each .Build call to create separate glTF outputs
        /// </summary>
        readonly Queue<BaseAction> _actions = new Queue<BaseAction>();

        /// <summary>
        /// List of processed elements by their unique id
        /// </summary>
        readonly List<string> _processed = new List<string>();

        /// <summary>
        /// Flag to mark current node as skipped
        /// </summary>
        bool _skipElement = false;


        readonly Stack<PartData> _partStack = new Stack<PartData>();

        glTFBIMBounds CalculateBounds(float[] matrix = null) {

            glTFBIMBounds CalculateMeshBounds(Transform xform) {
                float minx, miny, minz, maxx, maxy, maxz;
                minx = miny = minz = maxx = maxy = maxz = float.NaN;

                foreach (var partData in _partStack)
                    foreach (var vertex in partData.Primitive.Vertices) {
                        var vtx = vertex;
                        if (xform is Transform)
                            vtx = vtx.Transform(xform);

                        minx = minx is float.NaN || vtx.X < minx ? vtx.X : minx;
                        miny = miny is float.NaN || vtx.Y < miny ? vtx.Y : miny;
                        minz = minz is float.NaN || vtx.Z < minz ? vtx.Z : minz;
                        maxx = maxx is float.NaN || vtx.X > maxx ? vtx.X : maxx;
                        maxy = maxy is float.NaN || vtx.Y > maxy ? vtx.Y : maxy;
                        maxz = maxz is float.NaN || vtx.Z > maxz ? vtx.Z : maxz;
                    }

                return new glTFBIMBounds(
                    minx, miny, minz,
                    maxx, maxy, maxz
                );
            };

            Transform mxform = null;
            if (matrix != null)
                mxform = matrix.FromGLTFMatrix();

            // if link matrix is available we are processing mesh in a link
            // therefore .LinkHostBounds must be set on the calculated bounds
            glTFBIMBounds linkHostBounds = null;
            if (_linkMatrix != null) {
                var lxform = _linkMatrix.FromGLTFMatrix();
                if (mxform != null)
                    lxform = lxform.Multiply(mxform);
                linkHostBounds = CalculateMeshBounds(lxform);
                if (_cfgs.EmbedLinkedModels)
                    return linkHostBounds;
            }

            glTFBIMBounds finalBounds;
            if (mxform != null)
                finalBounds = CalculateMeshBounds(mxform);
            else
                finalBounds = CalculateMeshBounds(null);

            finalBounds.LinkHostBounds = linkHostBounds;
            return finalBounds;
        }

        float[] LocalizePartStack() {
            List<float> vx = new List<float>();
            List<float> vy = new List<float>();
            List<float> vz = new List<float>();

            foreach (var partData in _partStack)
                foreach (var vtx in partData.Primitive.Vertices) {
                    vx.Add(vtx.X);
                    vy.Add(vtx.Y);
                    vz.Add(vtx.Z);
                }

            var min = new VectorData(vx.Min(), vy.Min(), vz.Min());
            var max = new VectorData(vx.Max(), vy.Max(), vz.Max());
            var anchor = min + ((max - min) / 2f);
            var translate = new VectorData(0, 0, 0) - anchor;

            foreach (var partData in _partStack)
                foreach (var vtx in partData.Primitive.Vertices) {
                    vtx.X += translate.X;
                    vtx.Y += translate.Y;
                    vtx.Z += translate.Z;
                }

            return new float[16] {
                1f,             0f,             0f,             0f,
                0f,             1f,             0f,             0f,
                0f,             0f,             1f,             0f,
                -translate.X,   -translate.Y,   -translate.Z,    1f
            };
        }
    }
}
