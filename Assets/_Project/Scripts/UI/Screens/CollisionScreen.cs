using System;
using UnityEngine;
using UnityEngine.UI;

public class CollisionScreen : MonoBehaviour, IUIScreen
{
    [SerializeField] Button restartButton;
    [SerializeField] Button useLifeButton;

    public UIScreenId Id => UIScreenId.Collision;
    public bool IsVisible => gameObject.activeSelf;
    public event Action RestartClicked;
    public event Action UseLifeClicked;

    void Awake()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(() => RestartClicked?.Invoke());
        if (useLifeButton != null)
            useLifeButton.onClick.AddListener(() => UseLifeClicked?.Invoke());
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void SetLivesAvailable(bool available)
    {
        if (useLifeButton != null)
            useLifeButton.interactable = available;
    }
}
