using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    AudioSource audio;
    public AudioClip[] playerAttackClips;
    Dictionary<AudioClip, int> clipToId;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        clipToId = new Dictionary<AudioClip, int>();
    }

    public void playMultipleClips(AudioClip[] clips)
    {
        var firstClip = clips[0];
        if (!clipToId.ContainsKey(firstClip))
        {
            clipToId[firstClip] = 0;
        }
        audio.PlayOneShot(clips[clipToId[firstClip]]);
        clipToId[firstClip]++;
        if (clipToId[firstClip] >= clips.Length)
        {
            clipToId[firstClip] = 0;
        }
    }

    public void playPlayerAttack()
    {
        playMultipleClips(playerAttackClips);
    }

    public void playAudio(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
