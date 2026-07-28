using System;
using System.Collections.Generic;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;

public class UIManager : MonoBehaviour, IUIService
{
    [SerializeField] StartScreen startScreen;
    [SerializeField] PauseMenuScreen pauseMenuScreen;

    [Inject] IEventBus eventBus;

    readonly Dictionary<UIScreenId, IUIScreen> screens = new();
    IDisposable pausedSub;
    IDisposable resumedSub;
    IDisposable startedSub;

    void Awake()
    {
        Register(startScreen);
        Register(pauseMenuScreen);
    }

    void OnEnable()
    {
        if (startScreen != null)
            startScreen.StartClicked += OnStartClicked;
        if (pauseMenuScreen != null)
            pauseMenuScreen.ResumeClicked += OnResumeClicked;

        pausedSub = eventBus.Subscribe<GamePausedEvent>(_ => Show(UIScreenId.Pause));
        resumedSub = eventBus.Subscribe<GameResumedEvent>(_ => Hide(UIScreenId.Pause));
        startedSub = eventBus.Subscribe<GameStartedEvent>(_ => Hide(UIScreenId.Start));
    }

    void Start()
    {
        HideAll();
        Show(UIScreenId.Start);
    }

    void OnDisable()
    {
        if (startScreen != null)
            startScreen.StartClicked -= OnStartClicked;
        if (pauseMenuScreen != null)
            pauseMenuScreen.ResumeClicked -= OnResumeClicked;

        pausedSub?.Dispose();
        resumedSub?.Dispose();
        startedSub?.Dispose();
    }

    public void Show(UIScreenId id)
    {
        if (screens.TryGetValue(id, out var screen))
            screen.Show();
    }

    public void Hide(UIScreenId id)
    {
        if (screens.TryGetValue(id, out var screen))
            screen.Hide();
    }

    public void HideAll()
    {
        foreach (var screen in screens.Values)
            screen.Hide();
    }

    public bool IsVisible(UIScreenId id) =>
        screens.TryGetValue(id, out var screen) && screen.IsVisible;

    void Register(IUIScreen screen)
    {
        if (screen == null) return;
        screens[screen.Id] = screen;
    }

    void OnStartClicked() => eventBus.Publish(new StartGameRequestedEvent());

    void OnResumeClicked() => eventBus.Publish(new ResumeGameRequestedEvent());
}
