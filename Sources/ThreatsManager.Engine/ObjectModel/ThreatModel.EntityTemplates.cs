﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.Entities;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [Child]
        [JsonProperty("entityTemplates", ItemTypeNameHandling = TypeNameHandling.Objects, Order = 45)]
        private AdvisableCollection<IEntityTemplate> _entityTemplates { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<IEntityTemplate> EntityTemplates => _entityTemplates?.AsEnumerable();

        [InitializationRequired]
        public IEntityTemplate GetEntityTemplate(Guid id)
        {
            return _entityTemplates?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public IEnumerable<IEntityTemplate> GetEntityTemplates(EntityType type)
        {
            return _entityTemplates?.Where(x => type == x.EntityType);
        }

        [InitializationRequired]
        public void Add([NotNull] IEntityTemplate entityTemplate)
        {
            using (var scope = UndoRedoManager.OpenScope("Add Entity Template"))
            {
                if (_entityTemplates == null)
                    _entityTemplates = new AdvisableCollection<IEntityTemplate>();

                _entityTemplates.Add(entityTemplate);
                UndoRedoManager.Attach(entityTemplate);
                scope.Complete();

                ChildCreated?.Invoke(entityTemplate);
            }
        }

        [InitializationRequired]
        public IEntityTemplate AddEntityTemplate([Required] string name, string description, [NotNull] IEntity source)
        {
            return AddEntityTemplate(name, description, null, null, null, source);
        }

        [InitializationRequired]
        public IEntityTemplate AddEntityTemplate([Required] string name, string description, 
            Bitmap bigImage, Bitmap image, Bitmap smallImage, [NotNull] IEntity source)
        {
            var result = new EntityTemplate(name)
            {
                Description = description,
                BigImage = bigImage ?? source.GetImage(ImageSize.Big), 
                Image = image ?? source.GetImage(ImageSize.Medium), 
                SmallImage = smallImage ?? source.GetImage(ImageSize.Small),
                EntityType = GetEntityType(source)
            };
            source.CloneProperties(result);
            Add(result);

            return result;
        }

        [InitializationRequired]
        public IEntityTemplate AddEntityTemplate([Required] string name, string description, EntityType entityType)
        {
            return AddEntityTemplate(name, description, null, null, null, entityType);
        }

        [InitializationRequired]
        public IEntityTemplate AddEntityTemplate([Required] string name, string description, 
            Bitmap bigImage, Bitmap image, Bitmap smallImage, EntityType entityType)
        {
            var result = new EntityTemplate(name)
            {
                Description = description,
                EntityType = entityType
            };
            switch (entityType)
            {
                case EntityType.ExternalInteractor:
                    result.BigImage = bigImage ?? Resources.external_big;
                    result.Image = image ?? Resources.external;
                    result.SmallImage = smallImage ?? Resources.external_small;
                    break;
                case EntityType.Process:
                    result.BigImage = bigImage ?? Resources.process_big;
                    result.Image = image ?? Resources.process;
                    result.SmallImage = smallImage ?? Resources.process_small;
                    break;
                case EntityType.DataStore:
                    result.BigImage = bigImage ?? Resources.storage_big;
                    result.Image = image ?? Resources.storage;
                    result.SmallImage = smallImage ?? Resources.storage_small;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
            }
            Add(result);

            return result;
        }

        private EntityType GetEntityType([NotNull] IEntity entity)
        {
            EntityType result;

            if (entity is IExternalInteractor)
                result = EntityType.ExternalInteractor;
            else if (entity is IProcess)
                result = EntityType.Process;
            else if (entity is IDataStore)
                result = EntityType.DataStore;
            else
                throw new ArgumentException("The Entity is not of a supported type.");

            return result;
        }

        [InitializationRequired(false)]
        public bool RemoveEntityTemplate(Guid id)
        {
            bool result = false;

            var template = GetEntityTemplate(id);

            if (template != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Entity Template"))
                {
                    result = _entityTemplates.Remove(template);
                    if (result)
                    {
                        UndoRedoManager.Detach(template);
                        scope.Complete();

                        ChildRemoved?.Invoke(template);
                    }
                }
            }

            return result;
        }
    }
}