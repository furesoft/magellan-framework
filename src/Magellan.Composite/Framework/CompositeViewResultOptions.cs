using Magellan.Framework;
using Microsoft.Practices.Prism.Regions;


namespace Magellan.Composite.Framework
{
    internal static class CompositeViewResultOptions
    {
        public static string GetRegionName(this ViewResultOptions options)
        {
            return options.GetOrDefault<string>("RegionName");
        }

        public static void SetRegionName(this ViewResultOptions options, string regionName)
        {
            options["RegionName"] = regionName;
        }

        public static IRegionManager GetRegionManager(this ViewResultOptions options)
        {
            return options.GetOrDefault<IRegionManager>("RegionManager");
        }

        public static void SetRegionManager(this ViewResultOptions options, IRegionManager regionManager)
        {
            options["RegionManager"] = regionManager;
        }

        public static IRegion GetRegion(this ViewResultOptions options)
        {
            return options.GetOrDefault<IRegion>("Region");
        }

        public static void SetRegion(this ViewResultOptions options, IRegion region)
        {
            options["Region"] = region;
        }
    }
}
