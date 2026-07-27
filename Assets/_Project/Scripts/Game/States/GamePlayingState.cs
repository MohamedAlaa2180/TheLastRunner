using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayingState : IState
{
    readonly GameStateMachine machine;

    public GamePlayingState(GameStateMachine machine)
    {
        this.machine = machine;
    }

    public void Enter() => Time.timeScale = 1f;

    public void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            machine.Pause();
    }

    public void Exit() { }
}
