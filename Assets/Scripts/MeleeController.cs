using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [Header("Melee Attacking")]
    [SerializeField] float attackDistance = 3f;
    [SerializeField] float attackDelay = 0.4f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] int attackDamage = 0;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] KeyCode meleeAttackKey = KeyCode.Q;

    [Header("Camera")]
    [SerializeField] Camera cam;

    [Header("References")]
    [SerializeField] GameObject hitEffect;
    [SerializeField] AudioClip hitSound;
    
    AudioSource audioSource;

    bool attacking = false;
    bool readyToAttack = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(meleeAttackKey))
        {
            Attack();
        }
    }
    void Attack()
    {
        if(!readyToAttack || attacking)
        {
            return;
        }
        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);
    }
    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }
    void AttackRaycast()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, attackDistance, attackLayer))
        {
            HitTarget(hitInfo.point);
        }
    }
    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
}
