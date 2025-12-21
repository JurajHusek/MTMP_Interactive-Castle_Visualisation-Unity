using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public Transform ship1Object;
    public float rotationSpeed = 20f;
    private bool isRotatingShip1 = false;
    public Transform ship2Object;
    private bool isRotatingShip2 = false;

    private void Update()
    {
        if (isRotatingShip1)
        {
            ship1Object.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
        }
        if (isRotatingShip2)
        {
            ship2Object.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
        }
    }
    public void ActivateObject(GameObject obj)
    {
        obj.SetActive(true); 
    }
    public void DeactivateObject(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void RotationShip1(bool status)
    {
        isRotatingShip1 = status;
    }
    public void RotationShip2(bool status)
    {
        isRotatingShip2 = status;
    }
}
