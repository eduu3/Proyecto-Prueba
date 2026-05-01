using UnityEngine;
using System.Collections;

public class DieState : IStates
{
    EnemyBehavior enemy;

    public DieState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }
    public void Enter() 
    {
        enemy.DisableAnimator();
        enemy.StartDestroying();
        enemy.DieEvent();
    }
    public void Update()
    {
        enemy.ChangeColorTransparency();
    }
    public void Exit() { }
}
