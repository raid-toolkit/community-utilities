using System.Collections.Generic;
using System.Linq;
using Client.MVVM.Base.ViewModel.Contexts;
using Client.RaidApp;
using Client.ViewModel.DTO;

namespace Raid.Toolkit.Community.Extensibility.ViewModel
{
    public class ViewCache
    {
        private readonly Dictionary<ViewKey, List<IContext>> Views;
        public ViewCache(RaidViewMaster viewMaster)
        {
            Views = viewMaster._views.GroupBy(viewMeta => viewMeta.Key, viewMeta => viewMeta.View.Context).ToDictionary(kvp => kvp.Key, kvp => kvp.ToList());
        }

        public bool TryGetViewContext<T>(ViewKey viewKey, out T? viewContext) where T : class, IContext
        {
            if (!Views.TryGetValue(viewKey, out List<IContext>? contexts))
            {
                viewContext = null;
                return false;
            }
            viewContext = contexts.OfType<T>().FirstOrDefault();
            return viewContext != null;
        }
    }
}