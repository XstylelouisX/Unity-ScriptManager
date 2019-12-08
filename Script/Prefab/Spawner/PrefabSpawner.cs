//Creator:HUANG YU XIANG
//Date:2019/12/02
//Update:2019/12/08
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour {

    public GameObject spawnObject; //生成物件
    public bool startLoad = false; //是否載入生成
    public bool loopSpawn = false; //是否循環生成
    public bool randomSpawn = false; //是否生成位置為亂數
    public int spawnNumber = 0; //生成數量
    public int spawnInterval = 0; //生成間隔
    public Transform[] spawnPos; //生成位置

    private List<GameObject> recordSpawn = new List<GameObject>(); //紀錄生成物件
    private int listCount = 0;
    private bool isSpawn = false; //是否生成

    private void Start()
    {
        if (startLoad == true)
        {
            StartSpawn();
        }
    }

    /// <summary>
    /// Initialization state(初始化狀態)
    /// </summary>
    public void StartSpawn()
    {
        isSpawn = false;
        if (loopSpawn == false)
        {
            StartCoroutine(SpawnAntidote(spawnNumber, spawnInterval));
        }
        else
        {
            StopCoroutine("CheckSpawn");
            StartCoroutine(CheckSpawn());
        }
    }

    /// <summary>
    /// Check gameobject in list(檢查清單，循環生成)
    /// </summary>
    private IEnumerator CheckSpawn()
    {
        listCount = 0;
        //print("存在數量：" + recordSpawn.Count);
        for (int i = 0; recordSpawn.Count > i; i++)
        {
            if (recordSpawn[i] == null)
            {
                listCount++;
            }
        }
        //print("不存在數量；" + listCount);
        if (recordSpawn.Count == listCount && isSpawn == false)
        {
            isSpawn = true;
            StartCoroutine(SpawnAntidote(spawnNumber, spawnInterval));
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CheckSpawn());
    }

    /// <summary>
    /// Spawn gameobject(生成物件)
    /// </summary>
    /// <param name="number">Spawn number(生成數量)</param>
    /// <param name="interval">Spawn interval(生成間隔)</param>
    private IEnumerator SpawnAntidote(int number,int interval)
    {
        DestroySpawn();
        if (number > spawnPos.Length)
        {
            print("error：設定生成數量超出現有生成位置的最大值");
            yield break;
        }
        yield return new WaitForSeconds(interval); //生成間隔
        var random = RangeManager.Instance.GetRange(number, 0, spawnPos.Length);
        for (int i = 0; number > i; i++)
        {
            if (randomSpawn == true)
            {
                //print("生成位置：" + random[i]);
                recordSpawn.Add(Instantiate(spawnObject, spawnPos[random[i]]));
            }
            else
            {
                recordSpawn.Add(Instantiate(spawnObject, spawnPos[i]));
            }
        }
        isSpawn = false;
    }

    /// <summary>
    /// Destroy gameobject(刪除物件)
    /// </summary>
    public void DestroySpawn()
    {
        //刪除引用物件
        foreach (var obj in recordSpawn)
        {
            Destroy(obj);
        }
        //清除引用清單
        recordSpawn.Clear();
    }
}
