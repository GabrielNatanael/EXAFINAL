using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeProjectile : MonoBehaviour
{
    [Header("Knife Settings")]
    [SerializeField] float knifeSpeed = 10f;
    [SerializeField] Transform particleEffect = null;
    private Rigidbody rb;

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
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
