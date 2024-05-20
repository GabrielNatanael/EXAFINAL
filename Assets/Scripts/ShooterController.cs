using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] LayerMask mouseColliderMask;
    [Header("Weapons and Aim")]
    [SerializeField] Transform debugTransform;
    [SerializeField] Transform knifeProjectilePrefab;
    [SerializeField] Transform spawnProjectilePos;
    [SerializeField] Transform particleEffect = null;
    [Header("KeyCodes")]
    [SerializeField] KeyCode knifeThrowKey = KeyCode.E;
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 999f, mouseColliderMask))
        {
            debugTransform.position = hitInfo.point;
        }
        if (Input.GetKeyDown(knifeThrowKey))
        {
            Vector3 aimDir = (hitInfo.point - spawnProjectilePos.position).normalized;
            Instantiate(knifeProjectilePrefab, spawnProjectilePos.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
        if (Input.GetKeyDown(shootKey))
        {
            if(hitInfo.point != null)
            {
                Instantiate(particleEffect, hitInfo.point, Quaternion.identity);
            }
        }
    }
}
