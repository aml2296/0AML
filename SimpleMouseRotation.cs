using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleMouseRotation : MonoBehaviour
{
    [SerializeField] float sensitivity = 1f;
    [SerializeField] Transform ObjectToRotate;
    [SerializeField] Vector3 axis = Vector3.up;

    [SerializeField] float minimumChange = 0.5f;

    void Update()
    {
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");
        Vector3 changeInposition = new Vector3(xAxis, yAxis, 0);
        if (changeInposition.sqrMagnitude > minimumChange * minimumChange)
        {
            ObjectToRotate.RotateAround(transform.position, axis, changeInposition.x * sensitivity * Time.deltaTime);
        }
    }
}