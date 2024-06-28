using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StickyNoteBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    
    private Vector3 stuckRotation = new Vector3(-90, 0, 0); 
    private float moveDuration = 0.1f;

    private Rigidbody rb;
    private Coroutine moveCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb.isKinematic = false;
        // rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ListenAudio()
    {
        UnityEvent<string> audioEvent = new UnityEvent<string>();
        SpeechToTextManager.Instance.StartRecording(audioEvent);
        audioEvent.AddListener(recognizedText =>
        {
            _text.text = recognizedText;
        });
    }

    public void StopAudio()
    {
        SpeechToTextManager.Instance.StopRecording();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ContactPoint contact = collision.contacts[0];
            
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            
            moveCoroutine = StartCoroutine(MoveToStuckPosition(contact.point, Quaternion.LookRotation(-contact.normal)));
        }
    }

    private IEnumerator MoveToStuckPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        rb.constraints = RigidbodyConstraints.None;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Quaternion finalRotation = Quaternion.Euler(stuckRotation.x, targetRotation.eulerAngles.y, stuckRotation.z);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            transform.rotation = Quaternion.Slerp(startRotation, finalRotation, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        transform.rotation = finalRotation;
        
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
