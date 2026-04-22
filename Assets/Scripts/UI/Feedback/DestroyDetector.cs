using UnityEngine;

public class DestroyDetector : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.LogError($"[DestroyDetector] {name} is being destroyed! " +
                       $"Stack trace above will show who did it.");
    }
}