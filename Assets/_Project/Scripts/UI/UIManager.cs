using System;
using System.Collections.Generic;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;

public class UIManager : MonoBehaviour, IUIService
{
    [SerializeField] StartScreen startScreen;
    [SerializeField] PauseMenuScreen pauseMenuScreen;
    [SerializeField] CountdownScreen countdownScreen;
    [SerializeField] CollisionScreen collisionScreen;

    [Inject] IEventBus eventBus;

    readonly Dictionary<UIScreenId, IUIScreen> screens = new();
    IDisposable pausedSub;
    IDisposable resumedSub;
    IDisposable startedSub;
    IDisposable countdownStartedSub;
    IDisposable countdownTickSub;
    IDisposable hitSub;
    IDisposable livesChangedSub;

    void Awake()
    {
        Register(startScreen);
        Register(pauseMenuScreen);
        Register(countdownScreen);
        Register(collisionScreen);
    }

    void OnEnable()
    {
        if (startScreen != null)
            startScreen.StartClicked += OnStartClicked;
        if (pauseMenuScreen != null)
            pauseMenuScreen.ResumeClicked += OnResumeClicked;
        if (collisionScreen != null)
        {
            collisionScreen.RestartClicked += OnRestartClicked;
            collisionScreen.UseLifeClicked += OnUseLifeClicked;
        }

        pausedSub = eventBus.Subscribe<GamePausedEvent>(_ => Show(UIScreenId.Pause));
        resumedSub = eventBus.Subscribe<GameResumedEvent>(_ =>
        {
            Hide(UIScreenId.Pause);
            Hide(UIScreenId.Countdown);
        });
        startedSub = eventBus.Subscribe<GameStartedEvent>(_ => Hide(UIScreenId.Countdown));
        countdownStartedSub = eventBus.Subscribe<CountdownStartedEvent>(_ =>
        {
            Hide(UIScreenId.Start);
            Hide(UIScreenId.Collision);
            Show(UIScreenId.Countdown);
        });
        countdownTickSub = eventBus.Subscribe<CountdownTickEvent>(evt => countdownScreen?.SetValue(evt.Value));
        hitSub = eventBus.Subscribe<GameHitEvent>(_ => Show(UIScreenId.Collision));
        livesChangedSub = eventBus.Subscribe<LivesChangedEvent>(evt => collisionScreen?.SetLivesAvailable(evt.Lives > 0));
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
        if (collisionScreen != null)
        {
            collisionScreen.RestartClicked -= OnRestartClicked;
            collisionScreen.UseLifeClicked -= OnUseLifeClicked;
        }

        pausedSub?.Dispose();
        resumedSub?.Dispose();
        startedSub?.Dispose();
        countdownStartedSub?.Dispose();
        countdownTickSub?.Dispose();
        hitSub?.Dispose();
        livesChangedSub?.Dispose();
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

    void OnRestartClicked() => eventBus.Publish(new RestartRequestedEvent());

    void OnUseLifeClicked() => eventBus.Publish(new ContinueRequestedEvent());
}
