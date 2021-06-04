﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Mitre.Capec;
using ThreatsManager.Utilities;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AttackPatternNode : Node
    {
        public AttackPatternNode()
        {

        }
        
        internal AttackPatternNode([NotNull] MitreGraph graph, [NotNull] AttackPatternType attackPattern) : base(graph, "CAPEC", attackPattern.ID)
        {
            if (attackPattern.Status == StatusEnumeration.Deprecated || attackPattern.Status == StatusEnumeration.Obsolete)
                throw new ArgumentException(Properties.Resources.InvalidStatus, "attackPattern");

            Name = attackPattern.Name;
            Description = attackPattern.Description.ConvertToString();
            if (Enum.TryParse<Evaluation>(attackPattern.Likelihood_Of_Attack.ToString(), out var likelihood))
                Likelihood = likelihood;
            else
                Likelihood = Evaluation.Unknown;
            if (Enum.TryParse<Evaluation>(attackPattern.Typical_Severity.ToString(), out var severity))
                Severity = severity;
            else
                Severity = Evaluation.Unknown;

            #region Add relationships.
            var relAttackPatterns = attackPattern.Related_Attack_Patterns?.ToArray();
            if (relAttackPatterns?.Any() ?? false)
            {
                foreach (var a in relAttackPatterns)
                {
                    if (Enum.TryParse<RelationshipType>(a.Nature.ToString(), out var relType))
                    {
                        AddRelationship(relType, "CAPEC", a.CAPEC_ID);
                    }
                }
            }

            var relWeaknesses = attackPattern.Related_Weaknesses?.ToArray();
            if (relWeaknesses?.Any() ?? false)
            {
                foreach (var w in relWeaknesses)
                {
                    AddRelationship(RelationshipType.Leverages, "CWE", w.CWE_ID);
                }
            }
            #endregion

            #region Add Consequences.
            var consequences = attackPattern.Consequences?.ToArray();
            if (consequences?.Any() ?? false)
            {
                foreach (var consequence in consequences)
                {
                    AddConsequence(consequence);
                }
            }
            #endregion

            #region Add Potential Mitigations.
            var potentialMitigations = attackPattern.Mitigations?.ToArray();
            if (potentialMitigations?.Any() ?? false)
            {
                if (PotentialMitigations == null)
                    PotentialMitigations = new List<PotentialMitigation>();

                foreach (var potentialMitigation in potentialMitigations)
                {
                    PotentialMitigations.Add(new PotentialMitigation(null, null, 
                        potentialMitigation.ConvertToString(), null));
                }
            }
            #endregion

            #region Add Taxonomy Mappings.
            var taxonomyMappings = attackPattern.Taxonomy_Mappings?.ToArray();
            if (taxonomyMappings?.Any() ?? false)
            {
                foreach (var taxonomyMapping in taxonomyMappings)
                {
                    AddTaxonomyMapping(taxonomyMapping);
                }
            }
            #endregion
        }

        [JsonProperty("likelihood")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Evaluation Likelihood { get; private set; }

        [JsonProperty("severity")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Evaluation Severity { get; private set; }

        [JsonProperty("consequences")]
        public List<Consequence> Consequences { get; private set; }

        [JsonProperty("mitigations")]
        public List<PotentialMitigation> PotentialMitigations { get; private set; }

        [JsonProperty("taxonomyMappings")]
        public List<TaxonomyMapping> TaxonomyMappings { get; private set; }

        #region Private Member Functions.
        private void AddConsequence(ConsequencesTypeConsequence consequence)
        {
            if (consequence != null)
            {
                IEnumerable<string> scopes = null;
                IEnumerable<string> impacts = null;
                Evaluation likelihood = Evaluation.Unknown;

                var s = consequence.Scope?.ToArray();
                if (s?.Any() ?? false)
                {
                    scopes = s.Select(x => x.GetXmlEnumLabel());
                }

                var i = consequence.Impact?.ToArray();
                if (i?.Any() ?? false)
                {
                    impacts = i.Select(x => x.GetXmlEnumLabel());
                }

                if (Enum.TryParse<Evaluation>(consequence.Likelihood.ToString(), out var l))
                {
                    likelihood = l;
                }

                var notes = consequence.Note?.ConvertToString();

                if (Consequences == null)
                    Consequences = new List<Consequence>();

                Consequences.Add(new Consequence(scopes, impacts, likelihood, notes));
            }
        }

        private void AddTaxonomyMapping(TaxonomyMappingsTypeTaxonomy_Mapping taxonomyMapping)
        {
            if (taxonomyMapping != null)
            {
                if (TaxonomyMappings == null)
                    TaxonomyMappings = new List<TaxonomyMapping>();

                TaxonomyMappings.Add(new TaxonomyMapping(taxonomyMapping.Taxonomy_Name.GetXmlEnumLabel(),
                    taxonomyMapping.Entry_ID, taxonomyMapping.Entry_Name, taxonomyMapping.Mapping_Fit == TaxonomyMappingFitEnumeration.Exact));
            }
        }
        #endregion
    }
}
