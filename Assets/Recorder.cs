using TMPro;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField] private AudioClip[] _interrogationClips;
    [SerializeField] private AudioSource _recorderAudioSource;
    [SerializeField] private TextMeshPro _screenText;

    private int _currentRecordingIndex = 0;
    
    public void PlayNextRecording()
    {
        if (_currentRecordingIndex >= (_interrogationClips.Length - 1))
        {
            _currentRecordingIndex = 0;
        }
        else
        {
            _currentRecordingIndex++;
        }

        SetClip();
    }

    public void Play()
    {
        _recorderAudioSource.Play();
    }

    public void Stop()
    {
        _recorderAudioSource.Pause();
    }

    private void SetClip()
    {
        _recorderAudioSource.Stop();
        _recorderAudioSource.clip = _interrogationClips[_currentRecordingIndex];
        _screenText.text = "Interrogation 22 May, 2024\n\n<color=#FFFFFF>" +
                           _interrogationClips[_currentRecordingIndex].name.Replace('_', ' ') + "</color>";
    }
}
