using UnityEngine;

public class EnemyMusicTrigger : MonoBehaviour
{
    private DynamicMusic musicManager;

    void Start()
    {
        musicManager = FindObjectOfType<DynamicMusic>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            musicManager.EnterCombat();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            musicManager.ExitCombat();
        }
    }
}