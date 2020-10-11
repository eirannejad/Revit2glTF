﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace GLTFRevitExport.GLTFExtension {
    [AttributeUsage(AttributeTargets.Property)]
    public class APIBuiltinParametersAttribute : Attribute {
        public BuiltInParameter TypeParam { get; private set; }
        public BuiltInParameter InstanceParam { get; private set; }

        public APIBuiltinParametersAttribute(BuiltInParameter typeParam, BuiltInParameter instParam) {
            TypeParam = typeParam;
            InstanceParam = instParam;
        }
    }
}
