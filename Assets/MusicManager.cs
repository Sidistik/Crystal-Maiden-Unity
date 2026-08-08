using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music Clips")]
    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip fightMusic;
    [SerializeField] private AudioClip bossMusic;

    [Header("Volume Settings")]
    [SerializeField] private float normalMusicVolume = 1f;
    [SerializeField] private float fightMusicVolume = 1f;
    [SerializeField] private float bossMusicVolume = 1f;

    [Header("Music Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private AudioSource audioSource;
    private bool isFading = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayNormalMusic();
    }

    public void PlayNormalMusic()
    {
        if (audioSource.clip != normalMusic)
            StartCoroutine(FadeToMusic(normalMusic, normalMusicVolume));
        else
            audioSource.volume = normalMusicVolume; 
    }

    public void PlayFightMusic()
    {
        if (audioSource.clip != fightMusic)
            StartCoroutine(FadeToMusic(fightMusic, fightMusicVolume));
        else
            audioSource.volume = fightMusicVolume;
    }

    public void PlayBossMusic()
    {
        if (audioSource.clip != bossMusic)
            StartCoroutine(FadeToMusic(bossMusic, bossMusicVolume));
        else
            audioSource.volume = bossMusicVolume;
    }


    private IEnumerator FadeToMusic(AudioClip newClip, float targetVolume)
    {
        if (isFading) yield break;

        isFading = true;

        float startVolume = audioSource.volume;

    
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        
        audioSource.clip = newClip;
        audioSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
        isFading = false;
    }
}
