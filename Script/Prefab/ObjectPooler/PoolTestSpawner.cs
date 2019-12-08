//Creator:HUANG YU XIANG
//Date:2019/12/08
//Update:2019/12/08
//Source:https://www.youtube.com/watch?v=tdSmKaJvCoA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestSpawner : MonoBehaviour {

    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    private void FixedUpdate()
    {
        objectPooler.SpawnFromPool("Cube", transform.position, Quaternion.identity);
    }
}
