using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoldItemRender : MonoBehaviour
{
    [SerializeField] protected MeshRenderer renderMain;
    [SerializeField] protected MeshRenderer renderCopy;
    [SerializeField] protected MeshFilter meshMain;
    [SerializeField] protected MeshFilter meshCopy;
    [SerializeField] protected Transform target;

    private void Start()
    {
        renderMain = target.GetComponent<MeshRenderer>();
        meshMain = target.GetComponent<MeshFilter>();

        ApplyCopy();
    }

    protected void ApplyCopy()
    {
        if (renderMain == null)
            renderMain = target.AddComponent<MeshRenderer>();
        if (meshMain == null)
            meshMain = target.AddComponent<MeshFilter>();

        renderMain.sharedMaterial = renderCopy.sharedMaterial;
        meshMain.sharedMesh = meshCopy.sharedMesh;
    }

    public void Hide() => target.gameObject.SetActive(false);
    public void Show() => target.gameObject.SetActive(true);
    public void CopyToTarget(Transform from)
    {
        renderCopy = from.GetComponent<MeshRenderer>();
        meshCopy = from.GetComponent<MeshFilter>();
        ApplyCopy();
    }
}
