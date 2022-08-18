using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Raid.Toolkit.Extensibility;

namespace Raid.Toolkit.Community.Extensibility.Utilities
{
    public class AsyncGameModelInstanceList<T, U> : AsyncObservableCollection<T>, IDisposable where T : GameModelInstance<U> where U : ModelScopeBase
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IGameInstanceManager InstanceManager;
        private bool IsDisposed;

        public IList<T> Instances => Items;

        public AsyncGameModelInstanceList(IServiceProvider serviceProvider, IGameInstanceManager instanceManager)
            : base(instanceManager.Instances.Select(instance => ActivatorUtilities.CreateInstance<T>(serviceProvider, instance)).ToList())
        {
            ServiceProvider = serviceProvider;
            InstanceManager = instanceManager;
            instanceManager.OnRemoved += InstanceManager_OnRemoved;
            instanceManager.OnAdded += InstanceManager_OnAdded;
        }

        private void InstanceManager_OnRemoved(object sender, IGameInstanceManager.GameInstancesUpdatedEventArgs e)
        {
            T[] itemsToRemove = Items.Where(item => item.GameInstance.Id == e.Instance.Id).ToArray();
            foreach (var itemToRemove in itemsToRemove)
                _ = Remove(itemToRemove);
        }

        private void InstanceManager_OnAdded(object sender, IGameInstanceManager.GameInstancesUpdatedEventArgs e)
        {
            Add(ActivatorUtilities.CreateInstance<T>(ServiceProvider, e.Instance));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    InstanceManager.OnRemoved -= InstanceManager_OnRemoved;
                    InstanceManager.OnAdded -= InstanceManager_OnAdded;
                }
                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}