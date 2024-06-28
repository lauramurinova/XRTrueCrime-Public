using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] private Material _redMaterial;
    [SerializeField] private GameObject _wallBloodStain;
    [SerializeField] private GameObject _tableBloodStain;
    
    // Reference to scene manager and scene requirements
    private OVRSceneManager _sceneManager;
    private string[] _sceneRequirements;

    // Lists to store room anchors and wall anchors
    private List<OVRSceneAnchor> _roomAnchors;
    
    // Anchors for the player's table and the key crime wall
    private OVRSceneAnchor _playerTable;
    private OVRSceneAnchor _keyCrimeWall;
    
    // Position of the player's table (initialized to Vector3.zero)
    private Vector3 _playerTablePos = Vector3.zero;
    
    // Bool flags
    private bool _tableExists;
    private bool _anchorsSet;
    private bool _roomSetupComplete;

    private void Awake()
    {
        SetSceneRequirements();
        _sceneManager = FindObjectOfType<OVRSceneManager>();
        
        _sceneManager.SceneModelLoadedSuccessfully += OnLoaded;
        _sceneManager.SceneCaptureReturnedWithoutError += OnSceneCaptureReturnedWithoutError;
        _sceneManager.UnexpectedErrorWithSceneCapture += OnUnexpectedErrorWithSceneCapture;
    }

    
    void SetSceneRequirements()
    {
        _sceneRequirements = new[]
        {
            OVRSceneManager.Classification.Table,
        };
        
    }
    
    void OnLoaded()
    {
        CheckTableExistence();
    }
    

    async void CheckTableExistence()
    {
        _tableExists = await _sceneManager.DoesRoomSetupExist(_sceneRequirements);

        if (_tableExists)
        {
            Debug.LogError("Table exists in the loaded scene model.");
           OnSceneCaptureReturnedWithoutError();
        }
        else
        {
            // _sceneManager.RequestSceneCapture(_sceneRequirements);
            Debug.LogError("Scene capture requested");
        }
    }

    // Handle the SceneCaptureReturnedWithoutError event
    void OnSceneCaptureReturnedWithoutError()
    {
        // User completed space setup successfully
        Debug.LogError("Space setup completed!");
        
        //Store room anchors
        SetRoomAnchors();

        _roomSetupComplete = true;
    }

    // Handle the UnexpectedErrorWithSceneCapture event
    void OnUnexpectedErrorWithSceneCapture()
    {
        // Something went wrong during space setup
        Debug.LogError("Error during space setup.");
        
        //Request scene capture
        _sceneManager.RequestSceneCapture(_sceneRequirements);
    }
    
    private void SetRoomAnchors()
    {
        // Populate anchor lists
        
        OVRSceneAnchor.GetSceneAnchors(_roomAnchors);
        
        Debug.Log(_roomAnchors);
        
        //Set player's table
        SetPlayerTable();
        
        // Find the closest wall to the table
        _keyCrimeWall = FindNearestWall(_playerTablePos);
        _keyCrimeWall.gameObject.AddComponent<BoxCollider>();
        Debug.LogError("Key crime wall: " + _keyCrimeWall);
        
        var bloodStain = Instantiate(_wallBloodStain, _keyCrimeWall.transform);
        bloodStain.transform.position = FindNearestPointOnWall(_keyCrimeWall, _playerTablePos);
        
        _anchorsSet = true;
    }
    
    Vector3 FindNearestPointOnWall(OVRSceneAnchor wallAnchor, Vector3 tablePosition)
    {
        Collider wallCollider = wallAnchor.GetComponent<Collider>();
        if (wallCollider == null)
        {
            Debug.LogError("Wall does not have a collider.");
            return wallAnchor.transform.position;
        }

        Vector3 nearestPoint = wallCollider.ClosestPoint(tablePosition);
        return nearestPoint;
    }
    
    // Set the player's table anchor and position
    void SetPlayerTable()
    {
        _playerTable = FindPlayerTable();
        _playerTablePos = _playerTable.transform.position;
        foreach (var meshRenderer in _playerTable.GetComponentsInChildren<MeshRenderer>())
        {
            if (meshRenderer.gameObject.name == "Mesh")
            {
                Debug.Log("I AM HERE");
                Instantiate(_tableBloodStain, meshRenderer.transform);
            }
        }

        Debug.LogError("Player table: " + _playerTable);
    }
    
    OVRSceneAnchor FindPlayerTable()
    {
        if (_tableExists)
        {
            foreach (OVRSceneAnchor obj in _roomAnchors)
            {
                OVRSemanticClassification classification = obj.GetComponentInChildren<OVRSemanticClassification>();
                
                Debug.Log(classification.Labels);
                // Check if 'Table' label exists
                if (classification.Labels.Contains("TABLE"))
                {
                    // Return the MRUK object labeled Table
                    return obj;
                }
            }
        }
        // If no 'Table' anchor found, return null
        return null;
    }
    
    // Find the nearest wall to a given position
    OVRSceneAnchor FindNearestWall(Vector3 position)
    {
        OVRSceneAnchor nearestWall = null;
        float nearestDistance = float.MaxValue;

        foreach (var anchor in _roomAnchors)
        {
            if(!anchor.GetComponentInChildren<OVRSemanticClassification>().Labels[0].Contains("WALL_FACE")) continue;
            
            float distance = Vector3.Distance(_playerTablePos, anchor.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestWall = anchor;
            }
        }

        return nearestWall;
    }
}