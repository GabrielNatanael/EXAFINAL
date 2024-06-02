using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeProjectile : MonoBehaviour
{
    [Header("Knife Settings")]
    [SerializeField] float knifeSpeed = 10f;
    [SerializeField] Transform particleEffect = null;

    Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.velocity = transform.forward*knifeSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dummy"))
        {
            other.gameObject.GetComponent<Dummy>().DummyExplode();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().EnemyExplode();
        }
        if (other.gameObject.CompareTag("FlyingEnemy"))
        {
            other.gameObject.GetComponent<FlyingEnemy>().FlyEnemyExplode();
        }
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
