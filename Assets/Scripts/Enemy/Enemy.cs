using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float walkDistance = 5f;
    [SerializeField] float detectionRange = 10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float shootingCooldown = 2f;
    [SerializeField] Transform playerManualTransform = null;
    [Header("Health")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float chipSpeed = 2f;
    [Header("Health Bar UI")]
    [SerializeField] Image frontHealthBar;
    [SerializeField] Image backHealthBar;
    [Header("Audio")]
    [SerializeField] AudioClip hurtSoundEffect = null;

    private AudioSource audioSource;
    private Vector3 initialPos;
    private bool movingRight = true;
    private float nextShotTime;
    private Transform player;

    private float health;
    private float lerpTimer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialPos = transform.position;
        nextShotTime = Time.time;
        health = maxHealth;
    }
    private void Update()
    {
        if(player == null)
        {
            player = playerManualTransform;
        }

        health = Mathf.Clamp(health, 0, maxHealth);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer <= detectionRange)
        {
            ShootAtPlayer();
        }
        else
        {
            Walk();
        }
        UpdateHealthUI();
    }

    void Walk()
    {
        float distanceFromStart = transform.position.x - initialPos.x;

        if(movingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            if(distanceFromStart>=walkDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if(distanceFromStart <= -walkDistance)
            {
                movingRight=true;
            }
        }
    }
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
    void ShootAtPlayer()
    {
        if(Time.time >= nextShotTime)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab,transform.position, Quaternion.LookRotation(dir, Vector3.up));
            projectile.GetComponent<Rigidbody>().velocity = dir * projectileSpeed;
            nextShotTime = Time.time + shootingCooldown;
        }
    }
    public void EnemyPunched()
    {
        float randomDamage = Random.Range(5, 10);
        TakeDamage(randomDamage);
    }
    public void EnemyShoot()
    {
        float randomDamage = Random.Range(5, 20);
        TakeDamage(randomDamage);
    }
    public void EnemyExplode()
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
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(hurtSoundEffect);
        health -= damage;
        lerpTimer = 0f;
        if(health <= 0f)
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
