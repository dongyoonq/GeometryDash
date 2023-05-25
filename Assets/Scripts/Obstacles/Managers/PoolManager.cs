using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public GameObject[] blockPrefabs; // �Ѿ� ó�� ���� ������Ʈ Ǯ�� ��� 

    // Dictionaries to store active and inactive game objects based on their types
    public Dictionary<string, Queue<GameObject>> activeGameObjects = new Dictionary<string, Queue<GameObject>>(); // Ȱ��ȭ ���� ���� 
    public Dictionary<string, Queue<GameObject>> inactiveGameObjects = new Dictionary<string, Queue<GameObject>>(); // ��Ȱ��ȭ �� ���� 

    public event UnityAction Trigger;
    private void Start()
    {
        // Instantiate the required number of game objects for each block type
        foreach (GameObject prefab in blockPrefabs)
        {
            string blockType = prefab.name;

            // Instantiate inactive game objects list for each block type
            inactiveGameObjects[blockType] = new Queue<GameObject>();

            for (int i = 0; i < 2; i++) // Ȥ���� �ߺ�ȣ��Ǿ� �����Ѱ� �����ϱ����� 2���� �غ��Ѵ�. 
            {
                GameObject obj = InstantiateBlock(prefab);
                obj.SetActive(false);
                inactiveGameObjects[blockType].Enqueue(obj);
            }
        }
    }
    /// <summary>
    /// ť�� ��� ���� 
    /// </summary>
    /// <param name="blockType"></param>
    private void StockCheck(string blockType)
    {
        // Ȥ���� ���̻� �ش� ������ �������� �ʴ´ٸ�, ���� �����ؼ� Inactive��ųʸ��� �߰����ش�. 
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
    /// ���� Ȱ��ȭ�� ���� �ڵ� 
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