using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MatchingEvidence : MonoBehaviour
{
    private int nextClueIndex = 0; // Counter to assign unique indices to clues
    
    [SerializeField] public List<Clue> allClues; // List of all clues in the scene
    [SerializeField] public List<Objective> objectives; // List of all objectives in the scene
    [SerializeField] public List<Clue> discoveredClues = new List<Clue>(); // List of discovered clues in the scene
    
    [SerializeField] private InteractableUnityEventWrapper[] clueButtons; // Predefined buttons on the main Clue page
    [SerializeField] private InteractableUnityEventWrapper[] obj1EvidenceButtonSet; // Predefined buttons on the first objective page
    [SerializeField] private InteractableUnityEventWrapper[] obj2EvidenceButtonSet; // Predefined buttons on the second objective page
    
    [SerializeField] private TabletManager tabletManager; // Reference to the TabletManager
    public UnityEvent<int> onObjectiveCompleted;
    public UnityEvent onObjectiveFailed;
    

    
    // Add new clue to discovered clues list
    public void AddToDiscoveredClues(Clue clue)
    {
        discoveredClues.Add(clue);
    }
    
    // This method processes a clue based on the given indices
    public void ProcessClueByIndex(int clueIndex, int hypothesisIndex)
    {
        if (clueIndex >= 0 && clueIndex < discoveredClues.Count)
        {
            Clue clue = discoveredClues[clueIndex];
            clue.selectedHypothesisIndex = hypothesisIndex;
            
            Debug.Log($"Clue {clueIndex} processed with hypothesis {hypothesisIndex}");
        }
        else
        {
            Debug.LogWarning("Invalid clue index");
        }
    }

    // This method confirms the completion status of an objective based on its index
    public void ConfirmObjectiveByIndex(int objectiveIndex)
    {
        if (objectiveIndex >= 0 && objectiveIndex < objectives.Count)
        {
            Objective objective = objectives[objectiveIndex];
            Debug.Log($"Selected clues for objective {objectiveIndex}: {string.Join(", ", objective.selectedClues.Select(c => c.index))}");
            
            foreach (Clue clue in objective.selectedClues)
            {
                Debug.Log($"Selected Clue: {clue.index}, Hypothesis: {clue.selectedHypothesisIndex}");
            }
            
            bool isConfirmed = objective.CheckObjectiveCompletion();

            if (isConfirmed)
            {
                Debug.Log("Objective" + objectiveIndex + " complete: ");
                onObjectiveCompleted.Invoke(objectiveIndex);
            }
            else
            {
                Debug.Log("Objective" + objectiveIndex + " failed - Review evidence");
                onObjectiveFailed.Invoke();
            }
            
        }
        else
        {
            Debug.LogWarning("Invalid objective index");
        }
    }

    // This method returns the next available clue index and increments the counter
    public int GetNextClueIndex()
    {
        return nextClueIndex++;
    }
    
    // This method adds a discovered clue to the tablet's clue windows list
    public void AddClueToWindows(GameObject tabletWindow)
    {
        tabletManager.clueWindows.Add(tabletWindow);
    }

    // This method assigns a discovered clue to a button at its index
    public void AssignClueToButton(Clue clue)
{
    int clueIndex = clue.index;

    // Ensure the clueIndex is within bounds
    if (clueIndex >= 0 && clueIndex < clueButtons.Length)
    {
        // Activate the clue button
        clueButtons[clueIndex].gameObject.SetActive(true);
        
        // Set the clue page thumbnail
        clueButtons[clueIndex].GetComponentInChildren<SpriteRenderer>().sprite = clue.thumbnailIcon;

        // Activate the evidence page button
        obj1EvidenceButtonSet[clueIndex].gameObject.SetActive(true);
        
        // Set the evidence page thumbnail
        obj1EvidenceButtonSet[clueIndex].GetComponentInChildren<SpriteRenderer>().sprite = clue.thumbnailIcon;
        
        obj2EvidenceButtonSet[clueIndex].gameObject.SetActive(true);
        
        // Set the evidence page thumbnail
        obj2EvidenceButtonSet[clueIndex].GetComponentInChildren<SpriteRenderer>().sprite = clue.thumbnailIcon;

        // Add a listener to the clue page button to open the clue window when selected
        clueButtons[clueIndex].WhenSelect.AddListener(() => tabletManager.OpenClueWindow(clueIndex));

        // Add a listener to the evidence pages button to toggle clue selection
        obj1EvidenceButtonSet[clueIndex].WhenSelect.AddListener(() =>
        {
            clue.ToggleSelection(obj1EvidenceButtonSet[clueIndex]);
            UpdateSelectedClues(clue, objectives[0]);
        });

        obj2EvidenceButtonSet[clueIndex].WhenSelect.AddListener(() =>
        {
            clue.ToggleSelection(obj2EvidenceButtonSet[clueIndex]);
            UpdateSelectedClues(clue, objectives[1]);
        });

        Debug.Log("Button assigned for clue index: " + clueIndex);
    }
    else
    {
        Debug.LogError("Clue index out of bounds: " + clueIndex);
    }
}

private void UpdateSelectedClues(Clue clue, Objective objective)
{
    if (clue.IsSelected())
    {
        if (!objective.selectedClues.Contains(clue))
        {
            objective.selectedClues.Add(clue);
            Debug.Log($"Clue {clue.index} added to selected clues for objective: {objective.name}");
        }
        else
        {
            Debug.Log($"Clue {clue.index} is already in selected clues for objective: {objective.name}");
        }
    }
    else
    {
        if (objective.selectedClues.Contains(clue))
        {
            objective.selectedClues.Remove(clue);
            Debug.Log($"Clue {clue.index} removed from selected clues for objective: {objective.name}");
        }
        else
        {
            Debug.Log($"Clue {clue.index} was not in selected clues for objective: {objective.name}");
        }
    }
}

    
}
