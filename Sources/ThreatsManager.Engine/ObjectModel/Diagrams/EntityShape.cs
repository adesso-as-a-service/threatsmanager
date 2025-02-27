﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Diagrams
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [AssociatedIdChanger]
    [PropertiesContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class EntityShape : IEntityShape, IThreatModelChild, IInitializableObject
    {
        public EntityShape()
        {

        }

        public EntityShape([NotNull] IEntity entity) : this()
        {
            _model = entity.Model;
            _associated = entity;
        }

        public bool IsInitialized => Model != null && _associatedId != Guid.Empty;

        #region Default implementation.
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

        public void Unapply(IPropertySchema schema)
        {
        }

        public Guid SourceTMId { get; }

        public string SourceTMName { get; }

        public string VersionId { get; }

        public string VersionAuthor { get; }

        public void SetSourceInfo(IThreatModel source)
        {
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [Child]
        [JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects)]
        private AdvisableCollection<IProperty> _properties { get; set; }
        [JsonProperty("sourceTMId")]
        protected Guid _sourceTMId { get; set; }
        [JsonProperty("sourceTMName")]
        protected string _sourceTMName { get; set; }
        [JsonProperty("versionId")]
        protected string _versionId { get; set; }
        [JsonProperty("versionAuthor")]
        protected string _versionAuthor { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.EntityShape;

        [Reference]
        [field: NotRecorded]
        [field: UpdateAssociatedId]
        private IEntity _associated { get; set; }

        [JsonProperty("id")]
        private Guid _associatedId { get; set; }

        public Guid AssociatedId => _associatedId;

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IIdentity Identity => _associated ?? (_associated = Model.GetEntity(_associatedId));

        [Child]
        private RecordablePointF _recordablePosition { get; set; }

        [property: NotRecorded]
        [JsonProperty("pos")]
        [SafeForDependencyAnalysis]
        public PointF Position
        {
            get
            {
                return _recordablePosition?.Position ?? PointF.Empty;
            }

            set
            {
                if (_recordablePosition == null)
                    _recordablePosition = new RecordablePointF(value);
                else
                {
                    _recordablePosition.Position = value;
                }
            }
        }

        public IEntityShape Clone([NotNull] IEntityShapesContainer container)
        {
            EntityShape result = null;
            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Entity Shape"))
                {
                    result = new EntityShape()
                    {
                        _associatedId = _associatedId,
                        _model = model,
                        _modelId = model.Id,
                        Position = new PointF(Position.X, Position.Y),
                    };
                    container.Add(result);
                    this.CloneProperties(result);

                    if (model.Id != _modelId)
                        result.SetSourceInfo(Model);

                    scope?.Complete();
                }
            }

            return result;
        }
        #endregion
    }
}
