﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Engine.Aspects;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.Entities
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [NotifyPropertyChanged]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertiesContainerAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Trust Boundary Template")]
    public class TrustBoundaryTemplate : ITrustBoundaryTemplate, IInitializableObject
    {
        public TrustBoundaryTemplate()
        {
        }

        public TrustBoundaryTemplate([Required] string name)
        {
            _id = Guid.NewGuid();
            Name = name;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }

        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IProperty> Properties { get; }
        public bool HasProperty(IPropertyType propertyType)
        {
            return false;
        }
        public IProperty GetProperty(IPropertyType propertyType)
        {
            return null;
        }

        public IProperty AddProperty(IPropertyType propertyType, string value)
        {
            return null;
        }

        public bool RemoveProperty(IPropertyType propertyType)
        {
            return false;
        }

        public bool RemoveProperty(Guid propertyTypeId)
        {
            return false;
        }

        public void ClearProperties()
        {
        }

        public void Apply(IPropertySchema schema)
        {
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("name")]
        protected string _name { get; set; }
        [JsonProperty("description")]
        protected string _description { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Parent]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        protected IThreatModel _model { get; set; }
        [Child]
        [JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects)]
        private AdvisableCollection<IProperty> _properties { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.TrustBoundaryTemplate;

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        [InitializationRequired]
        public ITrustBoundary CreateTrustBoundary([Required] string name)
        {
            ITrustBoundary result;

            using (var scope = UndoRedoManager.OpenScope("Create Trust Boundary from Template"))
            {
                result = _model.AddTrustBoundary(name, this);

                if (result != null)
                {
                    result.Description = Description;
                    this.CloneProperties(result);
                    scope.Complete();
                }
            }

            return result;
        }

        [InitializationRequired]
        public void ApplyTo([NotNull] ITrustBoundary trustBoundary)
        {
            using (var scope = UndoRedoManager.OpenScope("Apply Template to an existing Trust Boundary"))
            {
                trustBoundary.ClearProperties();
                this.CloneProperties(trustBoundary);
                if (trustBoundary is TrustBoundary internalTb)
                {
                    internalTb._templateId = Id;
                    internalTb._template = this;
                }
                scope.Complete();
            }
        }

        public ITrustBoundaryTemplate Clone([NotNull] ITrustBoundaryTemplatesContainer container)
        {
            TrustBoundaryTemplate result = null;

            if (container is IThreatModel model)
            {
                result = new TrustBoundaryTemplate()
                {
                    _id = Id,
                    Name = Name,
                    Description = Description,
                    _model = model,
                    _modelId = model.Id,
                };
                this.CloneProperties(result);

                container.Add(result);
            }

            return result;
        }
        #endregion
    }
}
