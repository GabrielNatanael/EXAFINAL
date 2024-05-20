using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask mouseColliderMask;
    [SerializeField] Transform debugTransform;
    [SerializeField] Transform knifeProjectilePrefab;
    [SerializeField] Transform spawnProjectilePos;
    [SerializeField] KeyCode knifeThrowKey = KeyCode.E;
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
    }
}
