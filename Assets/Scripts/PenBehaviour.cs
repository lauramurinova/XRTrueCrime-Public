using System.Collections.Generic;
using UnityEngine;

public class PenBehaviour : MonoBehaviour
{
    public Transform stickyNoteTransform; // Reference to the sticky note transform
    public GameObject writingStrokePrefab; // Prefab for the writing stroke

    private bool penIsTouchingStickyNote = false;
    private List<Vector3> writingStrokes = new List<Vector3>();
    private Vector3 lastPenPosition;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PenTip"))
        {
            penIsTouchingStickyNote = true;
            lastPenPosition = transform.position;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("PenTip"))
        {
            penIsTouchingStickyNote = false;
        }
    }

    void Update()
    {
        if (penIsTouchingStickyNote)
        {
            Vector3 currentPenPosition = transform.position;
            // Calculate the movement vector from last position to current position
            Vector3 movementVector = currentPenPosition - lastPenPosition;
            // Write on the sticky note
            Write(currentPenPosition, movementVector);
            lastPenPosition = currentPenPosition;
        }
    }

    void Write(Vector3 position, Vector3 direction)
    {
        // Instantiate a writing stroke object (e.g., a particle) at the position with the given direction
        GameObject writingStroke = Instantiate(writingStrokePrefab, position, Quaternion.LookRotation(direction), stickyNoteTransform);
        // Store the position of the writing stroke
        writingStrokes.Add(position);
        // Update visual feedback accordingly
        UpdateVisualFeedback(position);
    }

    void UpdateVisualFeedback(Vector3 position)
    {
        // Update the appearance of the sticky note surface to reflect the writing strokes
        // You can use shaders, texture painting, or other techniques for this
    }

    void Undo()
    {
        if (writingStrokes.Count > 0)
        {
            // Remove the last stroke from the writing data
            writingStrokes.RemoveAt(writingStrokes.Count - 1);
            // Update visual feedback accordingly
            // Note: You need to implement this method based on your visual feedback implementation
        }
    }
}
