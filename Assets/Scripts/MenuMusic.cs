using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuMusic : MonoBehaviour
{
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
    }

    public void PlayMusic()
    {
        if (!source.isPlaying)
            source.Play();
    }

    public void StopMusic()
    {
        source.Stop();
    }
}