using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("SFX")]
    public AudioSource sfxSource;

    [Header("Music Sources")]
    public AudioSource musicSourceA, musicSourceB; 

    private AudioSource activeSource, inactiveSource;

    private Coroutine currentMusicRoutine;

    [Header("Mixer")]
    public AudioMixer mixer;

    private void Awake() //singleton 
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(instance);

        activeSource = musicSourceA;
        inactiveSource = musicSourceB;
    }

    public void PlayMusic(AudioClip clip)
    {
        StopCurrentRoutine();

        activeSource.Stop();
        activeSource.clip = clip;
        activeSource.volume = 1f;
        activeSource.Play();
    }

    public void FadeInMusic(AudioClip clip, float duration)
    {
        StopCurrentRoutine();
        currentMusicRoutine = StartCoroutine(FadeInRoutine(clip, duration));
    }

    public void FadeOutMusic(float duration) 
    {
        StopCurrentRoutine();
        currentMusicRoutine = StartCoroutine(FadeOutRoutine(duration));
    }

    public void CrossfadeMusic(AudioClip clip, float duration)
    {
        StopCurrentRoutine();
        currentMusicRoutine = StartCoroutine(CrossfadeRoutine(clip, duration));
    }

    public void PlayMusicSmart(AudioClip clip, float duration = 1f)
    { 
        if(!activeSource.isPlaying)
            FadeInMusic(clip, duration);
        else
            CrossfadeMusic(clip, duration);
    }

    private IEnumerator FadeInRoutine(AudioClip clip, float duration)
    {
        activeSource.Stop();
        activeSource.clip = clip;
        activeSource.volume = 0f;
        activeSource.Play();

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / duration);
            activeSource.volume = progress;
            yield return null;
        }

        activeSource.volume = 1f;
    }

    private IEnumerator FadeOutRoutine(float duration)
    { 
        float startVolume = activeSource.volume;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / duration);
            activeSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            yield return null;
        }

        activeSource.Stop();
    }

    private IEnumerator CrossfadeRoutine(AudioClip newClip, float duration)
    {
        inactiveSource.Stop();
        inactiveSource.clip = newClip; 
        inactiveSource.volume = 0f; 
        inactiveSource.Play();

        float t = 0f;
        float startVolume = activeSource.volume;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / duration);

            activeSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            inactiveSource.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        activeSource.Stop();

        AudioSource temp = activeSource;
        activeSource = inactiveSource;
        inactiveSource = temp;
    }


    private void StopCurrentRoutine()
    { 
        if(currentMusicRoutine != null)
            StopCoroutine(currentMusicRoutine);
    }

    public void PlaySFX(AudioClip clip, bool randomPitch = false)
    {
        if (randomPitch)
            sfxSource.pitch = Random.Range(0.8f, 1.2f);
        else
            sfxSource.pitch = 1f;

        sfxSource.PlayOneShot(clip);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
