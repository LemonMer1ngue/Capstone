using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformManager : MonoBehaviour
{
    public static FallingPlatformManager Instance { get; private set; }

    [SerializeField] private GameObject fallingPlatformPrefab;

    private void Awake()
    {
        // Pastikan hanya ada satu instance dari PrefabManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Metode untuk mendapatkan prefab falling platform
    public GameObject GetFallingPlatformPrefab()
    {
        if (fallingPlatformPrefab != null)
        {
            return fallingPlatformPrefab;
        }
        else
        {
            Debug.LogError("Falling Platform Prefab is not assigned in PrefabManager!");
            return null;
        }
    }
}
