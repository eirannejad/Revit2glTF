using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

using GLTFRevitExport.GLTF.Extensions.BIM;

namespace GLTFRevitExport.Build.Actions {
    class LinkBeginAction : ElementBeginAction {
        public Document LinkDocument { get; private set; }
        public string LinkId { get; private set; }

        public LinkBeginAction(RevitLinkInstance link, RevitLinkType linkType, Document linkedDoc)
            : base(link, linkType) {
            LinkDocument = linkedDoc;
            LinkId = element.UniqueId;
        }
    }

    class LinkTransformAction : ElementTransformAction {
        public LinkTransformAction(float[] xform) : base(xform) { }
    }

    class LinkBoundsAction : ElementBoundsAction {
        public LinkBoundsAction() : base(null) { }

        public glTFBIMBounds Bounds {
            get => _bounds;
            set => _bounds = value;
        }
    }

    class LinkEndAction : ElementEndAction {
    }
}