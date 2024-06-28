using System;
using System.Collections.Generic;
using UnityEngine;

public class AnsweringMachine : MonoBehaviour
{
    // messages on the answering machine
    private List<Tuple<string, Voices>> _messages = new List<Tuple<string, Voices>>
    {
        new Tuple<string, Voices>(
            "Alex, it's Dylan. Listen up, man." +
            " I'm getting real tired of waiting" +
            " for that money you owe me. You think" +
            " you can just ignore me and sweep it" +
            " under the rug? Nah, it doesn't work" +
            " like that. You better cough up what" +
            " you owe, and you better do it fast." +
            " I'm not messing around here. You don't" +
            " want to see what happens if you keep" +
            " dodging me. Call me back, and let's " +
            "settle this. You've been warned.",
            Voices.Dylan
        )
    };

    private int _messageIndex = -1;
    private string _noMessage = "There are no messages left";
    
    /// <summary>
    /// Plays the next message defined, if none exists, no message is triggered.
    /// </summary>
    public void PlayNextMessage()
    {
        _messageIndex++;
        if (_messageIndex >= _messages.Count)
        {
            SpeechManager.Instance.Speak(_noMessage, Voices.AnsweringMachine);
        }
        else
        {
            SpeechManager.Instance.Speak(_messages[_messageIndex].Item1, _messages[_messageIndex].Item2);
        }
    }

    /// <summary>
    /// Add another message to the machine.
    /// </summary>
    public void AddMessage(string message, Voices voice)
    {
        _messages.Add(new Tuple<string, Voices>(message, voice));
    }
}
