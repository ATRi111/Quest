using UnityEngine;
using Public;

public class CAudioController : CSigleton<CAudioController>
{
    public static void PlayAudio(string name)
            => GameObject.Find(name).GetComponent<AudioSource>().Play();
    public static void StopAudio(string name)
        => GameObject.Find(name).GetComponent<AudioSource>().Stop();
}
