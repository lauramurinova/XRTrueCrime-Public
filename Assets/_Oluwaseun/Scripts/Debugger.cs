using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public GameObject carton;

    public Transform spawnPos;

    public void DebugLog1(string inside)
    {
        Debug.LogError($"{inside}");
    }
    
    private void SpawnCarton()
    {
        Instantiate(carton, spawnPos.position, Quaternion.identity);
    }
}
