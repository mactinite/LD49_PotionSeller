using mactinite.ToolboxCommons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : SingletonMonobehavior<SoundtrackManager>
{
    public AudioSource audioSource;

    public AudioClip slowClip;
    public AudioClip fastClip;

    public float fadeTime= 1;
    private float volume = 1;

    public static float Volume
    {
        get => Instance.volume;
        set
        {
            Instance.volume = Mathf.Clamp01(value);
            Instance.audioSource.volume = Instance.volume;
        }
    }

    public static void GoToSlowTrack()
    {
        Instance.StartCoroutine(Instance.fadeToClip(Instance.slowClip));
    }


    IEnumerator fadeToClip(AudioClip clip)
    {
        float t = 0;
        while(t <= fadeTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }

        audioSource.clip = clip;
        audioSource.Play();
        t = 0;
        while (t <= fadeTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
    }

    public static void GoToFastTrack()
    {
        Instance.StartCoroutine(Instance.fadeToClip(Instance.fastClip));
    }

    public void SetVolume(float val)
    {
        Volume = val;
    }


}
