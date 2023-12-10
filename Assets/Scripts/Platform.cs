using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Platform : MonoBehaviour
{
    const string AnimationName = "isFilled";

    private Animator _animator;
    private Barrel _barrel;
    private Robot _robot;

    public static Action<Barrel> onFreeBarrel;

    public bool IsFilling { get; private set; }

    private void Awake()
    {
        IsFilling = false;
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Robot.takeBarrel += ResetFilling;
    }

    private void OnDisable()
    {
        Robot.takeBarrel -= ResetFilling;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Robot>(out Robot robot))
        {
            _robot = robot;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<Robot>(out Robot robot))
        {
            if(_robot == robot && IsFilling == false)
                _animator.ResetTrigger(AnimationName);
        }
    }

    private void ResetFilling(Platform platform)
    {
        IsFilling = false;
    }

    public void SpawnBarrel(Barrel barrel)
    {
        Vector3 spawnPositoon = transform.position + new Vector3 (0f, 0f, 0f);
        _barrel = Instantiate(barrel, transform.position, Quaternion.identity);
        _barrel.transform.parent = gameObject.transform;
        _animator.SetTrigger(AnimationName);
        IsFilling = true;
    }

    public void EndAnimation()
    {
        onFreeBarrel?.Invoke(_barrel);
    }
}
