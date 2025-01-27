﻿using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    [Extension("AEF71689-3A5D-4355-AE2E-C14DBC47C8DB", "Asset Marker Factory", 100, ExecutionMode.Simplified)]
    public class AssetMarkerProviderFactory : IMarkerProviderFactory
    {
        public string Name => "Asset";

        public Scope ContextScope => Scope.Undefined;

        public IMarkerProvider Create(object item)
        {
            IMarkerProvider result = null;

            if (item is IEntity entity)
                result = new AssetMarkerProvider(entity);

            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
