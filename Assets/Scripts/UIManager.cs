using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    public void ShowMessage(string message)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;
    }

    public void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }
}
