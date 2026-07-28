using Reflex.Core;
using UnityEngine;

public class UIInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] UIManager uiManager;

    public void InstallBindings(ContainerBuilder builder)
    {
        if (uiManager != null)
            builder.RegisterValue(uiManager, new[] { typeof(IUIService) });
    }
}
