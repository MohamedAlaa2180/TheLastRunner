using System;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [Inject] IEventBus eventBus;
    public IEventBus EventBus => eventBus;

    public IState CurrentState { get; private set; }

    GameMenuState menuState;
    GamePlayingState playingState;
    GamePausedState pausedState;
    GameOverState gameOverState;

    IDisposable startRequestedSub;
    IDisposable resumeRequestedSub;

    void Awake()
    {
        menuState = new GameMenuState();
        playingState = new GamePlayingState(this);
        pausedState = new GamePausedState(this);
        gameOverState = new GameOverState();
    }

    void OnEnable()
    {
        startRequestedSub = eventBus.Subscribe<StartGameRequestedEvent>(_ => StartGame());
        resumeRequestedSub = eventBus.Subscribe<ResumeGameRequestedEvent>(_ => Resume());
    }

    void OnDisable()
    {
        startRequestedSub?.Dispose();
        resumeRequestedSub?.Dispose();
    }

    void Start()
    {
        ChangeState(menuState);
    }

    void Update() => CurrentState?.Update();

    void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void StartGame()
    {
        if (CurrentState != menuState) return;
        ChangeState(playingState);
        eventBus.Publish(new GameStartedEvent());
    }

    public void Pause()
    {
        if (CurrentState != playingState) return;
        ChangeState(pausedState);
        eventBus.Publish(new GamePausedEvent());
    }

    public void Resume()
    {
        if (CurrentState != pausedState) return;
        ChangeState(playingState);
        eventBus.Publish(new GameResumedEvent());
    }

    public void EndGame()
    {
        if (CurrentState == gameOverState) return;
        ChangeState(gameOverState);
        eventBus.Publish(new GameOverEvent());
    }
}
