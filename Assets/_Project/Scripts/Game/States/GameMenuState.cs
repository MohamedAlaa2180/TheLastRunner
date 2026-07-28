using UnityEngine;

public class GameMenuState : IState
{
    public void Enter() => Time.timeScale = 0f;

    public void Update() { }

    public void Exit() { }
}
