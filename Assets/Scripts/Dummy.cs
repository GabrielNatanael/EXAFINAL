using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dummy : MonoBehaviour
{
    [SerializeField] Transform lookAtTransform;
    [Header("Health")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float chipSpeed = 2f;
    [Header("Health Bar UI")]
    [SerializeField] Image frontHealthBar;
    [SerializeField] Image backHealthBar;

    private float health;
    private float lerpTimer;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        health = maxHealth;
    }
    void Update()
    {   
        transform.LookAt(lookAtTransform);

        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }
    public void Attacked()
    {
        anim.SetTrigger("AttackTrigger");
        float randomDamage = Random.Range(5, 20);
        TakeDummyDamage(randomDamage);
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

    void TakeDummyDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;

        if(health <= 0f)
        {
            RestoreDummyHealth(maxHealth);
        }
    }
    void RestoreDummyHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
