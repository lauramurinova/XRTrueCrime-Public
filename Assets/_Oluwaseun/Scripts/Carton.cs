using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
public class Carton : MonoBehaviour
{
    [SerializeField] private GameObject _tagPile;
    [SerializeField] private Transform _spawnPos;
    // Start is called before the first frame update
    private void OnEnable()
    {
        var TagPile = Instantiate(_tagPile, _spawnPos.position, Quaternion.identity);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
