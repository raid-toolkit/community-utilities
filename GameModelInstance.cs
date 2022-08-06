using Microsoft.Extensions.DependencyInjection;
using Raid.Toolkit.Extensibility;
using System;

namespace Raid.Toolkit.Community.Extensibility.Utilities
{
    public class GameModelInstance<T> where T : ModelScopeBase
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public IGameInstance GameInstance { get; }
        private readonly IServiceProvider ServiceProvider;

        public T GetModelScope()
        {
            return ActivatorUtilities.CreateInstance<T>(ServiceProvider, GameInstance.Runtime);
        }

        public override string ToString()
        {
            return Name;
        }

        public GameModelInstance(IServiceProvider serviceProvider, IGameInstance gameInstance)
        {
            ServiceProvider = serviceProvider;
            GameInstance = gameInstance;
            Id = gameInstance.Id;
            Name = GetModelScope().AppModel._userWrapper.UserGameSettings.GameSettings.Name;
        }
    }
}
