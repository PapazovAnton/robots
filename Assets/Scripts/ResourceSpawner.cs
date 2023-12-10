using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Barrel _template;
    [SerializeField] private int _spawnColdown;

    private List<Platform> _platforms = new List<Platform>();

    private void Awake()
    {
        Platform[] platforms = GetComponentsInChildren<Platform>();

        foreach (Platform platform in platforms)
        {
            _platforms.Add(platform);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnedBarels());
    }

    private IEnumerator SpawnedBarels()
    {
        while (true)
        {
            SpawnBarrel();
            yield return new WaitForSeconds(_spawnColdown);
        }
    }

    private void SpawnBarrel()
    {
        Platform platform = GetRandomFreePlatform();

        if(platform != null)
        {
            platform.SpawnBarrel(_template);
        }
    }

    private Platform GetRandomFreePlatform()
    {
        Platform[] freePlatforms = _platforms.Where(p => p.IsFilling == false).ToArray();

        if (freePlatforms.Length > 0)
        {
            int keyRandomPlatform = UnityEngine.Random.Range(0, freePlatforms.Length);
            return freePlatforms[keyRandomPlatform];
        }

        return null;
    }
}