﻿using System;
using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.DevOps.Panels.RiskTrend
{
#pragma warning disable CS0067
    public partial class RiskTrendPanelFactory
    {
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory, IIdentity> PanelCreationRequired;
        public event Action<IPanelFactory, IPanel> PanelDeletionRequired;
        public event Action<IPanelFactory, IPanel> PanelShowRequired;
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Home;
        public string Bar => "Overview";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "CreatePanel", "Risk Trend", Properties.Resources.chart_line_big,
                Properties.Resources.chart_line)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList([NotNull] IThreatModel model)
        {
            return null;
        }

        [InitializationRequired]
        public void ExecuteRibbonAction(IThreatModel threatModel, [NotNull] IActionDefinition action)
        {
            switch (action.Name)
            {
                case "CreatePanel":
                    PanelCreationRequired?.Invoke(this, action.Tag as IIdentity);
                    break;
            }
        }
    }
}