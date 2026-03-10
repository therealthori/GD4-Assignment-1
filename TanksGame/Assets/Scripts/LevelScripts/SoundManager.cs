using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    public AudioSource movementSource;

    [Header("Audio Clips")]
    public AudioClip bulletbounce;
    public AudioClip wallBreak;
    public AudioClip playerDeath;
    public AudioClip TankMove;
    public AudioClip reloadSound;
    public AudioClip shootSound;
    public AudioClip bulletbreak;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play a sound effect
    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySoundWithPitch(AudioClip clip, float pitch)
    {
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip);
        sfxSource.pitch = 1f;
    }
    public void PlaySoundWithVolume(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
    public void StartTankMove()
    {
        if (!movementSource.isPlaying)
{
    movementSource.clip = TankMove;
    movementSource.loop = true;
    movementSource.Play();
}
    }
    public void StopTankMove()
    {
       if (movementSource.isPlaying)
{
    movementSource.Stop();
}
    }
}

