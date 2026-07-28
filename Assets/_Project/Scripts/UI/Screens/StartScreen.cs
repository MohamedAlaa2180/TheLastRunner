using System;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour, IUIScreen
{
    [SerializeField] Button startButton;

    public UIScreenId Id => UIScreenId.Start;
    public bool IsVisible => gameObject.activeSelf;
    public event Action StartClicked;

    void Awake()
    {
        if (startButton != null)
            startButton.onClick.AddListener(() => StartClicked?.Invoke());
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
