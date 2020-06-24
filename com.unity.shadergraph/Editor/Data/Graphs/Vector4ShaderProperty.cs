using System;
using UnityEditor.Graphing;
using UnityEngine;

namespace UnityEditor.ShaderGraph.Internal
{
    [Serializable]
    [FormerName("UnityEditor.ShaderGraph.Vector4ShaderProperty")]
    public sealed class Vector4ShaderProperty : VectorShaderProperty
    {
        internal Vector4ShaderProperty()
        {
            displayName = "Vector4";
        }
        internal override bool isGpuInstanceable => true;
        
        public override PropertyType propertyType => PropertyType.Vector4;
        
        internal override AbstractMaterialNode ToConcreteNode()
        {
            var node = new Vector4Node();
            node.FindInputSlot<FloatMaterialSlot>(Vector4Node.InputSlotXId).value = value.x;
            node.FindInputSlot<FloatMaterialSlot>(Vector4Node.InputSlotYId).value = value.y;
            node.FindInputSlot<FloatMaterialSlot>(Vector4Node.InputSlotZId).value = value.z;
            node.FindInputSlot<FloatMaterialSlot>(Vector4Node.InputSlotWId).value = value.w;
            return node;
        }

        internal override PreviewProperty GetPreviewMaterialProperty()
        {
            return new PreviewProperty(propertyType)
            {
                name = referenceName,
                vector4Value = value
            };
        }

        internal override ShaderInput Copy()
        {
            return new Vector4ShaderProperty()
            {
                displayName = displayName,
                hidden = hidden,
                value = value,
                precision = precision,
                gpuInstanced = gpuInstanced,
            };
        }
    }
}
