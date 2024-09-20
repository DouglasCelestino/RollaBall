using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSourceBackgroundMusic;
    public AudioSource audioSourceSFX;
    public AudioClip backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        audioSourceBackgroundMusic.clip = backgroundMusic;
        audioSourceBackgroundMusic.loop = true;
        audioSourceBackgroundMusic.Play();
    }

    // Update is called once per frame
    public void PlaySFX(AudioClip clip)
    {
        // toca uma unique sound effect
        audioSourceSFX.PlayOneShot(clip);
    }
}
