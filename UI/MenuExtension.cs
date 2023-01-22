using System;
using Raid.Toolkit.Common;
using Raid.Toolkit.Extensibility;

namespace Raid.Toolkit.Community.Extensibility.Utilities.UI
{
    public class MenuExtension<T> : IDisposable where T : class
    {
        private bool IsDisposed;
        private bool IsShowing;
        private readonly IExtensionHost Host;
        private readonly DisposableCollection Disposables = new();

        public MenuExtension(IExtensionHost host, string menuItemName, WindowOptions options = null)
        {
            Host = host;
            _ = Host.RegisterWindow<T>(options ?? new WindowOptions() { RememberPosition = true, RememberVisibility = true });
            MenuEntry menuEntry = new() { DisplayName = menuItemName, IsEnabled = true, IsVisible = true };
            menuEntry.Activate += OnShowUI;
            _ = Disposables.Add(host.RegisterMenuEntry(menuEntry));
        }

        private void OnShowUI(object sender, EventArgs e)
        {
            _ = Show();
        }

        public IWindowAdapter<T>? Show()
        {
            if (IsShowing) return null;
            IWindowAdapter<T> window = Host.CreateWindow<T>();
            window.Closing += OnWindowClosed;
            window.Show();
            IsShowing = true;
            if (window is IDisposable disposable)
                Disposables.Add(disposable);
            return window;
        }

        private void OnWindowClosed(object? sender, WindowAdapterCloseEventArgs e)
        {
            if (sender is IWindowAdapter adapter)
                adapter.Closing -= OnWindowClosed;

            if (sender is IDisposable disposable)
                Disposables.Remove(disposable);

            IsShowing = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Disposables.Dispose();
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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
