using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public enum Voices
{
    Detective, Dylan, AnsweringMachine
}

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager Instance;
    
    [SerializeField] private TTSSpeaker _detective;
    [SerializeField] private TTSSpeaker _dylan;
    [SerializeField] private TTSSpeaker _answeringMachine;

    private TTSSpeaker _currentSpeaker;

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
    
    /// <summary>
    /// Initiates Text to Speech by WIT.AI.
    /// </summary>
    public  void Speak(string text, Voices voice)
    {
        SetSpeaker(voice);
        _currentSpeaker.Speak(text);
    }

    public void StopSpeaker()
    {
        _currentSpeaker.Stop();
    }

    /// <summary>
    /// Sets the current speaker based on the given voice (enum type).
    /// </summary>
    private void SetSpeaker(Voices voice)
    {
        switch (voice)
        {
            case Voices.Detective:
            {
                _currentSpeaker = _detective;
                break;
            }
            case Voices.Dylan:
            {
                _currentSpeaker = _dylan;
                break;
            }
            case Voices.AnsweringMachine:
            {
                _currentSpeaker = _answeringMachine;
                break;
            }
        }
    }
}
