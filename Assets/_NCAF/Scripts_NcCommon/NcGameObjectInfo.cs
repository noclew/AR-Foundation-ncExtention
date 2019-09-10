using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NcGameObjectInfo : MonoBehaviour
{
    // Initial transform parameters in global space
    public NcTransform OriginalTransformData;
    public Transform InitialParent { get; set; }
    // Start is called before the first frame update
    private void Awake()
    {
        OriginalTransformData = new NcTransform(transform);
        InitialParent = transform.parent;
    }

    private void Start()
    {

    }


    private void Update()
    {

    }

    public void ResetParent()
    {
        transform.SetParent(InitialParent);
    }
    public void SetActive(bool flag)
    {
        gameObject.SetActive(flag);
        return;
    }

    public override string ToString()
    {
        string msg = "name: " + OriginalTransformData.name + "\n" +
            "GlobalPos: " + OriginalTransformData.position + "\n" +
            "GlobalRot: " + OriginalTransformData.rotation + "\n" +
            "GlobalScale: " + OriginalTransformData.lossyScale + "\n" +
            "LocalPos: " + OriginalTransformData.localPosition + "\n" +
            "LocalRot: " + OriginalTransformData.localRotation + "\n" +
            "LocalScale: " + OriginalTransformData.localScale + "\n";
        return msg;
    }
}


public struct NcTransform
{
    public string name;
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }
    public Vector3 lossyScale { get; set; }

    public Vector3? localPosition { get; set; }
    public Quaternion? localRotation { get; set; }
    public Vector3? localScale { get; set; }

    public NcTransform(Transform tr)
    {
        name = tr.name;
        position = tr.position;
        rotation = tr.rotation;
        lossyScale = tr.lossyScale;
        //lossyScale = Vector3.one;

        localPosition = tr.localPosition;
        localRotation = tr.localRotation;
        localScale = tr.localScale;
        //localScale = Vector3.one;
    }

    public NcTransform(NcTransform tr)
    {
        name = tr.name;
        position = tr.position;
        rotation = tr.rotation;
        lossyScale = tr.lossyScale;
        //lossyScale = Vector3.one;

        localPosition = tr.localPosition;
        localRotation = tr.localRotation;
        localScale = tr.localScale;
        //localScale = Vector3.one;
    }

    public NcTransform(Vector3 pos_gl, Quaternion rot_gl, Vector3 sc_gl, string sc_name="")
    {
        name = sc_name;
        position = pos_gl;
        rotation = rot_gl;
        lossyScale = sc_gl;

        localPosition = null;
        localRotation = null;
        localScale = null;
    }

    public NcTransform GetSwappedYZTransform()
    {
        Vector3 originalScale= this.lossyScale;
        Vector3 originalLocalScale = this.localScale ?? default(Vector3);

        Vector3 newScale = originalScale;
        Vector3 newLocalScale = originalLocalScale;

        newScale.y = originalScale.z;
        newScale.z = originalScale.y;
        newLocalScale.y = originalLocalScale.z;
        newLocalScale.z = originalLocalScale.y;

        NcTransform res = new NcTransform(this);
        res.lossyScale = newScale;
        res.localScale = newLocalScale;
        return res;
    }

}
