using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField] string[] tag = null;
    [SerializeField] LayerMask mask;
    [SerializeField] UnityEvent OnEnter;
    [SerializeField] UnityEvent OnStay;
    [SerializeField] UnityEvent OnLeave;

    private bool TagCheck(Collider other)
    {
        if (tag == null)
            return true;
        foreach (string t in tag)
            if (other.gameObject.CompareTag(t))
            {
                return true;
            }
        return false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (TagCheck(other))
            OnEnter?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (TagCheck(other))
            OnLeave?.Invoke();
    }
    private void OnTriggerStay(Collider other)
    {
        if (TagCheck(other))
            OnStay?.Invoke();
    }
}
