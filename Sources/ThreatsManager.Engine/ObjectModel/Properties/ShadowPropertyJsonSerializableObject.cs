﻿using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ShadowPropertyAspect]
    public class ShadowPropertyJsonSerializableObject : PropertyJsonSerializableObject, IShadowProperty
    {
        public ShadowPropertyJsonSerializableObject()
        {

        }

        public ShadowPropertyJsonSerializableObject([NotNull] IPropertyJsonSerializableObject original) : base(original.PropertyType as IJsonSerializableObjectPropertyType)
        {
            _originalId = original.Id;
            _original = original;
        }

        #region Default implementation.
        [Reference]
        [field: NonSerialized]
        public IProperty Original { get; }
        public bool IsOverridden { get; }
        public void RevertToOriginal()
        {
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("originalId")]
        private Guid _originalId { get; set; }
        [JsonProperty("overridden")]
        private bool _overridden { get; set; }
        [Reference]
        [field: NonSerialized]
        [field: NotRecorded]
        private IProperty _original { get; set; }
        #endregion    

        #region Specific implementation.
        public override object Value
        {
            get
            {
                var result = base.Value;

                if (!_overridden)
                {
                    if (Original is IPropertyJsonSerializableObject originalProperty)
                        result = originalProperty.Value;
                }

                return result;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (Original is IPropertyJsonSerializableObject originalProperty)
                {
                    if (value != originalProperty.Value)
                    {
                        if (!_overridden)
                        {
                            _overridden = true;
                            InvokeChanged();
                        }

                        if (base.Value != value)
                        {
                            base.Value = value;
                        }
                    }
                    else
                    {
                        if (_overridden)
                        {
                            _overridden = false;
                            InvokeChanged();
                        }
                    }
                }
            }
        }
        #endregion
    }
}