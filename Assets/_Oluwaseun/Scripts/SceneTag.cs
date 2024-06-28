using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets;

public class SceneTag : MonoBehaviour
{
    public int sceneTagNo = 0;

    [SerializeField] private List<TMP_Text> _tagText; 

    // Update is called once per frame
    void Update()
    {
        AssignTagIndex();
    }
    
    private void AssignTagIndex()
    {
        ChangeSceneTagNumber(sceneTagNo);
    }

    private void ChangeSceneTagNumber(int TagNo)
    {
        foreach (var text in _tagText)
        {
            text.text = $"{TagNo}";
        }
    }
}
