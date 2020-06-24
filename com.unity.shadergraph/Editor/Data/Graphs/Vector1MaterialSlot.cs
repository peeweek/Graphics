using System;
using System.Collections.Generic;
using UnityEditor.Graphing;
using UnityEditor.ShaderGraph.Drawing.Slots;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ShaderGraph
{
    [Serializable]
    class FloatMaterialSlot : MaterialSlot, IMaterialSlotHasValue<float>
    {
        [SerializeField]
        float m_Value;

        [SerializeField]
        float m_DefaultValue;

        [SerializeField]
        string[] m_Labels;

        public FloatMaterialSlot()
        {
        }

        public FloatMaterialSlot(
            int slotId,
            string displayName,
            string shaderOutputName,
            SlotType slotType,
            float value,
            ShaderStageCapability stageCapability = ShaderStageCapability.All,
            string label1 = "X",
            bool hidden = false)
            : base(slotId, displayName, shaderOutputName, slotType, stageCapability, hidden)
        {
            m_DefaultValue = value;
            m_Value = value;
            m_Labels = new[] { label1 };
        }

        public float defaultValue { get { return m_DefaultValue; } }

        public float value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public override bool isDefaultValue => value.Equals(defaultValue);

        public override VisualElement InstantiateControl()
        {
            return new MultiFloatSlotControlView(owner, m_Labels, () => new Vector4(value, 0f, 0f, 0f), (newValue) => value = newValue.x);
        }

        protected override string ConcreteSlotValueAsVariable()
        {
            return NodeUtils.FloatToShaderValue(value);
        }

        public override void AddDefaultProperty(PropertyCollector properties, GenerationMode generationMode)
        {
            if (!generationMode.IsPreview())
                return;

            var matOwner = owner as AbstractMaterialNode;
            if (matOwner == null)
                throw new Exception(string.Format("Slot {0} either has no owner, or the owner is not a {1}", this, typeof(AbstractMaterialNode)));

            var property = new FloatShaderProperty()
            {
                overrideReferenceName = matOwner.GetVariableNameForSlot(id),
                generatePropertyBlock = false,
                value = value
            };
            properties.AddShaderProperty(property);
        }

        public override SlotValueType valueType { get { return SlotValueType.Float; } }
        public override ConcreteSlotValueType concreteValueType { get { return ConcreteSlotValueType.Float; } }

        public override void GetPreviewProperties(List<PreviewProperty> properties, string name)
        {
            var pp = new PreviewProperty(PropertyType.Float)
            {
                name = name,
                floatValue = value,
            };
            properties.Add(pp);
        }

        public override void CopyValuesFrom(MaterialSlot foundSlot)
        {
            var slot = foundSlot as FloatMaterialSlot;
            if (slot != null)
                value = slot.value;
        }
        public override void CopyDefaultValue(MaterialSlot other)
        {
            base.CopyDefaultValue(other);
            if (other is IMaterialSlotHasValue<float> ms)
            {
                m_DefaultValue = ms.defaultValue;
            }
        }
    }
}
