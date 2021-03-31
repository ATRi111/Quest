using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class scene : MonoBehaviour
{
    protected virtual void Start()
    {
        Random.InitState((int)Time.time);
    }

    public static AudioSource FindAudio(string name)
    {
        return GameObject.Find(name).GetComponent<AudioSource>();
    }
}
