using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        InitGameManager(); 
    }

    private static void InitGameManager()
    {
        if (GameManager.Instance == null)
        {
            GameObject gameManager = new GameObject() { name = "Game Manager" };
            gameManager.AddComponent<GameManager>();
            gameManager.transform.position = new Vector2(0, 0); 
        }
    }
}
