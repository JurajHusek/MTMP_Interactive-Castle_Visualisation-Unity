using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public void ActivateObject(GameObject obj) // activate chosed GameObject from hierarchy in scene
    {
        obj.SetActive(true); 
    }
    public void DeactivateObject(GameObject obj) // deactivate chosed GameObject from hierarchy in scene
    {
        obj.SetActive(false);
    }
}
