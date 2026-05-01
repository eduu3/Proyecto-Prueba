using UnityEngine;

public class StateMachine 
{
    IStates currentState;
    public IStates CurrentState => currentState;
    public void ChangeState(IStates newState)
    {
        if (currentState == newState) return;
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    public void Update()
    {
        currentState.Update();
    }
}
