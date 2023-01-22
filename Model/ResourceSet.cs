using Client.ViewModel.Contextes.RegionDialog;
using SharedModel.Meta.Account;

using System.Collections.Generic;

namespace Raid.Toolkit.Community.Model
{
    public class ResourceSet
    {
        public IReadOnlyDictionary<ResourceTypeId, double> Resources { get; } = new Dictionary<ResourceTypeId, double>();

        public ResourceSet(StagePrice price)
        {
            Resources = new Dictionary<ResourceTypeId, double>() {
                {ResourceTypeId.Energy, price.Energy },
                {ResourceTypeId.DoubleAscend_FortressKey, price.FortressKeys },
            };
        }

        public ResourceSet(IReadOnlyDictionary<ResourceTypeId, double> resources)
        {
            Resources = resources;
        }

        public ResourceSet(ResourceSet resources)
        {
            Resources = new Dictionary<ResourceTypeId, double>(resources.Resources);
        }

        public static ResourceSet operator -(ResourceSet a, ResourceSet b)
        {
            Dictionary<ResourceTypeId, double> result = new(a.Resources);
            foreach (var (resourceType, y) in b.Resources)
            {
                result.TryGetValue(resourceType, out double x);
                result[resourceType] -= x;
            }
            return new(result);
        }

        public static ResourceSet operator /(ResourceSet a, double divisor)
        {
            Dictionary<ResourceTypeId, double> result = new(a.Resources);
            foreach (var (resourceType, x) in a.Resources)
                result[resourceType] = x / divisor;
            return new(result);
        }

        public static ResourceSet operator *(ResourceSet a, double divisor)
        {
            Dictionary<ResourceTypeId, double> result = new(a.Resources);
            foreach (var (resourceType, x) in a.Resources)
                result[resourceType] = x * divisor;
            return new(result);
        }

        public static ResourceSet operator +(ResourceSet a, ResourceSet b)
        {
            Dictionary<ResourceTypeId, double> result = new(a.Resources);
            foreach (var (resourceType, y) in b.Resources)
            {
                result.TryGetValue(resourceType, out double x);
                result[resourceType] += x;
            }
            return new(result);
        }

        public static bool operator <(ResourceSet a, ResourceSet b)
        {
            foreach (var (resourceType, y) in b.Resources)
            {
                a.Resources.TryGetValue(resourceType, out double x);
                if (x >= y) return false;
            }
            return true;
        }

        public static bool operator <=(ResourceSet a, ResourceSet b)
        {
            foreach (var (resourceType, y) in b.Resources)
            {
                a.Resources.TryGetValue(resourceType, out double x);
                if (x > y) return false;
            }
            return true;
        }

        public static bool operator >(ResourceSet a, ResourceSet b)
        {
            foreach (var (resourceType, x) in a.Resources)
            {
                b.Resources.TryGetValue(resourceType, out double y);
                if (x <= y) return false;
            }
            return true;
        }

        public static bool operator >=(ResourceSet a, ResourceSet b)
        {
            foreach (var (resourceType, x) in a.Resources)
            {
                b.Resources.TryGetValue(resourceType, out double y);
                if (x < y) return false;
            }
            return true;
        }
    }
}
