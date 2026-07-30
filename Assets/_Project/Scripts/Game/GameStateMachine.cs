using System;
using System.Collections;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] int countdownSeconds = 3;

    [Inject] IEventBus eventBus;
    public IEventBus EventBus => eventBus;

    public IState CurrentState { get; private set; }

    GameMenuState menuState;
    GamePlayingState playingState;
    GamePausedState pausedState;
    GameOverState gameOverState;
    GameHitState hitState;

    IDisposable startRequestedSub;
    IDisposable resumeRequestedSub;
    IDisposable playerHitSub;
    IDisposable continueRequestedSub;
    IDisposable restartRequestedSub;
    IDisposable livesChangedSub;

    int currentLives = 3;

    void Awake()
    {
        menuState = new GameMenuState();
        playingState = new GamePlayingState(this);
        pausedState = new GamePausedState(this);
        gameOverState = new GameOverState();
        hitState = new GameHitState();
    }

    void OnEnable()
    {
        startRequestedSub = eventBus.Subscribe<StartGameRequestedEvent>(_ => StartCoroutine(CountdownThenStart()));
        resumeRequestedSub = eventBus.Subscribe<ResumeGameRequestedEvent>(_ => Resume());
        playerHitSub = eventBus.Subscribe<PlayerHitObstacleEvent>(_ => Hit());
        continueRequestedSub = eventBus.Subscribe<ContinueRequestedEvent>(_ => StartCoroutine(CountdownThenContinue()));
        restartRequestedSub = eventBus.Subscribe<RestartRequestedEvent>(_ => RestartGame());
        livesChangedSub = eventBus.Subscribe<LivesChangedEvent>(evt => currentLives = evt.Lives);
    }

    void OnDisable()
    {
        startRequestedSub?.Dispose();
        resumeRequestedSub?.Dispose();
        playerHitSub?.Dispose();
        continueRequestedSub?.Dispose();
        restartRequestedSub?.Dispose();
        livesChangedSub?.Dispose();
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

    public void Hit()
    {
        if (CurrentState != playingState) return;
        ChangeState(hitState);
        eventBus.Publish(new GameHitEvent());
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

    void Continue()
    {
        if (CurrentState != hitState) return;
        ChangeState(playingState);
        eventBus.Publish(new GameResumedEvent());
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator CountdownThenStart()
    {
        if (CurrentState != menuState) yield break;
        yield return RunCountdown();
        StartGame();
    }

    IEnumerator CountdownThenContinue()
    {
        if (CurrentState != hitState || currentLives <= 0) yield break;
        yield return RunCountdown();
        Continue();
    }

    IEnumerator RunCountdown()
    {
        eventBus.Publish(new CountdownStartedEvent());
        for (int i = countdownSeconds; i >= 1; i--)
        {
            eventBus.Publish(new CountdownTickEvent(i));
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
