﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    public partial class PropertySchema
    {
        [Child]
        [JsonProperty("propertyTypes", ItemTypeNameHandling = TypeNameHandling.Objects)]
        private AdvisableCollection<IPropertyType> _propertyTypes { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<IPropertyType> PropertyTypes => _propertyTypes?.OrderBy(x => x.Priority);

        public event Action<IPropertySchema, IPropertyType> PropertyTypeAdded;
        public event Action<IPropertySchema, IPropertyType> PropertyTypeRemoved;

        public IPropertyType GetPropertyType(Guid id)
        {
            return _propertyTypes?.FirstOrDefault(x => x.Id == id);
        }

        public IPropertyType GetPropertyType([Required] string name)
        {
            return _propertyTypes?.FirstOrDefault(x => name.IsEqual(x.Name));
        }

        public IPropertyType AddPropertyType([Required] string name, PropertyValueType type)
        {
            IPropertyType result = null;

            if (GetPropertyType(name) == null)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Property Type"))
                { 
                    var propertyTypeInterface = type.GetEnumType();
                    if (propertyTypeInterface != null)
                    {
                        var implementation = Assembly.GetExecutingAssembly().DefinedTypes
                            .FirstOrDefault(x => x.ImplementedInterfaces.Contains(propertyTypeInterface));

                        if (implementation != null)
                        {
                            result = Activator.CreateInstance(implementation, name, this) as IPropertyType;
                        }
                    }

                    if (result != null)
                    {
                        if (_propertyTypes == null)
                            _propertyTypes = new AdvisableCollection<IPropertyType>();

                        UndoRedoManager.Attach(result, Model);
                        _propertyTypes.Add(result);
                        scope?.Complete();
                    }
                }

                if (result != null)
                    PropertyTypeAdded?.Invoke(this, result);
            }

            return result;
        }

        public bool RemovePropertyType(Guid id)
        {
            bool result = false;

            var propertyType = GetPropertyType(id);

            if (propertyType != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Property Type"))
                {
                    result = _propertyTypes.Remove(propertyType);
                    if (result)
                    {
                        UndoRedoManager.Detach(propertyType);
                        scope?.Complete();
                    }
                }
                if (result)
                    PropertyTypeRemoved?.Invoke(this, propertyType);
            }

            return result;
        }
    }
}
