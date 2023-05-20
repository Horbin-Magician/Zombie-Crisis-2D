using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    void Start(){
        for(int i=0; i<1; ++i)
            GameObject.Instantiate(Resources.Load("Prefabs/Enemy") as GameObject, new Vector3(5f + i*0.1f, 5, 0), Quaternion.identity);
    }
}
