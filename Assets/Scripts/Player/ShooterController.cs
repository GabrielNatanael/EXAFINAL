using System.Collections;
using System.Collections.Generic;
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
    [Header("Melee")]
    [SerializeField] float punchDistance = 3;
    [SerializeField] Transform PunchEffect;
    [Header("KeyCodes")]
    [SerializeField] KeyCode knifeThrowKey = KeyCode.E;
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode punchKey = KeyCode.Q;
    [Header("Animators")]
    [SerializeField] Animator pistolAnim=null;
    [SerializeField] Animator fistAnim=null;
    private void Awake()
    {
        Pistol.SetActive(false);
        Fists.SetActive(true);
        KnifeLauncher.SetActive(false);
    }
    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 999f, mouseColliderMask))
        {
            debugTransformShoot.position = hitInfo.point;
        }
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit punchHitInfo, punchDistance, mouseColliderMask))
        {
            debugTransformPunch.position = punchHitInfo.point;
        }
        if (Input.GetKeyDown(knifeThrowKey))
        {
            Pistol.SetActive(false);
            Fists.SetActive(false);
            KnifeLauncher.SetActive(true);

            Vector3 aimDir = (hitInfo.point - spawnProjectilePos.position).normalized;
            Instantiate(knifeProjectilePrefab, spawnProjectilePos.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
        if (Input.GetKeyDown(shootKey))
        {
            Pistol.SetActive(true);
            Fists.SetActive(false);
            KnifeLauncher.SetActive(false);

            if (hitInfo.point != null)
            {
                Instantiate(ShootEffect, hitInfo.point, Quaternion.identity);
                pistolAnim.SetTrigger("GunFired");
            }
            if (hitInfo.collider.CompareTag("Dummy"))
            {
                hitInfo.collider.gameObject.GetComponent<Dummy>().DummyShot();
            }
        }
        if (Input.GetKeyDown(punchKey))
        {
            Pistol.SetActive(false);
            Fists.SetActive(true);
            KnifeLauncher.SetActive(false);

            if (punchHitInfo.point != null)
            {
                Instantiate(PunchEffect, punchHitInfo.point, Quaternion.identity);
                fistAnim.SetTrigger("PunchTrigger");
            }
            if (punchHitInfo.collider.CompareTag("Dummy"))
            {
                punchHitInfo.collider.gameObject.GetComponent<Dummy>().DummyPunched();
            }
        }
    }
}
