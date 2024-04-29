using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float distance = 3f;
    [SerializeField] LayerMask mask;

    private Camera cam;
    private PlayerUI playerUI;


    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        playerUI = GetComponent<PlayerUI>();
    }
    private void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactables>() != null)
            {
                Interactables interactables = hitInfo.collider.GetComponent<Interactables>();
                playerUI.UpdateText(interactables.promptMessage);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactables.BaseInteract();
                }
            }
        }
    }
}
