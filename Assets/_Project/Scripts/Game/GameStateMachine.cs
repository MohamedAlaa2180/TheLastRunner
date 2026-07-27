using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [Inject] IEventBus eventBus;
    public IEventBus EventBus => eventBus;

    public IState CurrentState { get; private set; }

    GamePlayingState playingState;
    GamePausedState pausedState;
    GameOverState gameOverState;

    void Awake()
    {
        playingState = new GamePlayingState(this);
        pausedState = new GamePausedState(this);
        gameOverState = new GameOverState();
    }

    void Start()
    {
        ChangeState(playingState);
        eventBus.Publish(new GameStartedEvent());
    }

    void Update() => CurrentState?.Update();

    void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
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
