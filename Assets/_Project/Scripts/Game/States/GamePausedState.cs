using UnityEngine;
using UnityEngine.InputSystem;

public class GamePausedState : IState
{
    readonly GameStateMachine machine;

    public GamePausedState(GameStateMachine machine)
    {
        this.machine = machine;
    }

    public void Enter() => Time.timeScale = 0f;

    public void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            machine.Resume();
    }

    public void Exit() { }
}
