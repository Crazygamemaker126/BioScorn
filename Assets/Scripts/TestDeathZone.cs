using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class TestDeathZone : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Camera deathCam;

    [SerializeField] private bool playerHasDied;

    private void Awake()
    {
        playerHasDied = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.SetActive(false);
            playerHasDied = true;

            deathCam.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (playerHasDied)
        {
            gameOverPanel.SetActive(true);

            if (Input.anyKeyDown) 
            {
                SceneManager.LoadScene("TestScene");
            }
        }
    }
}
