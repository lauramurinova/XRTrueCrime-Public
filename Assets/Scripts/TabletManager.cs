using System;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabletManager : MonoBehaviour
{
    private int currentIndex = 0;
    [SerializeField] private List<GameObject> windows; // Main pages: Objectives, Clues, Evidence, Review Theory
    [SerializeField] public List<GameObject> clueWindows; // Individual clue pages
    
    [SerializeField] private InteractableUnityEventWrapper obj1Button; // Button to attempt the first objective
    [SerializeField] private InteractableUnityEventWrapper obj2Button; // Button to attempt the second objective
    
    [SerializeField] public List<GameObject> evidenceSelectionWindows; // Evidence selection pages
    [SerializeField] private InteractableUnityEventWrapper sendObj1EvidenceButton; // Button to review selected clues for the first objective
    [SerializeField] private InteractableUnityEventWrapper sendObj2EvidenceButton; // Button to review selected clues for the second objective
    
    [SerializeField] public List<GameObject> reviewPages; // Pages to review selected clues and submit hypothesis for objectives
    [SerializeField] private InteractableUnityEventWrapper obj1SubmissionButton; // Button to submit the first objective on review
    [SerializeField] private InteractableUnityEventWrapper obj2SubmissionButton; // Button to submit the first objective on review
    
    [SerializeField] private InteractableUnityEventWrapper nextImageButton; // Single button for next image
    [SerializeField] private InteractableUnityEventWrapper prevImageButton; // Single button for previous image
    [SerializeField] private InteractableUnityEventWrapper returnButton; // Single button for returning to clue page
    
    [SerializeField] private InteractableUnityEventWrapper nextPageArrowButton;
    [SerializeField] private InteractableUnityEventWrapper prevPageArrowButton; // Navigation buttons for main pages

    private ReviewEvidence reviewEvidence;
    private MatchingEvidence matchingEvidence;
    private Clue currentClue;


    private void Awake()
    {
        reviewEvidence = FindObjectOfType<ReviewEvidence>();
        matchingEvidence = FindObjectOfType<MatchingEvidence>();
    }

    void Start()
    {
        CloseAllWindows();
        CloseAllClueWindows();
        CloseAllEvidencePages();
        windows[0].SetActive(true); // Open the first window (Objectives)
        ToggleMainPageButtons(true);
        
        nextPageArrowButton.WhenSelect.AddListener(OpenNextWindow);
        prevPageArrowButton.WhenSelect.AddListener(OpenPrevWindow);
        
        obj1Button.WhenSelect.AddListener(OpenEvidenceSelectionPage1);
        obj2Button.WhenSelect.AddListener(OpenEvidenceSelectionPage2);
    }

    private void CloseAllEvidencePages()
    {
        foreach (var page in evidenceSelectionWindows)
        {
            page.SetActive(false);
        }
    }

    public void OpenEvidenceSelectionPage1()
    {
        CloseAllWindows();
        CloseAllClueWindows();
        CloseAllEvidencePages();
        ToggleCluePageNav(false);
        ToggleMainPageButtons(false);
        
        evidenceSelectionWindows[0].gameObject.SetActive(true);
        SetupEvidenceReviewButtons();
    }
    public void OpenEvidenceSelectionPage2()
    {
        CloseAllWindows();
        CloseAllClueWindows();
        CloseAllEvidencePages();
        ToggleCluePageNav(false);
        ToggleMainPageButtons(false);
        
        evidenceSelectionWindows[1].gameObject.SetActive(true);
        SetupEvidenceReviewButtons();
    }
    
    public void ShowObj1ReviewPage()
    {
        CloseAllWindows();
        CloseAllClueWindows();
        CloseAllEvidencePages();
        ToggleCluePageNav(false);
        ToggleMainPageButtons(false);
        
        SetupObjSubmissionButtons();
        
        
        // Update and populate clue hypotheses and buttons
        reviewEvidence.UpdateObj1ReviewPage();
        
        reviewPages[0].gameObject.SetActive(true);
    }
    public void ShowObj2ReviewPage()
    {
        CloseAllWindows();
        CloseAllClueWindows();
        CloseAllEvidencePages();
        ToggleCluePageNav(false);
        ToggleMainPageButtons(false);
        
        SetupObjSubmissionButtons();
        
        // Update populate clue hypothesis and buttons
        reviewEvidence.UpdateObj2ReviewPage();
        
        reviewPages[1].gameObject.SetActive(true);
    }

    public void OpenNextWindow()
    {
        // Close the current window
        windows[currentIndex].SetActive(false);

        // Update the current index
        currentIndex = (currentIndex + 1) % windows.Count;

        // Open the next window
        windows[currentIndex].SetActive(true);
    }

    public void OpenPrevWindow()
    {
        // Close the current window
        windows[currentIndex].SetActive(false);

        // Update the current index
        currentIndex = ((currentIndex - 1) + windows.Count) % windows.Count;

        // Open the previous window
        windows[currentIndex].SetActive(true);
    }
    
    

    public void OpenWindow(int index)
    {
        CloseAllWindows();
        CloseAllClueWindows();
        ToggleCluePageNav(false);
        
        windows[index].SetActive(true);
        Debug.Log("Page:"+ index +" opened");
        ToggleMainPageButtons(true);
    }

    public void OpenClueWindow(int index)
    {
        Debug.Log("Opening clue window for index: " + index);
        
        ToggleMainPageButtons(false);
        CloseAllWindows();
        CloseAllClueWindows();
        clueWindows[index].SetActive(true);
        
        ToggleCluePageNav(true);

        currentClue = clueWindows[index].GetComponent<Clue>();
        SetupClueNavigationButtons();
        ShowClueImage(0);
    }

    private void ToggleCluePageNav(bool toggle)
    {
        nextImageButton.gameObject.SetActive(toggle);
        prevImageButton.gameObject.SetActive(toggle);
        returnButton.gameObject.SetActive(toggle);
    }

    private void ToggleMainPageButtons(bool toggle)
    {
        prevPageArrowButton.gameObject.SetActive(toggle);
        nextPageArrowButton.gameObject.SetActive(toggle);
    }

    private void SetupClueNavigationButtons()
    {
        if (currentClue == null)
            return;

        nextImageButton.WhenSelect.RemoveAllListeners();
        prevImageButton.WhenSelect.RemoveAllListeners();

        nextImageButton.WhenSelect.AddListener(NextClueImage);
        prevImageButton.WhenSelect.AddListener(PreviousClueImage);
    }

    public void ShowClueImage(int index)
    {
        if (currentClue == null || index < 0 || index >= currentClue.images.Length)
            return;

        for (int i = 0; i < currentClue.images.Length; i++)
        {
            currentClue.images[i].gameObject.SetActive(i == index);
        }
    }

    public void NextClueImage()
    {
        if (currentClue == null)
            return;

        int currentIndex = GetCurrentImageIndex();
        int nextIndex = (currentIndex + 1) % currentClue.images.Length;
        ShowClueImage(nextIndex);
    }

    public void PreviousClueImage()
    {
        if (currentClue == null)
            return;

        int currentIndex = GetCurrentImageIndex();
        int previousIndex = (currentIndex - 1 + currentClue.images.Length) % currentClue.images.Length;
        ShowClueImage(previousIndex);
    }

    private int GetCurrentImageIndex()
    {
        for (int i = 0; i < currentClue.images.Length; i++)
        {
            if (currentClue.images[i].gameObject.activeSelf)
                return i;
        }
        return 0;
    }

    public void CloseAllWindows()
    {
        foreach (var win in windows)
        {
            win.SetActive(false);
        }

        foreach (var pages in reviewPages)
        {
            pages.SetActive(false);
        }
    }

    public void CloseAllClueWindows()
    {
        foreach (var win in clueWindows)
        {
            win.SetActive(false);
        }
    }

    private void SetupEvidenceReviewButtons()
    {
        sendObj1EvidenceButton.WhenSelect.AddListener(ShowObj1ReviewPage);
        sendObj2EvidenceButton.WhenSelect.AddListener(ShowObj2ReviewPage);
    }

    private void SetupObjSubmissionButtons()
    {
        
        obj1SubmissionButton.WhenSelect.AddListener(ConfirmObj1);
        obj2SubmissionButton.WhenSelect.AddListener(ConfirmObj2);
    }

    private void ConfirmObj1()
    {
        matchingEvidence.ConfirmObjectiveByIndex(0);
    }
    
    private void ConfirmObj2()
    {
        matchingEvidence.ConfirmObjectiveByIndex(1);
    }
}
