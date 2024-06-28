using System;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.UI;

public class Clue : MonoBehaviour
{
    public Sprite thumbnailIcon;
    public GameObject tabletWindow;
    public int index = 100; // Unique index assigned at runtime
    public string description;
    public string[] hypothesisOptions; // Hypothesis options presented to the player
    public Image[] images; // Unique set of images for each clue
    public InteractableUnityEventWrapper[] hypothesisButtons; // Unique buttons for each clue

    public int selectedHypothesisIndex; // Player's selected hypothesis index
    public int correctHypothesisIndex; // Correct hypothesis index
    
    private bool assigned;
    public bool isSelected; // Track selection state

    private MatchingEvidence matchingEvidence;


    private void Awake()
    {
       matchingEvidence = FindObjectOfType<MatchingEvidence>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger entered by: " + other.gameObject.name);
            
            if (!assigned) // Ensure the index is only set once
            {
                SetIndex();
                assigned = true;
            } 
        }
        
    }

    private void SetIndex()
    {
        if (matchingEvidence != null)
        {
            index = matchingEvidence.GetNextClueIndex();
            Debug.Log("Clue index set to: " + index);
                
            //Inform tablet manager to clue to list of windows
            matchingEvidence.AddClueToWindows(tabletWindow);
                
            // Inform MatchingEvidence to assign this clue to a button
            matchingEvidence.AssignClueToButton(this);
            
            // Add the clue to discovered clues
            matchingEvidence.AddToDiscoveredClues(this);

            // Setup hypothesis buttons
            for (int i = 0; i < hypothesisButtons.Length; i++)
            {
                int hypothesisIndex = i;
                var button = hypothesisButtons[i];
                hypothesisButtons[i].WhenSelect.AddListener(() =>
                {
                    button.GetComponentInChildren<RoundedBoxProperties>().BorderColor = Color.green;

                    for (int i = 0; i < hypothesisButtons.Length; i++)
                    {
                        if (i == hypothesisIndex)
                        {
                            if (hypothesisButtons[i].GetComponentInChildren<RoundedBoxProperties>().BorderOuterRadius == 0)
                            {
                                hypothesisButtons[i].GetComponentInChildren<RoundedBoxProperties>().BorderOuterRadius = 0.05f;
                            }
                            else
                            {
                                hypothesisButtons[i].GetComponentInChildren<RoundedBoxProperties>().BorderOuterRadius = 0;
                            }
                        }
                        else
                        {
                            hypothesisButtons[i].GetComponentInChildren<RoundedBoxProperties>().BorderOuterRadius = 0;
                        }
                    }

                    matchingEvidence.ProcessClueByIndex(index, hypothesisIndex);
                });
            }
            
        } 
        
    }
    
    public void ToggleSelection(InteractableUnityEventWrapper clueButton)
    {
        isSelected = !isSelected;
        Debug.Log("Clue " + index + " selection toggled to: " + isSelected);
        
        // Update visual state based on selection
        clueButton.GetComponentInChildren<RoundedBoxProperties>().BorderColor = Color.green;
        clueButton.GetComponentInChildren<RoundedBoxProperties>().BorderOuterRadius = isSelected ? 0.05f : 0;
        
        Debug.Log("Color changed");
    }

    public bool IsSelected()
    {
        return isSelected;
    }
    
}