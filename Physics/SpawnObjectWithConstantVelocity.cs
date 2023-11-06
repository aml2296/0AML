using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectWithConstantVelocity : MonoBehaviour
{
    [SerializeField] Transform source;
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3 constantVelocity;

    private float destroyTime = 0.1f;
    List<GameObject> spawnedObjs = new();
    List<Vector3> spawnedVels = new();

    public bool Gravity { get => useGravity; set => useGravity = value; }
    [SerializeField] private bool useGravity = true;

    public GameObject Fire()
    {
        var obj = GameObject.Instantiate(prefab, source);
        var rbody = obj.GetComponent<Rigidbody>();
        if (rbody == null)
            rbody = obj.GetComponentInChildren<Rigidbody>();
        if (rbody)
            rbody.useGravity = useGravity;
        spawnedObjs.Add(obj);
        spawnedVels.Add(constantVelocity);
        StartCoroutine(HandleVelocity(obj, constantVelocity));
        return obj;
    }
    protected IEnumerator HandleVelocity(GameObject spawnedObj, Vector3 velocity)
    {
        Rigidbody rbod = spawnedObj.GetComponent<Rigidbody>();
        while (spawnedObj.activeSelf)
        {
            if (rbod != null)
            {
                rbod.velocity = velocity;
                yield return new WaitForFixedUpdate();
            }
            else
            {
                spawnedObj.transform.position = spawnedObj.transform.position + velocity * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    internal void SetAmmo(GameObject prefab)
    {
        this.prefab = prefab;
    }
    public void StopVelCoroutine(int i)
    {
        if (i < 0 || i >= spawnedObjs.Count)
            return;
        var spawnedObj = spawnedObjs[i];
        var spawnedVel = spawnedVels[i];
        StopCoroutine(HandleVelocity(spawnedObj, spawnedVel));
    }
    public void StopAllVelCoroutines()
    {
        for (int i = 0; i < spawnedObjs.Count; i++)
            StopVelCoroutine(i);
    }
    public void ClearObjects()
    {
        if (destroyTime < 0)
            destroyTime = 0;
        GameObject[] objs = spawnedObjs.ToArray();
        for(int i =0; i < objs.Length; i++)
        {
            Destroy(objs[i], destroyTime);
        }
        spawnedObjs.Clear();
        spawnedVels.Clear();
    }
}
