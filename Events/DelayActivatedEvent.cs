using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayActivatedEvent : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    [SerializeField] UnityEvent onDelayFire;
    public void Fire()
    {
        StartCoroutine(DelayFire(delay));
    }
    public IEnumerator DelayFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        onDelayFire.Invoke();
    }
}
