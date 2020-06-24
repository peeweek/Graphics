using System.Reflection;
using UnityEngine;

namespace UnityEditor.ShaderGraph
{
    [Title("Artistic", "Mask", "Color Mask")]
    class ColorMaskNode : CodeFunctionNode
    {
        public ColorMaskNode()
        {
            name = "Color Mask";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("Unity_ColorMask", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string Unity_ColorMask(
            [Slot(0, Binding.None)] Vector3 In,
            [Slot(1, Binding.None)] ColorRGB MaskColor,
            [Slot(2, Binding.None)] Float Range,
            [Slot(4, Binding.None)] Float Fuzziness,
            [Slot(3, Binding.None)] out Float Out)
        {
            return
                @"
{
    $precision Distance = distance(MaskColor, In);
    Out = saturate(1 - (Distance - Range) / max(Fuzziness, 1e-5));
}";
        }
    }
}
