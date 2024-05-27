using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations;

public class ShooterController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] LayerMask mouseColliderMask;
    [Header("Weapons Aim")]
    [SerializeField] Transform debugTransformShoot;
    [SerializeField] Transform debugTransformPunch;
    [SerializeField] Transform knifeProjectilePrefab;
    [SerializeField] Transform spawnProjectilePos;
    [SerializeField] Transform ShootEffect = null;
    [Header("Weapons")]
    [SerializeField] GameObject Pistol;
    [SerializeField] GameObject KnifeLauncher;
    [SerializeField] GameObject Fists;
    [SerializeField] GameObject PunchHitBox;
    [Header("Melee")]
    [SerializeField] float punchDistance = 3;
    [SerializeField] Transform PunchEffect;
    [SerializeField] float punchCooldown;
    [Header("KeyCodes")]
    [SerializeField] KeyCode switchKnifeThrowKey = KeyCode.Alpha3;
    [SerializeField] KeyCode switchShootKey = KeyCode.Alpha2;
    [SerializeField] KeyCode SwitchPunchKey = KeyCode.Alpha1;
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
    [Header("Animators")]
    [SerializeField] Animator pistolAnim=null;
    [SerializeField] Animator fistAnim=null;

    bool canPunch;

    Collider punchCollider;

    enum EquippedWeapon { Fists, pistol, knifeLauncher}
    EquippedWeapon currentWeapon = EquippedWeapon.Fists;

    private void Awake()
    {
        SwitchWeapon(EquippedWeapon.Fists);
        canPunch = true;
        punchCollider = PunchHitBox.GetComponent<Collider>();
    }
    private void Update()
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
            Punch();
            fistAnim.SetTrigger("PunchTrigger");
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
    }
    void SwitchWeapon(EquippedWeapon weapon)
    {
        Pistol.SetActive(weapon == EquippedWeapon.pistol);
        Fists.SetActive(weapon == EquippedWeapon.Fists);
        KnifeLauncher.SetActive(weapon == EquippedWeapon.knifeLauncher);
        currentWeapon = weapon;
    }
    void Punch()
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
    void Shoot(RaycastHit hitInfo)
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
    void KnifeLaunch(RaycastHit hitInfo)
    {
        Vector3 aimDir = (hitInfo.point - spawnProjectilePos.position).normalized;
        Instantiate(knifeProjectilePrefab, spawnProjectilePos.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
    IEnumerator PunchCooldown()
    {
        yield return new WaitForSeconds(punchCooldown);

        canPunch = true;
    }
}
