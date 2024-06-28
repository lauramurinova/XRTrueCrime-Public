using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class RoomSetup : MonoBehaviour
{
    // References to MRUK components
    private MRUK _mruk;
    private MRUKRoom _playerRoom;

    // Lists to store room anchors and wall anchors
    private List<MRUKAnchor> _playerRoomAnchors;
    private List<MRUKAnchor> _playerWalls;

    // Anchors for the player's table and the key crime wall
    private MRUKAnchor _playerTable;
    private MRUKAnchor _keyCrimeWall;

    // Position of the player's table (initialized to Vector3.zero)
    private Vector3 _playerTablePos = Vector3.zero;

    // Flags for initialization status and table presence
    private bool _isInitialized = false;
    private bool _isTableInScene;

    // Awake method runs before the first frame update
    void Awake()
    {
        // Find the MRUK component in the scene
        _mruk = FindObjectOfType<MRUK>();

        // Listen for the scene loaded event and call Initialize
        _mruk.SceneLoadedEvent.AddListener(Initialize);
    }

    // Initialize MRUK components and room information
    public void Initialize()
    {
        // Set the player's room
        _playerRoom = _mruk.GetCurrentRoom();
        _playerRoomAnchors = _playerRoom.Anchors;
        _playerWalls = _playerRoom.WallAnchors;

        // Check if a table exists in the room
        _isTableInScene = _playerRoom.DoesRoomHave(new[] { "TABLE" });
        SetPlayerTable();

        // Find the closest wall to the table
        _keyCrimeWall = FindNearestWall(_playerTablePos);
        Debug.LogError("Key crime wall: " + _keyCrimeWall);

        // Mark initialization as complete
        _isInitialized = true;
    }

    // Set the player's table anchor and position
    void SetPlayerTable()
    {
        if (!_isTableInScene) return;
        _playerTable = FindPlayerTable();
        _playerTablePos = _playerTable?.transform.position ?? Vector3.zero;

        Debug.LogError("Player table: " + _playerTable);
    }

    // Find the MRUK anchor labeled as "Table"
    MRUKAnchor FindPlayerTable()
    {
        if (_isTableInScene)
        {
            foreach (MRUKAnchor obj in _playerRoomAnchors)
            {
                List<string> labels = obj.AnchorLabels;

                // Check if 'Table' label exists
                if (labels.Contains("Table"))
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
    MRUKAnchor FindNearestWall(Vector3 position)
    {
        MRUKAnchor nearestWall = null;
        float nearestDistance = float.MaxValue;

        foreach (var wall in _playerWalls)
        {
            float distance = GetDistToPlane(position, wall);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestWall = wall;
            }
        }

        return nearestWall;
    }

    // Calculate the distance from a point to a wall plane
    private float GetDistToPlane(Vector3 position, MRUKAnchor wall)
    {
        Vector3 wallNormal = wall.transform.forward;

        float wallDistance = -Vector3.Dot(wallNormal, wall.transform.position);
        float distance = Mathf.Abs(Vector3.Dot(wallNormal, position) + wallDistance) / wallNormal.magnitude;

        return distance;
    }
}
