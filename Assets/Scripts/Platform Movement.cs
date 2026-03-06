
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    public bool moveHorizontal;
    public bool moveVertical;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (moveHorizontal)
        {
            transform.position = startPosition + new Vector3(Mathf.Sin(Time.time * speed) * distance, 0, 0);
        }

        if (moveVertical)
        {
            transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * speed) * distance, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}