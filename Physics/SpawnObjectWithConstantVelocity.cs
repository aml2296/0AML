using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectWithConstantVelocity : MonoBehaviour
{
    [SerializeField] Transform source;
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3 constantVelocity;

    List<GameObject> spawnedObjs = new();
    List<Vector3> spawnedVels = new();

    public GameObject Fire()
    {
        var obj = GameObject.Instantiate(prefab, source);
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
}
