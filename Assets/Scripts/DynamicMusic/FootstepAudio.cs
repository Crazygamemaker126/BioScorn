using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioClip[] footstepSFX;
    public bool randomize;
    public void PlayFootstep()
    {
        Debug.Log("Footstep");
        AudioClip clip = footstepSFX[Random.Range(0, footstepSFX.Length)];
        AudioManager.instance.PlaySFX(clip, randomize);
    }
}
