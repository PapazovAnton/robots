using System;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private float _speed;

    private string _layerNameIgone = "Ignore";
    private Barrel _barrel;

    public static Action<Platform> takeBarrel;
    public static Action<Robot> freed;

    public Transform Target { get; private set; }
    public bool IsFill { get; private set; }

    private void Awake()
    {
        IsFill = false;
    }

    private void Update()
    {
        if(Target != null)
        {
            MoveTo(Target);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Barrel>(out Barrel barrel))
        {
            collision.transform.SetParent(transform, true);
            collision.gameObject.layer = LayerMask.NameToLayer(_layerNameIgone);
            Target = _base.transform;
            _barrel = barrel; 
            IsFill = true;

            takeBarrel?.Invoke(barrel.GetComponent<Platform>());
        }

        if (collision.TryGetComponent<Base>(out Base mainBase))
        {
            if (_barrel != null)
            {
                Destroy(_barrel.gameObject);
                IsFill = false;
                Target = null;
                freed?.Invoke(this);
            }
        }
    }

    private void MoveTo(Transform target)
    {
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(transform.position, target.GetComponent<Transform>().position, _speed * Time.deltaTime);
    }

    public bool TrySetTarget(Barrel target)
    {
        if(Target != null)
        {
            return false;
        } 
        else
        {
            Target = target.transform;
            return true;
        }
    }
}
