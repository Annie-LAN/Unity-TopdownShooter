using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundLibrary : MonoBehaviour
{
    public SoundGRoup[] soundGroups;

    Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

    void Awake()
    {
        foreach(SoundGRoup soundGroup in soundGroups)
        {
            groupDictionary.Add(soundGroup.groupID, soundGroup.group);
        }        
    }
    public AudioClip GetClipFromName(string name)
    {
        if (groupDictionary.ContainsKey(name))
        {
            AudioClip[] sounds = groupDictionary[name];
            return sounds[UnityEngine.Random.Range(0, sounds.Length)];
        }
        return null;
    }

    [System.Serializable]
    public class SoundGRoup
    {
        public string groupID;
        public AudioClip[] group;
    }
}
