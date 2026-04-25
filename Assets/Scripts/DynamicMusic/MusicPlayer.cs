using UnityEngine;
using UnityEngine.InputSystem;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] music;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayMusicSmart(music[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            AudioManager.instance.PlayMusicSmart(music[1]);
        }
    }
}
