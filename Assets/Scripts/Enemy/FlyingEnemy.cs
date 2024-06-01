using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float flyDistance = 5f;
    [SerializeField] float detectionRange = 10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float shootingCooldown = 2f;
    [SerializeField] Transform player;
    [Header("Health")]
    [SerializeField] float maxHealth = 60f;
    [SerializeField] float chipSpeed = 2f;
    [Header("Health Bar UI")]
    [SerializeField] Image frontHealthBar;
    [SerializeField] Image backHealthBar;

    private Vector3 initialPos;
    private bool movingUp = true;
    private float nextShotTime;

    private float health;
    private float lerpTimer;

    private void Start()
    {
        initialPos = transform.position;
        nextShotTime = Time.time;
        health = maxHealth;
    }

    private void Update()
    {
        if ( player == null)
        {
            return;
        }
        health = Mathf.Clamp(health,0, maxHealth);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if ( distanceToPlayer <= detectionRange)
        {
            ShootAtPlayer();
        }
        else
        {
            Fly();
        }
        UpdateHealthUI();
    }

    void Fly()
    {
        float distanceFromStart =  transform.position.y - initialPos.y;

        if(movingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            if(distanceFromStart >= flyDistance)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            if(distanceFromStart <= -flyDistance)
            {
                movingUp = true;
            }
        }
    }

    void ShootAtPlayer()
    {
        if(Time.time >= nextShotTime)
        {
            Vector3 dir = (player.position - transform.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(dir, Vector3.up));
            projectile.GetComponent<Rigidbody>().velocity = dir * projectileSpeed;

            nextShotTime = Time.time + shootingCooldown;
        }
    }

    public void FlyEnemyPunched()
    {
        float randomDamage = Random.Range(5, 10);
        TakeDamage(randomDamage);
    }
    public void FlyEnemyShoot()
    {
        float randomDamage = Random.Range(5, 20);
        TakeDamage(randomDamage);
    }
    public void FlyEnemyExplode()
    {
        float randomDamage = Random.Range(20, 40);
        TakeDamage(randomDamage);
    }
    void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);

        }
    }
    void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
