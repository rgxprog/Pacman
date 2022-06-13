using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //-------------------------------------------

    public List<Sound> sounds = new List<Sound>();

    public static AudioManager instance;

    //-------------------------------------------

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    //-------------------------------------------

    public void Play(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
            sound.source.Play();
    }

    //-------------------------------------------

    public void Stop(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
            sound.source.Stop();
    }

    //-------------------------------------------

    public float GetClipLength(string name)
    {
        float clipLength = 0;

        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
            clipLength = sound.source.clip.length;

        return clipLength;
    }

    //-------------------------------------------
}

