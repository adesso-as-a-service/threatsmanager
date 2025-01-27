﻿using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface that defines a Threat Type, that is the definition of a Threat from which Threat Events can be derived.
    /// </summary>
    public interface IThreatType : IIdentity, IThreatModelChild, IPropertiesContainer, 
        IThreatTypeMitigationsContainer, IThreatTypeWeaknessesContainer, ISourceInfo
    {
        /// <summary>
        /// Identifier of the Severity.
        /// </summary>
        int SeverityId { get; }

        /// <summary>
        /// Severity of the Threat Type.
        /// </summary>
        ISeverity Severity { get; set; }

        /// <summary>
        /// Generates a Threat Event from the Threat Type.
        /// </summary>
        /// <returns>Threat Event generated from the current Threat Type.</returns>
        IThreatEvent GenerateEvent();

        /// <summary>
        /// Get the Mitigation Level for the Threat.
        /// </summary>
        /// <returns>Evaluation of the Mitigation Level.</returns>
        MitigationLevel GetMitigationLevel();

        /// <summary>
        /// Creates a duplicate of the current Threat Type and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Threat Type.</returns>
        IThreatType Clone(IThreatTypesContainer container);
    }
}