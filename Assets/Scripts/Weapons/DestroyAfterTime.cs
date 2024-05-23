using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float destroyTimer = 5f;

    private void Start()
    {
        Destroy(gameObject, destroyTimer);
    }
}
