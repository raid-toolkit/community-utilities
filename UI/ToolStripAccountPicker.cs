using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Raid.Toolkit.Community.Extensibility.Utilities.UI
{
    [DefaultProperty("GameInstanceManager")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
    public class ToolStripAccountPicker<T, U> : ToolStripComboBox where U : ModelScopeBase where T : GameModelInstance<U>
    {
        public event EventHandler SelectedValueChanged;

        private GameModelInstanceList<T, U> m_gameInstances;
        public ToolStripAccountPicker()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox.DisplayMember = "Name";
            ComboBox.SelectedValueChanged += ComboBox_SelectedValueChanged;
        }

        [Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
        public T SelectedInstance => SelectedItem as T;

        [Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
        public GameModelInstanceList<T, U> GameInstanceList
        {
            get { return m_gameInstances; }
            set
            {
                if (m_gameInstances == value)
                    return;

                m_gameInstances?.Dispose();
                m_gameInstances = value;

                BindingSource bindingSource = new() { DataSource = m_gameInstances };
                ComboBox.DataSource = bindingSource;
            }
        }

        private void ComboBox_SelectedValueChanged(object sender, System.EventArgs e)
        {
            SelectedValueChanged?.Invoke(this, new());
        }
    }
}
