using Assets;
using Oculus.Interaction;
using UnityEngine;

public class CrimeManager : MonoBehaviour
{
   public static CrimeManager Instance;

   [SerializeField] private PhoneManager _userPhone;
   [SerializeField] private GameObject _tutorial;
   [SerializeField] private Transform _uiPosition;
   [SerializeField] private GameObject[] _objective1Objects;
   [SerializeField] private GameObject[] _objective2Objects;
   [SerializeField] private MatchingEvidence _matchingEvidence;

   private string _currentText = "";
   private int _currentObjective = 0;
   
   private string _policeCallText = "Hi user, detective Morgan Reynolds here. Great that you came to the scene so quick." +
                                                     "Looks like a gunshot wound to the head." +
                                                     " Time of death is estimated between 8:30 and 9:00 PM." +
                                                     " We found a suicide note at the scene, it's on the table. It says he was deep" +
                                                     " in debt and couldn't take it anymore. That's what it looks" +
                                                     " like on the surface, but something feels off. I need you to" +
                                                     " help me rule out any foul play. We need to make sure this" +
                                                     " wasn't staged. I want to be thorough, that's why you should find 3 clues to prove your point. Thank you for taking care of this. See you.";

   private string _firstObjectiveDone =
      "Great job so far. What looked like a suicide is now a murder case. I sent you files of suspects" +
      " their interrogations, that you can listen to on the recorder. You have to find undeniable evidence of the real murderer." +
      "You were a real pro so far, I trust you gonna find out who it was. Dont forget to select all 5 evidences on your tablet" +
      "before concluding who it was.";

   private string _finishedGame =
      "You found the murderer. We didn't even had Chloe as a suspect, but you got this. Thank you for finding justice for Alex.";

   /// <summary>
   /// Create singleton of the manager.
   /// </summary>
   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      else if (Instance != this)
      {
         Destroy(gameObject);
      }
   }

   private void Start()
   {
      EnableArray(_objective1Objects, false);
      EnableArray(_objective2Objects, false);
      
      // _tutorial.transform.position = _uiPosition.position;
      // _tutorial.transform.rotation = _uiPosition.rotation;
      // _tutorial.SetActive(true);
      _matchingEvidence.onObjectiveCompleted.AddListener(objectiveIndex => 
      {
         Debug.Log("HERE "  + _currentObjective);
         if (objectiveIndex == 0)
         {
            Debug.Log("HERE");
            _currentObjective = objectiveIndex + 1;
            FirstObjectiveDone();
         }
         else if(objectiveIndex == 1)
         {
            _currentObjective = objectiveIndex + 1;
            CaseSolved();
         }
      });
   }

   public void DisableTutorial()
   {
      _currentText = _policeCallText;
      _userPhone.ReceiveCall();
      _tutorial.gameObject.SetActive(false);
   }

   public void FirstObjectiveDone()
   {
      _currentText = _firstObjectiveDone;
      _userPhone.ReceiveCall();
   }
   
   public void CaseSolved()
   {
      _currentObjective = -1;
      _currentText = _finishedGame;
      _userPhone.ReceiveCall();
   }

   public void FinishCall()
   {
      if (_currentObjective == 0)
      {
         EnableArray(_objective1Objects, true);
      }
      else if (_currentObjective == 1)
      {
         EnableArray(_objective2Objects, true);
      }
   }

   private void EnableArray(GameObject[] objects, bool enableObjects)
   {
      foreach (var obj in objects)
      {
         obj.SetActive(enableObjects);
      }
   }

   public void InitializePolicePhoneCall()
   {
      _tutorial.gameObject.SetActive(false);
      SpeechManager.Instance.Speak(_currentText, Voices.Detective);
   }
}
