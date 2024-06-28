using System.Collections;
using System.Collections.Generic;
using Oculus.Voice;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SpeechToTextManager : MonoBehaviour
{
    public static SpeechToTextManager Instance;
    
    [SerializeField] private AppVoiceExperience _appVoice;

    private UnityEvent<string> _currentEvent;
    
    private void Awake()
    {
        // TODO 
        Instance = this;
        _appVoice.VoiceEvents.OnFullTranscription.AddListener((transcription) =>
        {
            _currentEvent.Invoke(transcription);
            _currentEvent = null;
            _appVoice.Deactivate();
        });
    }

    public void StartRecording(UnityEvent<string> audioEvent)
    {
        _currentEvent = audioEvent;
        _appVoice.Activate();
    }
    
    public void StopRecording()
    {
        _appVoice.Deactivate();
    }
}
