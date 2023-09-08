using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioMaster : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] AudioSource[] source;
    int sourceCounter = 0;
    public int Count => audioClips.Count;
    private void OnEnable()
    {
        FindSource();
    }
    private void Start()
    {
        FindSource();
    }
    private void FindSource()
    {
        if (source == null)
        {
            AudioSource sourceTry = GetComponent<AudioSource>();
            if (sourceTry == null)
                Debug.LogError("No Audio sources for AudioMaster!! [" + gameObject.name + "]");

            source = new AudioSource[1];
            source[0] = sourceTry;
        }
    }
public void FireRandomRange(int minInclusive, int maxExclusive) => Fire(Random.Range(minInclusive, maxExclusive));
    public void FireRandom() => FireRandomRange(0, Count);
    public void Fire(string name)
    {
        IEnumerable<AudioClip> foundClips = audioClips.Where(p => p.name == name);
        if (foundClips.Any())
        {
            int startSourceCounter = sourceCounter;
            while (source[sourceCounter].isPlaying)
            {
                sourceCounter++;
                if (sourceCounter >= source.Length)
                    sourceCounter = 0;
                if (sourceCounter == startSourceCounter)
                {
                    Debug.LogWarning("Ran Out of AudioSources for: " + gameObject.name);
                    break;
                }
            }
            source[sourceCounter].clip = foundClips.First();
            source[sourceCounter].loop = false;
            source[sourceCounter].Play();
        }
    }
    public void Fire(int index)
    {
        if (index < 0 || index >= audioClips.Count)
            return;
        int startSourceCounter = sourceCounter;
        while (source[sourceCounter].isPlaying)
        {
            sourceCounter++;
            if (sourceCounter >= source.Length)
                sourceCounter = 0;
            if (sourceCounter == startSourceCounter)
            {
                Debug.LogWarning("Ran Out of AudioSources for: " + gameObject.name + "Overriding source index " + startSourceCounter);
                break;
            }
        }
        source[sourceCounter].clip = audioClips[index];
        source[sourceCounter].loop = false;
        source[sourceCounter].Play();
    }
}
