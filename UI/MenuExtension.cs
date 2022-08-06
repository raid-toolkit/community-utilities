using Raid.Toolkit.Common;
using Raid.Toolkit.Extensibility;
using System;
using System.Windows.Forms;

namespace Raid.Toolkit.Community.Extensibility.Utilities.UI
{
    public class MenuExtension<T> : IDisposable where T : Form
    {
        private bool IsDisposed;
        private bool IsShowing = false;
        private readonly IExtensionHost Host;
        private readonly DisposableCollection Disposables = new();

        public MenuExtension(IExtensionHost host, string menuItemName, WindowOptions options = null)
        {
            Host = host;
            Host.RegisterWindow<T>(options ?? new WindowOptions() { RememberPosition = true, RememberVisibility = true });
            MenuEntry menuEntry = new() { DisplayName = menuItemName, IsEnabled = true, IsVisible = true };
            menuEntry.Activate += OnShowUI;
            Disposables.Add(host.RegisterMenuEntry(menuEntry));
        }

        private void OnShowUI(object sender, EventArgs e)
        {
            Show();
        }

        public Form Show()
        {
            if (IsShowing) return null;
            T dialog = Host.CreateWindow<T>();
            dialog.FormClosed += Dialog_FormClosed;
            dialog.Show();
            IsShowing = true;
            return dialog;
        }

        private void Dialog_FormClosed(object sender, FormClosedEventArgs e)
        {
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
