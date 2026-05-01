using UnityEngine;
using System.Collections;

public class SpawnState : IStates
{
    EnemyBehavior enemy;

    public SpawnState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.LaunchEnemy();
    }
    public void Update() { }
    public void Exit() 
    {
        enemy.EndSpawnTime();   
    }
}
