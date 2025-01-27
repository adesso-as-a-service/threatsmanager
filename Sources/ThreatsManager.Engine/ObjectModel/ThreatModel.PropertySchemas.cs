﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [Child]
        [JsonProperty("schemas", Order = 30)]
        private AdvisableCollection<PropertySchema> _schemas { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<IPropertySchema> Schemas => _schemas?.OrderBy(x => x.Priority);

        [InitializationRequired]
        public IPropertySchema GetSchema([Required] string name, [Required] string nspace)
        {
            return _schemas?.FirstOrDefault(x => name.IsEqual(x.Name) && nspace.IsEqual(x.Namespace));
        }

        [InitializationRequired]
        public IPropertySchema GetSchema(Guid schemaId)
        {
            return _schemas?.FirstOrDefault(x => x.Id == schemaId);
        }

        [InitializationRequired]
        public void ApplySchema(Guid schemaId)
        {
            var schema = GetSchema(schemaId);
            if (schema != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Apply Schema"))
                {
                    if (schema.AppliesTo.HasFlag(Scope.ExternalInteractor))
                    {
                        ApplySchema<IExternalInteractor>(schema);
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Process))
                    {
                        ApplySchema<IProcess>(schema);
                    }

                    if (schema.AppliesTo.HasFlag(Scope.DataStore))
                    {
                        ApplySchema<IDataStore>(schema);
                    }

                    if (schema.AppliesTo.HasFlag(Scope.DataFlow))
                    {
                        var list = _flows?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.TrustBoundary))
                    {
                        var list = _groups?.OfType<ITrustBoundary>().ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatType))
                    {
                        var list = _threatTypes?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatTypeMitigation))
                    {
                        var list = _threatTypes?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                var mitigations = current.Mitigations?.ToArray();
                                if (mitigations?.Any() ?? false)
                                {
                                    foreach (var mitigation in mitigations)
                                        mitigation?.Apply(schema);
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatTypeWeakness))
                    {
                        var list = _threatTypes?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                var weaknesses = current.Weaknesses?.ToArray();
                                if (weaknesses?.Any() ?? false)
                                {
                                    foreach (var weakness in weaknesses)
                                        weakness?.Apply(schema);
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Weakness))
                    {
                        var list = _weaknesses?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.WeaknessMitigation))
                    {
                        var list = _weaknesses?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                var wms = current.Mitigations?.ToArray();
                                if (wms?.Any() ?? false)
                                {
                                    foreach (var wm in wms)
                                    {
                                        wm.Apply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatEvent))
                    {
                        var threatEvents = GetThreatEvents();
                        if (threatEvents?.Any() ?? false)
                        {
                            foreach (var threatEvent in threatEvents)
                            {
                                threatEvent.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatEventScenario))
                    {
                        var threatEvents = GetThreatEvents();
                        if (threatEvents?.Any() ?? false)
                        {
                            foreach (var threatEvent in threatEvents)
                            {
                                var ets = threatEvent.Scenarios?.ToArray();
                                if (ets?.Any() ?? false)
                                {
                                    foreach (var currEts in ets)
                                    {
                                        currEts?.Apply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatEventMitigation))
                    {
                        var threatEvents = GetThreatEvents();
                        if (threatEvents?.Any() ?? false)
                        {
                            foreach (var threatEvent in threatEvents)
                            {
                                var tms = threatEvent.Mitigations?.ToArray();
                                if (tms?.Any() ?? false)
                                {
                                    foreach (var tm in tms)
                                        tm?.Apply(schema);
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Vulnerability))
                    {
                        var vulnerabilities = GetVulnerabilities();
                        if (vulnerabilities?.Any() ?? false)
                        {
                            foreach (var vulnerability in vulnerabilities)
                            {
                                vulnerability.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.VulnerabilityMitigation))
                    {
                        var vulnerabilities = GetVulnerabilities();
                        if (vulnerabilities?.Any() ?? false)
                        {
                            foreach (var vulnerability in vulnerabilities)
                            {
                                var vms = vulnerability.Mitigations?.ToArray();
                                if (vms?.Any() ?? false)
                                {
                                    foreach (var vm in vms)
                                    {
                                        vm.Apply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Mitigation))
                    {
                        var list = _mitigations?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Diagram))
                    {
                        var list = _diagrams?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.EntityShape))
                    {
                        var diagrams = _diagrams?.ToArray();
                        if (diagrams?.Any() ?? false)
                        {
                            foreach (var diagram in diagrams)
                            {
                                var shapes = diagram.Entities?.ToArray();
                                if (shapes?.Any() ?? false)
                                {
                                    foreach (var shape in shapes)
                                    {
                                        shape.Apply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.GroupShape))
                    {
                        var diagrams = _diagrams?.ToArray();
                        if (diagrams?.Any() ?? false)
                        {
                            foreach (var diagram in diagrams)
                            {
                                var shapes = diagram.Groups?.ToArray();
                                if (shapes?.Any() ?? false)
                                {
                                    foreach (var shape in shapes)
                                    {
                                        shape.Apply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.EntityShape))
                    {
                        var diagrams = _diagrams?.ToArray();
                        if (diagrams?.Any() ?? false)
                        {
                            foreach (var diagram in diagrams)
                            {
                                var shapes = diagram.Entities?.ToArray();
                                if (shapes?.Any() ?? false)
                                {
                                    foreach (var shape in shapes)
                                    {
                                        shape.Apply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Link))
                    {
                        var diagrams = _diagrams?.ToArray();
                        if (diagrams?.Any() ?? false)
                        {
                            foreach (var diagram in diagrams)
                            {
                                var links = diagram.Links?.ToArray();
                                if (links?.Any() ?? false)
                                {
                                    foreach (var link in links)
                                    {
                                        link.Apply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatModel))
                    {
                        this.Apply(schema);
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatActor))
                    {
                        var list = _actors?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.EntityTemplate))
                    {
                        var list = _entityTemplates?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.FlowTemplate))
                    {
                        var list = _flowTemplates?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.TrustBoundaryTemplate))
                    {
                        var list = _trustBoundaryTemplates?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.LogicalGroup))
                    {
                        // TODO: Expand when the concept of Logical Group will be introduced.
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Severity))
                    {
                        var list = _severities?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Strength))
                    {
                        var list = _strengths?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Apply(schema);
                            }
                        }
                    }

                    scope?.Complete();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void ApplySchema<T>([NotNull] IPropertySchema schema) where T : IEntity
        {
            var list = _entities?.Where(x => x is T).ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var current in list)
                {
                    current?.Apply(schema);
                }
            }

            IEnumerable<IEntityTemplate> templates = null;
            if (typeof(T) == typeof(IExternalInteractor)) 
                templates = _entityTemplates?.Where(x => x.EntityType == EntityType.ExternalInteractor).ToArray();
            else if (typeof(T) == typeof(IProcess)) 
                templates = _entityTemplates?.Where(x => x.EntityType == EntityType.Process).ToArray();
            else if (typeof(T) == typeof(IDataStore)) 
                templates = _entityTemplates?.Where(x => x.EntityType == EntityType.DataStore).ToArray();
            if (templates?.Any() ?? false)
            {
                foreach (var current in templates)
                {
                    current?.Apply(schema);
                }
            }
        }

        [InitializationRequired]
        public void UnapplySchema(Guid schemaId)
        {
            var schema = GetSchema(schemaId);
            if (schema != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Unapply Schema"))
                {
                    if (schema.AppliesTo.HasFlag(Scope.ExternalInteractor))
                    {
                        UnapplySchema<IExternalInteractor>(schema);
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Process))
                    {
                        UnapplySchema<IProcess>(schema);
                    }

                    if (schema.AppliesTo.HasFlag(Scope.DataStore))
                    {
                        UnapplySchema<IDataStore>(schema);
                    }

                    if (schema.AppliesTo.HasFlag(Scope.DataFlow))
                    {
                        var list = _flows?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.TrustBoundary))
                    {
                        var list = _groups?.OfType<ITrustBoundary>().ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatType))
                    {
                        var list = _threatTypes?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatTypeMitigation))
                    {
                        var list = _threatTypes?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                var mitigations = current.Mitigations?.ToArray();
                                if (mitigations?.Any() ?? false)
                                {
                                    foreach (var mitigation in mitigations)
                                        mitigation?.Unapply(schema);
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatTypeWeakness))
                    {
                        var list = _threatTypes?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                var weaknesses = current.Weaknesses?.ToArray();
                                if (weaknesses?.Any() ?? false)
                                {
                                    foreach (var weakness in weaknesses)
                                        weakness?.Unapply(schema);
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Weakness))
                    {
                        var list = _weaknesses?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.WeaknessMitigation))
                    {
                        var list = _weaknesses?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                var wms = current.Mitigations?.ToArray();
                                if (wms?.Any() ?? false)
                                {
                                    foreach (var wm in wms)
                                    {
                                        wm.Unapply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatEvent))
                    {
                        var threatEvents = GetThreatEvents();
                        if (threatEvents?.Any() ?? false)
                        {
                            foreach (var threatEvent in threatEvents)
                            {
                                threatEvent.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatEventScenario))
                    {
                        var threatEvents = GetThreatEvents();
                        if (threatEvents?.Any() ?? false)
                        {
                            foreach (var threatEvent in threatEvents)
                            {
                                var ets = threatEvent.Scenarios?.ToArray();
                                if (ets?.Any() ?? false)
                                {
                                    foreach (var currEts in ets)
                                    {
                                        currEts?.Unapply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatEventMitigation))
                    {
                        var threatEvents = GetThreatEvents();
                        if (threatEvents?.Any() ?? false)
                        {
                            foreach (var threatEvent in threatEvents)
                            {
                                var tms = threatEvent.Mitigations?.ToArray();
                                if (tms?.Any() ?? false)
                                {
                                    foreach (var tm in tms)
                                        tm?.Unapply(schema);
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Vulnerability))
                    {
                        var vulnerabilities = GetVulnerabilities();
                        if (vulnerabilities?.Any() ?? false)
                        {
                            foreach (var vulnerability in vulnerabilities)
                            {
                                vulnerability.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.VulnerabilityMitigation))
                    {
                        var vulnerabilities = GetVulnerabilities();
                        if (vulnerabilities?.Any() ?? false)
                        {
                            foreach (var vulnerability in vulnerabilities)
                            {
                                var vms = vulnerability.Mitigations?.ToArray();
                                if (vms?.Any() ?? false)
                                {
                                    foreach (var vm in vms)
                                    {
                                        vm.Unapply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Mitigation))
                    {
                        var list = _mitigations?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Diagram))
                    {
                        var list = _diagrams?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.EntityShape))
                    {
                        var diagrams = _diagrams?.ToArray();
                        if (diagrams?.Any() ?? false)
                        {
                            foreach (var diagram in diagrams)
                            {
                                var shapes = diagram.Entities?.ToArray();
                                if (shapes?.Any() ?? false)
                                {
                                    foreach (var shape in shapes)
                                    {
                                        shape.Unapply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.GroupShape))
                    {
                        var diagrams = _diagrams?.ToArray();
                        if (diagrams?.Any() ?? false)
                        {
                            foreach (var diagram in diagrams)
                            {
                                var shapes = diagram.Groups?.ToArray();
                                if (shapes?.Any() ?? false)
                                {
                                    foreach (var shape in shapes)
                                    {
                                        shape.Unapply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Link))
                    {
                        var diagrams = _diagrams?.ToArray();
                        if (diagrams?.Any() ?? false)
                        {
                            foreach (var diagram in diagrams)
                            {
                                var links = diagram.Links?.ToArray();
                                if (links?.Any() ?? false)
                                {
                                    foreach (var link in links)
                                    {
                                        link.Unapply(schema);
                                    }
                                }
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatModel))
                    {
                        this.Unapply(schema);
                    }

                    if (schema.AppliesTo.HasFlag(Scope.ThreatActor))
                    {
                        var list = _actors?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.EntityTemplate))
                    {
                        var list = _entityTemplates?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.FlowTemplate))
                    {
                        var list = _flowTemplates?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.TrustBoundaryTemplate))
                    {
                        var list = _trustBoundaryTemplates?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.LogicalGroup))
                    {
                        // TODO: Expand when the concept of Logical Group will be introduced.
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Severity))
                    {
                        var list = _severities?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    if (schema.AppliesTo.HasFlag(Scope.Strength))
                    {
                        var list = _strengths?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var current in list)
                            {
                                current?.Unapply(schema);
                            }
                        }
                    }

                    scope?.Complete();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void UnapplySchema<T>([NotNull] IPropertySchema schema) where T : IEntity
        {
            var list = _entities?.Where(x => x is T).ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var current in list)
                {
                    current?.Unapply(schema);
                }
            }

            IEnumerable<IEntityTemplate> templates = null;
            if (typeof(T) == typeof(IExternalInteractor))
                templates = _entityTemplates?.Where(x => x.EntityType == EntityType.ExternalInteractor).ToArray();
            else if (typeof(T) == typeof(IProcess))
                templates = _entityTemplates?.Where(x => x.EntityType == EntityType.Process).ToArray();
            else if (typeof(T) == typeof(IDataStore))
                templates = _entityTemplates?.Where(x => x.EntityType == EntityType.DataStore).ToArray();
            if (templates?.Any() ?? false)
            {
                foreach (var current in templates)
                {
                    current?.Unapply(schema);
                }
            }
        }

        [InitializationRequired(false)]
        public bool AutoApplySchemas(IPropertiesContainer container)
        {
            bool result = false;
            Scope scope = Scope.Undefined;
            if (container is IExternalInteractor)
                scope = Scope.ExternalInteractor;
            else if (container is IProcess)
                scope = Scope.Process;
            else if (container is IDataStore)
                scope = Scope.DataStore;
            else if (container is IDataFlow)
                scope = Scope.DataFlow;
            else if (container is ITrustBoundary)
                scope = Scope.TrustBoundary;
            else if (container is IThreatType)
                scope = Scope.ThreatType;
            else if (container is IThreatEvent)
                scope = Scope.ThreatEvent;
            else if (container is IThreatEventScenario)
                scope = Scope.ThreatEventScenario;
            else if (container is IMitigation)
                scope = Scope.Mitigation;
            else if (container is IDiagram)
                scope = Scope.Diagram;
            else if (container is IEntityTemplate template)
            {
                switch (template.EntityType)
                {
                    case EntityType.ExternalInteractor:
                        scope = Scope.ExternalInteractor;
                        break;
                    case EntityType.Process:
                        scope = Scope.Process;
                        break;
                    case EntityType.DataStore:
                        scope = Scope.DataStore;
                        break;
                }
            }
            else if (container is ILink)
                scope = Scope.Link;
            else if (container is IEntityShape)
                scope = Scope.EntityShape;
            else if (container is IGroupShape)
                scope = Scope.GroupShape;
            else if (container is IThreatModel)
                scope = Scope.ThreatModel;
            else if (container is ISeverity)
                scope = Scope.Severity;
            else if (container is IStrength)
                scope = Scope.Strength;
            else if (container is IThreatActor)
                scope = Scope.ThreatActor;
            else if (container is IThreatEventMitigation)
                scope = Scope.ThreatEventMitigation;
            else if (container is IThreatTypeMitigation)
                scope = Scope.ThreatTypeMitigation;
            else if (container is IThreatTypeWeakness)
                scope = Scope.ThreatTypeWeakness;
            else if (container is IVulnerability)
                scope = Scope.Vulnerability;
            else if (container is IVulnerabilityMitigation)
                scope = Scope.VulnerabilityMitigation;
            else if (container is IWeakness)
                scope = Scope.Weakness;
            else if (container is IWeaknessMitigation)
                scope = Scope.WeaknessMitigation;

            if (scope != Scope.Undefined)
            {
                using (var s = UndoRedoManager.OpenScope("Auto Apply Schemas"))
                {
                    var schemas = _schemas?.Where(x => x.AutoApply && x.AppliesTo.HasFlag(scope)).OrderBy(x => x.Priority).ToArray();

                    if (schemas?.Any() ?? false)
                    {
                        foreach (var schema in schemas)
                        {
                            container?.Apply(schema);
                        }

                        s?.Complete();
                        result = true;
                    }
                }
            }

            return result;
        }

        [InitializationRequired]
        public void Add([NotNull] IPropertySchema propertySchema)
        {
            if (propertySchema is PropertySchema ps)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Property Schema"))
                {
                    if (_schemas == null)
                        _schemas = new AdvisableCollection<PropertySchema>();

                    UndoRedoManager.Attach(ps, this);
                    _schemas.Add(ps);
                    scope?.Complete();
                }
            }
            else
                throw new ArgumentException(nameof(propertySchema));
        }

        [InitializationRequired]
        public IPropertySchema AddSchema([Required] string name, [Required] string nspace)
        {
            IPropertySchema result = null;

            using (var scope = UndoRedoManager.OpenScope("Add Schema"))
            {
                if (GetSchema(name, nspace) == null)
                {
                    result = new PropertySchema(name, nspace);
                    Add(result);
                    RegisterEvents(result);
                    scope?.Complete();
                }
            }

            if (result != null)
                ChildCreated?.Invoke(result);

            return result;
        }

        [InitializationRequired]
        public bool RemoveSchema([Required] string name, [Required] string nspace, bool force = false)
        {
            bool result = false;

            var schema = GetSchema(name, nspace);
            if (schema != null)
            {
                result = RemoveSchema(schema, force);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveSchema(Guid schemaId, bool force = false)
        {
            bool result = false;

            var schema = GetSchema(schemaId);
            if (schema != null)
            {
                result = RemoveSchema(schema, force);
            }

            return result;
        }

        private bool RemoveSchema([NotNull] IPropertySchema schema, bool force)
        {
            bool result = false;

            if (schema is PropertySchema s && (force || !IsUsed(s)))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Property Schema"))
                {
                    RemoveRelated(s);

                    result = _schemas.Remove(s);
                    if (result)
                    {
                        UndoRedoManager.Detach(s);
                        UnregisterEvents(s);
                    }

                    scope?.Complete();
                }
                
                if (result)
                    ChildRemoved?.Invoke(s);
            }

            return result;
        }

        public bool IsSchemaUsed(Guid schemaId)
        {
            bool result = false;

            var schema = GetSchema(schemaId);
            if (schema != null)
                result = IsUsed(schema);

            return result;
        }

        private bool IsUsed([NotNull] IPropertySchema propertySchema)
        {
            return (_entities?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_entities?.Any(x => IsUsedForTEC(propertySchema, x)) ?? false) ||
                   (_entities?.Any(x => IsUsedForVC(propertySchema, x)) ?? false) ||
                   (_flows?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_flows?.Any(x => IsUsedForTEC(propertySchema, x)) ?? false) ||
                   (_flows?.Any(x => IsUsedForVC(propertySchema, x)) ?? false) ||
                   (_diagrams?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_diagrams?.Any(x => x.Entities?.Any(y => y.Properties?
                        .Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false) ||
                   (_diagrams?.Any(x => x.Groups?.Any(y => y.Properties?
                        .Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false) ||
                   (_diagrams?.Any(x => x.Links?.Any(y => y.Properties?
                        .Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false) ||
                   (_groups?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ||
                   IsUsedForTEC(propertySchema, this) ||
                   IsUsedForVC(propertySchema, this) ||
                   (_severities?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_strengths?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_mitigations?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_actors?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_entityTemplates?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_trustBoundaryTemplates?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (_flowTemplates?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   IsUsedForTC(propertySchema, this) ||
                   IsUsedForWC(propertySchema, this);
        }

        private bool IsUsedForTEC([NotNull] IPropertySchema propertySchema, IThreatEventsContainer container)
        {
            return (container.ThreatEvents?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (container.ThreatEvents?.Any(x => x.Scenarios?.Any(y => y.Properties?.Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false) ||
                   (container.ThreatEvents?.Any(x => x.Mitigations?.Any(y => y.Properties?.Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false) ||
                   (container.ThreatEvents?.Any(x => IsUsedForVC(propertySchema, x)) ?? false);
        }

        private bool IsUsedForVC([NotNull] IPropertySchema propertySchema, IVulnerabilitiesContainer container)
        {
            return (container.Vulnerabilities?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (container.Vulnerabilities?.Any(x => x.Mitigations?.Any(y => y.Properties?.Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false);
        }

        private bool IsUsedForTC([NotNull] IPropertySchema propertySchema, IThreatTypesContainer container)
        {
            return (container.ThreatTypes?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (container.ThreatTypes?.Any(x => x.Mitigations?.Any(y => y.Properties?.Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false) ||
                   (container.ThreatTypes?.Any(x => x.Weaknesses?.Any(y => y.Properties?.Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false);
        }

        private bool IsUsedForWC([NotNull] IPropertySchema propertySchema, IWeaknessesContainer container)
        {
            return (container.Weaknesses?.Any(x => x.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ||
                   (container.Weaknesses?.Any(x => x.Mitigations?.Any(y => y.Properties?.Any(z => (z.PropertyType?.SchemaId ?? Guid.Empty) == propertySchema.Id) ?? false) ?? false) ?? false);
        }

        private void RemoveRelated([NotNull] IPropertySchema propertySchema)
        {
            RemoveRelated(propertySchema, this);
            RemoveRelated(propertySchema, _entities);
            RemoveRelated(propertySchema, _flows);
            RemoveRelated(propertySchema, _groups);
            RemoveRelated(propertySchema, _diagrams);
            RemoveRelated(propertySchema, _severities);
            RemoveRelated(propertySchema, _strengths);
            RemoveRelated(propertySchema, _mitigations);
            RemoveRelated(propertySchema, _actors);
            RemoveRelated(propertySchema, _threatTypes);
            RemoveRelated(propertySchema, _weaknesses);
            RemoveRelated(propertySchema, _entityTemplates);
            RemoveRelated(propertySchema, _flowTemplates);
            RemoveRelated(propertySchema, _trustBoundaryTemplates);
        }

        private void RemoveRelated([NotNull] IPropertySchema schema, IEnumerable<IPropertiesContainer> containers)
        {
            var array = containers?.ToArray();
            if (array?.Any() ?? false)
            {
                foreach (var item in array)
                {
                    RemoveRelated(schema, item);
                }
            }
        }

        private void RemoveRelated([NotNull] IPropertySchema schema, IPropertiesContainer container)
        {
            if (container != null)
            {
                var properties = container.Properties?.Where(x => (x.PropertyType?.SchemaId ?? Guid.Empty) == schema.Id).ToArray();
                if (properties?.Any() ?? false)
                {
                    foreach (var property in properties)
                    {
                        container.RemoveProperty(property.PropertyType);
                    }
                }

                if (container is IDiagram diagram)
                {
                    RemoveRelated(schema, diagram.Entities);
                    RemoveRelated(schema, diagram.Groups);
                    RemoveRelated(schema, diagram.Links);
                }

                if (container is IThreatEventsContainer teContainer)
                {
                    RemoveRelated(schema, teContainer.ThreatEvents);
                }

                if (container is IThreatEvent threatEvent)
                {
                    RemoveRelated(schema, threatEvent.Mitigations);
                    RemoveRelated(schema, threatEvent.Scenarios);
                }

                if (container is IVulnerabilitiesContainer vContainer)
                {
                    RemoveRelated(schema, vContainer.Vulnerabilities);
                }

                if (container is IThreatType threatType)
                {
                    RemoveRelated(schema, threatType.Mitigations);
                    RemoveRelated(schema, threatType.Weaknesses);
                }

                if (container is IVulnerability vulnerability)
                {
                    RemoveRelated(schema, vulnerability.Mitigations);
                }

                if (container is IWeakness weakness)
                {
                    RemoveRelated(schema, weakness.Mitigations);
                }

                if (container is IWeaknessesContainer wContainer)
                {
                    RemoveRelated(schema, wContainer.Weaknesses);
                }
            }
        }

        [InitializationRequired]
        public IPropertyType GetPropertyType(Guid propertyTypeId)
        {
            return _schemas?
                .FirstOrDefault(x => x.PropertyTypes?.Any(y => y.Id == propertyTypeId) ?? false)?.PropertyTypes
                .FirstOrDefault(x => x.Id == propertyTypeId);
        }
    }
}