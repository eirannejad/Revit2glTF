using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using GLTFRevitExport.GLTF;
using GLTFRevitExport.Extensions;
using GLTFRevitExport.GLTF.Schema;
using GLTFRevitExport.GLTF.Extensions.BIM;
using GLTFRevitExport.Properties;
using System.Runtime.CompilerServices;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.DB.Architecture;

namespace GLTFRevitExport.Build.Actions.BaseTypes {
    abstract class BaseAction {
        public abstract void Execute(BuildContext ctx);
    }

    abstract class BaseElementAction : BaseAction {
        protected Element element;

        public BaseElementAction(Element e) => element = e;

        public bool Passes(ElementFilter filter) {
            if (element is null)
                return true;
            return filter.PassesFilter(element);
        }
    }

    abstract class BuildBeginAction : BaseElementAction {
        public BuildBeginAction(Element e) : base(e) { }
    }

    abstract class BuildEndAction : BaseAction { }
}