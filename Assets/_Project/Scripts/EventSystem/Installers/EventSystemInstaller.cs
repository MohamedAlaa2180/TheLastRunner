using Genesis.Core.Events;
using Reflex.Core;
using UnityEngine;

namespace Genesis.Core.Installers
{
    public class EventSystemInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(new EventBus(), new[] { typeof(IEventBus) });
        }
    }
}