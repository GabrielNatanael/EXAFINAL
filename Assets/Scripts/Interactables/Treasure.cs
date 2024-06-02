using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Interactables
{
    [SerializeField] GameObject sceneLoader;

    protected override void Interact()
    {
        sceneLoader.gameObject.GetComponent<SceneLoader>().LoadScene(4);
    }
}
