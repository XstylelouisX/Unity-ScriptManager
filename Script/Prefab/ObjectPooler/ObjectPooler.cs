//Creator:HUANG YU XIANG
//Date:2019/12/08
//Update:2019/12/08
//Source:https://www.youtube.com/watch?v=tdSmKaJvCoA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    //顯示在編輯器
    [System.Serializable]
    public class Pool
    {
        public string tag; //標籤
        public GameObject prefab; //物件
        public int size; //最大生成數量
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    /// <summary>
    /// Create prefab(建立物件)
    /// </summary>
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //建立物件並加入至佇列尾端
            for (int i = 0; pool.size > i; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            //加入字典
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Use prefab(使用物件)
    /// </summary>
    /// <param name="tag">物件標籤</param>
    /// <param name="position">物件位置</param>
    /// <param name="rotation">物件旋轉</param>
    /// <returns>物件本身</returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            print("物件池不存在" + tag + "的標籤");
            return null;
        }

        //取出字典中第一個
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        //是繼承接口的物件才調用方法
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        //取出後排至尾端
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
