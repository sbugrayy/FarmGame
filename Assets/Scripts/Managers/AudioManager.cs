using System;
using UnityEngine;

public enum AudioClipType { grapClip, shopClip}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSource;
    public AudioClip grapClip, shopClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayAudio(AudioClipType clipType)
    {
        if (audioSource != null)
        {
            AudioClip audioClip = null;
            if (clipType == AudioClipType.grapClip)
            {
                audioClip = grapClip;
            }
            else if (clipType == AudioClipType.shopClip)
            {
                audioClip = shopClip;
            }
            
            audioSource.PlayOneShot(audioClip, 0.6f);
        }
    }

    public void StopBackgroundMusic()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
    }
}
