using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class UniversalAudio : AudioMaster
{
    static UniversalAudio instance;
    public static UniversalAudio Instance => instance;
    public void OnEnable()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
}
