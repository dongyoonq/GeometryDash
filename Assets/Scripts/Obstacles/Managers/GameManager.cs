using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const string DefaultName = "GameManager";

    private static GameManager instance;
    private static PoolManager poolManager;

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return poolManager; } }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("GameInstance: valid instance already registered.");
            Destroy(this);
            return;
        }

        instance = this;
        gameObject.transform.position = new Vector2(0, 0); 
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject poolObj = GameObject.FindGameObjectWithTag("PoolManager");
        poolObj.transform.SetParent(transform); 
        poolManager = poolObj.GetComponent<PoolManager>();
    }
}
