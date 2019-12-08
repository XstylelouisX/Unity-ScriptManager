//Creator:HUANG YU XIANG
//Date:2019/12/08
//Update:2019/12/08
//Source:https://www.youtube.com/watch?v=tdSmKaJvCoA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestObject : MonoBehaviour, IPooledObject {

    public float upForce = 1f;
    public float sideForce = .1f;

    //相較於Start()，接口可以重複調用
    public void OnObjectSpawn()
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce / 2f, upForce);
        float zForce = Random.Range(-sideForce, sideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = force;
    }
}
