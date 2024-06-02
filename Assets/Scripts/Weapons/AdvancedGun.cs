using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedGun : ShooterController
{
    [Header("Advanced Pistol")]
    [SerializeField] Transform advancedShootEffect = null;
    [SerializeField] float advancedPistolFireRate = 0.2f;

    bool canShootAdvancedPistol = true;

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(attackKey) && currentWeapon == EquippedWeapon.pistol && canShootAdvancedPistol)
        {
            StartCoroutine(AdvancedShootRoutine());
        }
    }
    protected override void Shoot(RaycastHit hitInfo)
    {
        
    }
    IEnumerator AdvancedShootRoutine()
    {
        while (Input.GetKeyDown(attackKey)&&currentWeapon == EquippedWeapon.pistol)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, 999f, mouseColliderMask))
            {
                AdvancedShoot(hitInfo);
            }
            yield return new WaitForSeconds(advancedPistolFireRate);
        }
    }

    void AdvancedShoot(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null)
        {
            Instantiate(advancedShootEffect, hitInfo.point, Quaternion.identity);
            pistolAnim.SetTrigger("GunFired");
            audioSource.PlayOneShot(gunSound);

            if (hitInfo.collider.CompareTag("Dummy"))
            {
                Dummy dummy = hitInfo.collider.GetComponent<Dummy>();
                if (dummy != null)
                {
                    dummy.DummyShot();
                }
            }
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.EnemyShoot();
                }
            }
            if (hitInfo.collider.CompareTag("FlyingEnemy"))
            {
                FlyingEnemy flyEnemy = hitInfo.collider.GetComponent<FlyingEnemy>();
                if (flyEnemy != null)
                {
                    flyEnemy.FlyEnemyShoot();
                }
            }
        }
    }
}
