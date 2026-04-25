using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{

    [SerializeField] private float _explodeRadius;
    [SerializeField] private bool _boomOnStart;
    [SerializeField] private bool _HasKaboomed;

#if UNITY_EDITOR
    [SerializeField] private bool _drawGizmos;
    [SerializeField] private Color _color = Color.red;
#endif

    private void Start()
    {
        if(_boomOnStart)
        {
            Boom();
        }
    }

    public void Boom()
    {
        if(_HasKaboomed == true)
        {
            return;
        }

        _HasKaboomed = true;
        Collider[] GameObjects = Physics.OverlapSphere(transform.position, _explodeRadius);

        for (int i = 0; i < GameObjects.Length; i++)
        {
            GameObjects[i].GetComponent<IDamageable>().TakeDamage();
        }

        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        StartCoroutine(DelayBoom());
    }   

    private IEnumerator DelayBoom()
    {
        yield return new WaitForSeconds(3f);
        Boom(); 
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        if (_drawGizmos == false)
        {
            return;
        }
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _explodeRadius);
    }
#endif

}

public interface IDamageable
{
    void TakeDamage();
}