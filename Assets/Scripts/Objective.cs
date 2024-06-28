using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public string description;
    public List<Clue> requiredClues = new List<Clue>();
    public List<Clue> selectedClues = new List<Clue>(); // List to store player-selected clues
    public bool IsCompleted;

    // Method to check if the objective is completed
    public bool CheckObjectiveCompletion()
    {
        foreach (Clue requiredClue in requiredClues)
        {
            // Check if the required clue's name is in the selected clues
            bool isClueSelected = selectedClues.Exists(selectedClue => selectedClue.name == requiredClue.name);

            if (!isClueSelected)
            {
                Debug.Log($"Objective failed: Required clue '{requiredClue.name}' is not selected.");
                return false;
            }

            if (requiredClue.selectedHypothesisIndex != requiredClue.correctHypothesisIndex)
            {
                Debug.Log($"Objective failed: Clue '{requiredClue.name}' has an incorrect hypothesis selected.");
                return false;
            }
        }

        return true;
    }


}