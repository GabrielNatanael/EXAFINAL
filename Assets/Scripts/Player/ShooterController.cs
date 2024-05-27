using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations;

public class ShooterController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] protected Camera cam;
    [SerializeField] protected LayerMask mouseColliderMask;
    [Header("Weapons Aim")]
    [SerializeField] protected Transform debugTransformShoot;
    [SerializeField] protected Transform debugTransformPunch;
    [SerializeField] protected Transform knifeProjectilePrefab;
    [SerializeField] protected Transform spawnProjectilePos;
    [SerializeField] protected Transform ShootEffect = null;
    [Header("Weapons")]
    [SerializeField] protected GameObject Pistol;
    [SerializeField] protected GameObject KnifeLauncher;
    [SerializeField] protected GameObject Fists;
    [SerializeField] protected GameObject PunchHitBox;
    [Header("Melee")]
    [SerializeField] protected float punchDistance = 3;
    [SerializeField] protected  Transform PunchEffect;
    [SerializeField] protected float punchCooldown;
    [SerializeField] protected float comboResetTime = 1.5f;
    [Header("Airborne Detection")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float groundCheckDistance = 0.1f;
    [SerializeField] protected float airAttackFallMultiplier = 0.5f;
    [Header("KeyCodes")]
    [SerializeField] protected KeyCode switchKnifeThrowKey = KeyCode.Alpha3;
    [SerializeField] protected KeyCode switchShootKey = KeyCode.Alpha2;
    [SerializeField] protected KeyCode SwitchPunchKey = KeyCode.Alpha1;
    [SerializeField] protected KeyCode attackKey = KeyCode.Mouse0;
    [Header("Animators")]
    [SerializeField] protected Animator pistolAnim=null;
    [SerializeField] protected Animator fistAnim=null;

    protected bool canPunch;
    protected int punchComboIndex = 0;
    protected int airPunchComboIndex = 0;
    protected float lastPunchTime;

    protected Collider punchCollider;
    protected Rigidbody rb;

    protected enum EquippedWeapon { Fists, pistol, knifeLauncher}
    protected EquippedWeapon currentWeapon = EquippedWeapon.Fists;

    protected virtual void Awake()
    {
        SwitchWeapon(EquippedWeapon.Fists);
        canPunch = true;
        punchCollider = PunchHitBox.GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, 999f, mouseColliderMask))
        {
            debugTransformShoot.position = hitInfo.point;
        }

        if (Input.GetKeyDown(switchKnifeThrowKey))
        {
            SwitchWeapon(EquippedWeapon.knifeLauncher);
        }
        if (Input.GetKeyDown(switchShootKey))
        {
            SwitchWeapon(EquippedWeapon.pistol);
        }
        if (Input.GetKeyDown(SwitchPunchKey))
        {
            SwitchWeapon(EquippedWeapon.Fists);
        }

        if (Input.GetKeyDown(attackKey)&&currentWeapon == EquippedWeapon.Fists&&canPunch)
        {
            PerformPunch();
            StartCoroutine(PunchCooldown());
        }

        if(Input.GetKeyDown(attackKey)&&currentWeapon == EquippedWeapon.pistol)
        {
            Shoot(hitInfo);
        }

        if (Input.GetKeyDown(attackKey) && currentWeapon == EquippedWeapon.knifeLauncher)
        {
            KnifeLaunch(hitInfo);
        }
        if(Time.time - lastPunchTime > comboResetTime)
        {
            punchComboIndex = 0;
            airPunchComboIndex = 0;
        }
    }
    protected virtual void SwitchWeapon(EquippedWeapon weapon)
    {
        Pistol.SetActive(weapon == EquippedWeapon.pistol);
        Fists.SetActive(weapon == EquippedWeapon.Fists);
        KnifeLauncher.SetActive(weapon == EquippedWeapon.knifeLauncher);
        currentWeapon = weapon;
    }
    protected virtual void PerformPunch()
    {
        canPunch = false;
        lastPunchTime = Time.time;

        if(IsGrounded())
        {
            if (punchComboIndex == 0)
            {
                fistAnim.SetTrigger("punchTrigger1");
            }
            else if (punchComboIndex == 1)
            {
                fistAnim.SetTrigger("punchTrigger2");
            }
            else if (punchComboIndex == 2)
            {
                fistAnim.SetTrigger("punchTrigger3");
            }
            punchComboIndex = (punchComboIndex + 1) % 3;
            Punch();
        }
        else
        {
            if (airPunchComboIndex == 0)
            {
                fistAnim.SetTrigger("airPunchTrigger1");
            }
            else if (airPunchComboIndex == 1)
            {
                fistAnim.SetTrigger("airPunchTrigger2");
            }
            else if (airPunchComboIndex == 2)
            {
                fistAnim.SetTrigger("airPunchTrigger3");
            }
            airPunchComboIndex = (airPunchComboIndex + 1) % 3;
            Punch();
            SlowFall();
        }
    }
    protected virtual void Punch()
    {
        RaycastHit punchHitInfo;

        canPunch = false;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out punchHitInfo, punchDistance, mouseColliderMask))
        {
            if(punchHitInfo.collider != null)
            {
                Instantiate(PunchEffect, punchHitInfo.point, Quaternion.identity);
            }
        }
        if(punchCollider != null && punchHitInfo.collider != null)
        {
            if (punchCollider.isTrigger&&punchHitInfo.collider.CompareTag("Dummy"))
            {
                punchHitInfo.collider.gameObject.GetComponent<Dummy>().DummyPunched();
            }
        }
    }
    protected virtual void Shoot(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null)
        {
            Instantiate(ShootEffect, hitInfo.point, Quaternion.identity);
            pistolAnim.SetTrigger("GunFired");

            if (hitInfo.collider.CompareTag("Dummy"))
            {
                hitInfo.collider.gameObject.GetComponent<Dummy>().DummyShot();
            }
        }
    }
    protected virtual void KnifeLaunch(RaycastHit hitInfo)
    {
        Vector3 aimDir = (hitInfo.point - spawnProjectilePos.position).normalized;
        Instantiate(knifeProjectilePrefab, spawnProjectilePos.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
    protected virtual void SlowFall()
    {
        if (rb!=null&& rb.velocity.y < 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * airAttackFallMultiplier, rb.velocity.z);
        }
    }
    protected virtual IEnumerator PunchCooldown()
    {
        yield return new WaitForSeconds(punchCooldown);

        canPunch = true;
    }
    protected virtual bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
