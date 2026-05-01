using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public CreatureData data;
    public static event Action OnPlayerDeath;
    public int health;
    public int Health => health;
    [SerializeField] Sprite IdleSprite;
    [SerializeField] Sprite AttackSprite;
    [SerializeField] float animationWaitTime;
    SpriteRenderer sr;
    int maxHealth;
    int damage;

    private void Start()
    {
        maxHealth = data.maxHealth;
        health = maxHealth;
        damage = data.damage;
        sr = GetComponent<SpriteRenderer>();
        GameManager.OnRestartGame += ResetHealth;
    }
    private void OnDestroy()
    {
        GameManager.OnRestartGame -= ResetHealth;
    }
    private void Update()
    {
        CheckDamage();
    }
    void CheckDamage()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                IDamageable enemigo = hit.collider.GetComponent<IDamageable>();
                if (enemigo != null && enemigo.Health > 0)
                {
                    enemigo.TakeDamage(damage);
                    StartCoroutine(ChangeSpriteToAttack());
                }
            }
        }
    }
    IEnumerator ChangeSpriteToAttack()
    {
        sr.sprite = AttackSprite;
        yield return new WaitForSeconds(animationWaitTime);
        sr.sprite = IdleSprite;
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            OnPlayerDeath?.Invoke();  //Rompe el bucle del spawnManager
            Debug.Log("Player muerto"); 
        }
    }
    void ResetHealth(GameState currentState)
    {
        if(currentState == GameState.WaitingToStart) health = maxHealth;
    }
}
