using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HitBox")
        {
            Debug.Log("Player was hit.");
            //Get Dealt Damage
        }
    }
}
