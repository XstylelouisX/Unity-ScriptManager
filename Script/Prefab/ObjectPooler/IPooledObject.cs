//Creator:HUANG YU XIANG
//Date:2019/12/08
//Update:2019/12/08
//Source:https://www.youtube.com/watch?v=tdSmKaJvCoA
using UnityEngine;

//所有從這個接口派生的對象都必須實現(可取代Start()呼叫物件的一次性)
public interface IPooledObject {

    void OnObjectSpawn();
}
