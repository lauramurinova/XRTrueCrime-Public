using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class ReviewEvidence : MonoBehaviour
{
    // List of buttons to assign selected clue pages
    [SerializeField] private List<InteractableUnityEventWrapper> obj1SelectedClueButtons;
    [SerializeField] private List<InteractableUnityEventWrapper> obj2SelectedClueButtons;

    // List of text fields to assign selected hypothesis of said clues
    [SerializeField] private List<TMP_Text> obj1SelectedClueHypotthesis;
    [SerializeField] private List<TMP_Text> obj2SelectedClueHypotthesis;
    
    private MatchingEvidence matchingEvidence;
    private TabletManager tabletManager;
    
    
    void Awake()
    {
        tabletManager = FindObjectOfType<TabletManager>();
        matchingEvidence = FindObjectOfType<MatchingEvidence>();
    }
    
    public void UpdateObj1ReviewPage()
    {
        UpdateReviewPage(matchingEvidence.objectives[0], obj1SelectedClueButtons, obj1SelectedClueHypotthesis);
    }
    
    public void UpdateObj2ReviewPage()
    {
        UpdateReviewPage(matchingEvidence.objectives[1], obj2SelectedClueButtons, obj2SelectedClueHypotthesis);
    }
    
    // This method updates the review page with the selected clues and their hypotheses
    private void UpdateReviewPage(Objective objective, List<InteractableUnityEventWrapper> clueButtons, List<TMP_Text> hypothesisTexts)
    {
        // Loop through the selected clues of the objective
        for (int i = 0; i < objective.selectedClues.Count; i++)
        {
            // Get the current clue
            Clue clue = objective.selectedClues[i];
        
            // ADDED SPRITE
            clueButtons[i].GetComponentInChildren<SpriteRenderer>().sprite = clue.thumbnailIcon;
            
            // Activate the corresponding clue button
            clueButtons[i].gameObject.SetActive(true);
        
            // Remove any existing listeners to avoid duplicate listeners
            clueButtons[i].WhenSelect.RemoveAllListeners();
        
            // Capture the current clue index for proper closure in the listener
            int clueIndex = clue.index;
        
            // Add a listener to the button to visit the clue page when selected
            clueButtons[i].WhenSelect.AddListener(() => tabletManager.OpenClueWindow(clueIndex));
        
            // Update the hypothesis text with the selected hypothesis of the clue
            hypothesisTexts[i].text = clue.hypothesisOptions[clue.selectedHypothesisIndex];
            hypothesisTexts[i].gameObject.SetActive(true);
        }
    }

    
}
