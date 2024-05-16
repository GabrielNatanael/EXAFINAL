using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform oriantation;
    [SerializeField] Transform player;
    [SerializeField] Transform playerObj;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform combatLookAt;

    [SerializeField] GameObject thirdPersonCam;
    [SerializeField] GameObject combatCam;

    [SerializeField] float rotationSpeed;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ChangeCamStyle(CameraStyle.Basic); 
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            ChangeCamStyle(CameraStyle.Combat);
        }
        if(Input.GetKey(KeyCode.B))
        {
            ChangeCamStyle(CameraStyle.Basic);
        }

        Vector3 viewDir= player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        oriantation.forward = viewDir.normalized;

        if(currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = oriantation.forward * verticalInput + oriantation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            Vector3 dirCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            oriantation.forward = dirCombatLookAt.normalized;

            playerObj.forward = dirCombatLookAt.normalized;
        }
    }

    void ChangeCamStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        if(newStyle == CameraStyle.Basic)
        {
            thirdPersonCam.SetActive(true);
        }
        if(newStyle == CameraStyle.Combat)
        {
            combatCam.SetActive(true);
        }
        currentStyle = newStyle;
    }
}
