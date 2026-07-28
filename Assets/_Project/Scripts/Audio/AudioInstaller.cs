using Reflex.Core;
using UnityEngine;

public class AudioInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] AudioManager audioManager;

    public void InstallBindings(ContainerBuilder builder)
    {
        if (audioManager != null)
            builder.RegisterValue(audioManager, new[] { typeof(IAudioService) });
    }
}
