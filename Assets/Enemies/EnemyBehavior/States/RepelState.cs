using UnityEngine;
using System.Collections;

public class RepelState : IStates
{
    EnemyBehavior enemy;

    public RepelState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }
    public void Enter() 
    {
        enemy.StartRepelTime();
    }
    public void Update()
    {
        enemy.MoveBack();
        enemy.ChangeColorToYellow();
    }
    public void Exit() 
    { 
        enemy.EndRepealTime();
    }
}
