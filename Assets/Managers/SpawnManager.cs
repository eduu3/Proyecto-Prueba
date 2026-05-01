using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Random = UnityEngine.Random;
public class SpawnManager : MonoBehaviour
{
    public static event Action<int> OnRoundStart;
    [SerializeField] GameObject[] enemiesBoard;
    [SerializeField] int PoolSize = 5;
    [SerializeField] Vector2[] spawnPoints;
    [SerializeField] float roundTimeWaiting = 2f;
    [SerializeField] int actualRound = 1;
    public int enemies = 0;
    List<List<GameObject>> enemyPool;
    Coroutine spawnCoroutine;
    
    void Awake()
    {
        EnemyBehavior.OnEnemyKilled += DecreaseEnemyCount;
        GameManager.OnStart += IniciarJuego;
        enemyPool = new List<List<GameObject>>();
        CreateEnemyPool();
    }
    private void OnDestroy()
    {
        EnemyBehavior.OnEnemyKilled -= DecreaseEnemyCount;
        GameManager.OnStart -= IniciarJuego;
    }
    public void IniciarJuego(GameState currentState) 
    {
        if (!(currentState == GameState.Playing)) return;
        actualRound = 1;
        enemies = 0;

        if (spawnCoroutine != null) 
        { 
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(Spawn());
    }

    void DecreaseEnemyCount()
    {
        enemies--;
    }

    IEnumerator Spawn()               
    {
        while (GameManager.Instance.currentState == GameState.Playing)
        {
            enemies = actualRound;
            OnRoundStart?.Invoke(actualRound);
            for(int i = 0; i < actualRound; i++)
            {
                InstantiateRandomEnemy();
            }
            yield return new WaitUntil(() => enemies == 0);                   
            yield return new WaitForSeconds(roundTimeWaiting);
            actualRound++;
        }
    }
    Vector3 TakeRandomSpawnPoint() 
    {
        Vector3 spawnPoint = spawnPoints[Random.Range(0,  spawnPoints.Length)];
        return spawnPoint;
    }
    void InstantiateRandomEnemy() 
    {
        List<GameObject> randomList = enemyPool[Random.Range(0, enemyPool.Count)];
        for (int i = 0; i < randomList.Count; i++)
        {
            GameObject enemy = randomList[i];

            if (enemy.activeInHierarchy == false)
            {
                enemy.transform.position = TakeRandomSpawnPoint();
                enemy.SetActive(true);
                return;
            }
        }
    }
    void CreateEnemyPool()
    {
        for (int i = 0; i < enemiesBoard.Length; i++)
        {
            List<GameObject> enemyList = new List<GameObject>();   //Creo una lista y la añado a la lista principal
            enemyPool.Add(enemyList);

            GameObject enemy = enemiesBoard[i];                   //Saco el enemigo del array principal y lo desactivo
            enemy.SetActive(false);

            for (int j = 0; j < PoolSize; j++)                    //Por cada lista creo 5 enemigos y los añado a su lista especifica
            {
                GameObject newEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
                enemyList.Add(newEnemy);
            }
        }
    }
}
