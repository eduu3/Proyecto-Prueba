using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    //General variables
    public CreatureData data;
    public StateMachine stateMachine;
    public static event Action OnEnemyKilled;
    public string enemyType;
    public int health;
    public int Health => health;
    Transform player;
    int maxHealth;
    int damage;
    float speed;


    //Spawn variables
    public SpawnState spawnState;
    Coroutine blinkCoroutine;
    [SerializeField] float spawnTime = 2;
    [SerializeField] float blinkRate = 0.1f;

    //Chase variables
    public ChaseState chaseState;

    //Repel variables
    public RepelState repelState;
    public Coroutine repelCoroutine;
    [SerializeField] float repelTime = 2f;
    float backRepelPositionMultiplier = 20f;

    //Die variables
    public DieState dieState;
    Animator animator;
    SpriteRenderer sr;
    [SerializeField] float dieTime = 2f;
    float colorChangeSpeed = 0.5f;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        animator = GetComponent<Animator>();

        player = GameObject.FindWithTag("Player").transform;

        ResetEnemy();

        stateMachine = new StateMachine();

        CreateStates();

        stateMachine.ChangeState(spawnState);

        GameManager.OnGameOver += GetFastDestroyed;
    }
    protected void OnDisable()
    {
        GameManager.OnGameOver -= GetFastDestroyed;
        StopAllCoroutines();
    }
    protected void Update()
    {
        transform.position = CheckBounds.CheckMarges(transform.position);
        stateMachine.Update();
    }


    //------------------------------------------------------------ Primary Methods--------------------------------------------------
    public virtual void DieEvent()
    {
        OnEnemyKilled?.Invoke();
    }
    void CreateStates()
    {
        chaseState = new ChaseState(this);
        spawnState = new SpawnState(this);
        repelState = new RepelState(this);
        dieState = new DieState(this);
    }
    //------------------------------------------------------------ Secundary Methods--------------------------------------------------
    protected void GetFastDestroyed(GameState currentState)
    {
        if (!(currentState == GameState.GameOver)) return;
        if (gameObject == null || this == null) return;
        gameObject.SetActive(false);
    }
    protected void ResetEnemy()
    {
        enemyType = data.enemyType;
        maxHealth = data.maxHealth;
        health = maxHealth;
        damage = data.damage;
        speed = data.speed;

        if(animator != null)animator.enabled = true;

        sr.enabled = true;
        sr.color = new Color(1, 1, 1, 1); 
    }
    //----------RepealState------------
    public void StartRepelTime()
    {
        repelCoroutine = StartCoroutine(RepelTime());
    }
    public void MoveBack()
    {
        Vector2 backDirection = (transform.position - player.position).normalized;
        Vector2 repelPos = backDirection * backRepelPositionMultiplier;
        transform.position = Vector3.MoveTowards(transform.position, repelPos, speed * Time.deltaTime);
    }
    public void ChangeColorToYellow()
    {
        Color color = sr.color;
        if (color.b > 0)
        {
            color.b -= colorChangeSpeed * Time.deltaTime;
            sr.color = color;
        }
    }
    public void GetNormalColor()
    {
        Color color = sr.color;
        if (color.b < 1)
        {
            color.b = 1;
            sr.color = color;
        }
    }
    public IEnumerator RepelTime()
    {
        yield return new WaitForSeconds(repelTime);
        if (stateMachine.CurrentState != dieState) stateMachine.ChangeState(chaseState);
        GetNormalColor();
    }
    public void EndRepealTime()
    {
        if (repelCoroutine != null)
        {
            StopCoroutine(repelCoroutine);
        }
    }
    //----------DieState------------
    public void DisableAnimator()
    {
        if (animator != null) animator.enabled = false;
    }
    public void StartDestroying()
    {
        StartCoroutine(GetDestroyed());
    }
    public void ChangeColorTransparency()
    {
        Color color = sr.color;
        if (color.a > 0)
        {
            color.a -= colorChangeSpeed * Time.deltaTime;
            sr.color = color;
        }
    }
    public IEnumerator GetDestroyed()
    {
        yield return new WaitForSeconds(dieTime);
        gameObject.SetActive(false);
    }
    //----------SpawnState------------
    public void LaunchEnemy()
    {
        float blinkTime = 0f;
        blinkCoroutine = StartCoroutine(Blink());

        IEnumerator Blink()
        {
            while (blinkTime < spawnTime)
            {
                sr.enabled = !sr.enabled;
                yield return new WaitForSeconds(blinkRate);
                blinkTime += blinkRate;
            }
            stateMachine.ChangeState(chaseState);
        }
    }
    public void EndSpawnTime()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
    }
    //----------ChaseState------------
    public void FollowPlayer()
    {
        if (player == null) return;
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    //------------------------------------------------------------ Damage Behavior----------------------------------------------------

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) stateMachine.ChangeState(dieState);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stateMachine.CurrentState != chaseState) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable player = collision.gameObject.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(damage);
                stateMachine.ChangeState(repelState);
            }
        }
    }
}
