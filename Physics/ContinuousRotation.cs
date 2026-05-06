using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    public bool pause = true;
    bool resetTrigger = false;
    [SerializeField] bool RotationOnStart = false;
    [SerializeField] Vector3 rotationSpeed;
    void Start()
    {
        if(RotationOnStart)
            pause = false;
    }
    void Update()
    {
        if (pause)
            return;
        transform.Rotate(Vector3.right, rotationSpeed.x);
        transform.Rotate(Vector3.up, rotationSpeed.y);
        transform.Rotate(Vector3.forward, rotationSpeed.z);
    }
    public void ChangeXAxis(float speed) => rotationSpeed.x = speed;
    public void ChangeYAxis(float speed) => rotationSpeed.y = speed;
    public void ChangeZAxis(float speed) => rotationSpeed.z = speed;
    public void ChangeSpeed(Vector3 speed) => rotationSpeed = speed;
    public void Reset() => resetTrigger = true;
}