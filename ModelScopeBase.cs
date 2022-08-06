using Client.Model;
using Il2CppToolkit.Runtime;

namespace Raid.Toolkit.Community.Extensibility.Utilities
{
    public abstract class ModelScopeBase
    {
        public Il2CsRuntimeContext Context { get; }

        public ModelScopeBase(Il2CsRuntimeContext context)
        {
            Context = context;
        }

        private AppModel _AppModel;
        public AppModel AppModel
        {
            get
            {
                if (_AppModel == null)
                    _AppModel = Client.App.SingleInstance<AppModel>._instance.GetValue(Context);
                return _AppModel;
            }
        }
    }

    public class AppModelScope : ModelScopeBase
    {
        public AppModelScope(Il2CsRuntimeContext context) : base(context)
        {
        }
    }
}
