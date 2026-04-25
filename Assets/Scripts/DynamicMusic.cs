using UnityEngine;
using System.Collections;

public class DynamicMusic : MonoBehaviour
{
    public AudioSource calmSource;
    public AudioSource combatSource;

    public float fadeDuration = 2f;

    private bool inCombat = false;

    void Start()
    {
        calmSource.volume = 1f;
        combatSource.volume = 0f;

        calmSource.Play();
        combatSource.Play();
    }

    public void EnterCombat()
    {
        if (!inCombat)
        {
            inCombat = true;
            StartCoroutine(Crossfade(calmSource, combatSource));
        }
    }

    public void ExitCombat()
    {
        if (inCombat)
        {
            inCombat = false;
            StartCoroutine(Crossfade(combatSource, calmSource));
        }
    }

    IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float t = time / fadeDuration;

            from.volume = Mathf.Lerp(1f, 0f, t);
            to.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        from.volume = 0f;
        to.volume = 1f;
    }
}