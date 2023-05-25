using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public GameObject[] blockPrefabs; // 총알 처럼 쓰일 오브젝트 풀링 대상 

    // Dictionaries to store active and inactive game objects based on their types
    public Dictionary<string, Queue<GameObject>> activeGameObjects = new Dictionary<string, Queue<GameObject>>(); // 활성화 중인 블럭 
    public Dictionary<string, Queue<GameObject>> inactiveGameObjects = new Dictionary<string, Queue<GameObject>>(); // 비활성화 된 블럭 

    public event UnityAction Trigger;
    private void Start()
    {
        // Instantiate the required number of game objects for each block type
        foreach (GameObject prefab in blockPrefabs)
        {
            string blockType = prefab.name;

            // Instantiate inactive game objects list for each block type
            inactiveGameObjects[blockType] = new Queue<GameObject>();

            for (int i = 0; i < 2; i++) // 혹여나 중복호출되어 부족한걸 방지하기위해 2개씩 준비한다. 
            {
                GameObject obj = InstantiateBlock(prefab);
                obj.SetActive(false);
                inactiveGameObjects[blockType].Enqueue(obj);
            }
        }
    }
    /// <summary>
    /// 큐에 재고 관리 
    /// </summary>
    /// <param name="blockType"></param>
    private void StockCheck(string blockType)
    {
        // 혹여나 더이상 해당 블록이 존재하지 않는다면, 새로 생성해서 Inactive딕셔너리에 추가해준다. 
        if (inactiveGameObjects.ContainsKey(blockType) && inactiveGameObjects[blockType].Count <= 0)
        {
            GameObject prefab = GetPrefabByBlockType(blockType);
            GameObject newObj = InstantiateBlock(prefab);
            newObj.SetActive(false);
            inactiveGameObjects[blockType].Enqueue(newObj);
        }
    }

    public void ReturnBlockToPool(GameObject block)
    {
        string blockType = block.name;

        // Deactivate the block and return it to the pool
        block.SetActive(false);
        activeGameObjects[blockType].Dequeue();
        inactiveGameObjects[blockType].Enqueue(block);
    }

    private GameObject InstantiateBlock(GameObject prefab)
    {
        GameObject newBlock = Instantiate(prefab);
        return newBlock;
    }

    private GameObject GetPrefabByBlockType(string blockType)
    {
        foreach (GameObject prefab in blockPrefabs)
        {
            if (prefab.name == blockType)
                return prefab;
        }

        return null;
    }
    /// <summary>
    /// 블럭 활성화를 위한 코드 
    /// </summary>
    /// <param name="blockType"></param>
    public void ReleaseBlock(string blockType)
    {
        GameObject releasable = inactiveGameObjects[blockType].Dequeue();
        StockCheck(blockType);

        releasable.SetActive(true);
        activeGameObjects[blockType].Enqueue(releasable);
    }

    public void SetforRelease(string blockType, Transform releasePoint)
    {
        GameObject releasable = inactiveGameObjects[blockType].Dequeue();
        StockCheck(blockType);
        releasable.transform.position = releasePoint.position;
        releasable.SetActive(true);
        activeGameObjects[blockType].Enqueue(releasable);
    }

    public void TriggerRelease()
    {
        Trigger?.Invoke();
    }
}