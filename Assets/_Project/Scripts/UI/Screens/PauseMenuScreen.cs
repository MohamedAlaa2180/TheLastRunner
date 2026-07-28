using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScreen : MonoBehaviour, IUIScreen
{
    [SerializeField] Button resumeButton;

    public UIScreenId Id => UIScreenId.Pause;
    public bool IsVisible => gameObject.activeSelf;
    public event Action ResumeClicked;

    void Awake()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => ResumeClicked?.Invoke());
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
