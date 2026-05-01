using UnityEngine;

public class ChaseState : IStates
{
    EnemyBehavior enemy;

    public ChaseState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }
    public void Enter() { }
    public void Update()
    {
        enemy.FollowPlayer();
    }
    public void Exit() { }
}
