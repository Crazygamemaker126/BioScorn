using UnityEngine;
using TMPro;

public class TMPTest : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _tmp.text = "TEST MESSAGE";
            Color c = _tmp.color;
            c.a = 1f;
            _tmp.color = c;
            Debug.Log($"TMP color after set: {_tmp.color}");
        }
    }
}