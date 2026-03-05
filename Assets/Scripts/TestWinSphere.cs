using UnityEngine;
using UnityEngine.SceneManagement;

public class TestWinSphere : MonoBehaviour
{
    public GameObject victoryPanel;
    public Camera winCam;
    [SerializeField] private bool hasWon;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
        {
            hasWon = true;
            other.gameObject.SetActive(false);
            winCam.gameObject.SetActive(true);
            victoryPanel.SetActive(true);
            
            if (hasWon)
            {
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene("TestScene");
                }
            }
        }
    }
}
