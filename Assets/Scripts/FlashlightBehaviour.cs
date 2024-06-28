using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask bloodstainLayer;
    [SerializeField] private float raycastRange = 10f;
    [SerializeField] private float checkInterval = 0.1f;
    [SerializeField] float bloodstainAlhpha;
    [SerializeField] private Light flashlightLight;
    
    private List<GameObject> bloodstains = new List<GameObject>();

    private GameObject _currentBloodStain;

    private void Start()
    {
        bloodstains.AddRange(GameObject.FindGameObjectsWithTag("Bloodstain"));
        StartCoroutine(CheckBloodstainsVisibility());
    }

    private IEnumerator CheckBloodstainsVisibility()
    {
        while (true)
        {
            ToggleBloodstainsVisibility();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void ToggleBloodstainsVisibility()
    {
        if (flashlightLight != null && flashlightLight.enabled)
        {
            var transform1 = transform;
            Ray ray = new Ray(transform1.position, transform1.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastRange, bloodstainLayer))
            {
                _currentBloodStain = hit.collider.gameObject;
                SetBloodstainAlpha(hit.collider.gameObject, bloodstainAlhpha);
            }
            else
            {
                MakeAllBloodstainsInvisible();
            }
        }
        else
        {
            MakeAllBloodstainsInvisible();
        }
    }

    private void SetBloodstainAlpha(GameObject bloodstain, float alpha)
    {
        Renderer renderer = bloodstain.GetComponent<Renderer>();
        if (renderer != null && renderer is SpriteRenderer)
        {
            SpriteRenderer spriteRenderer = renderer as SpriteRenderer;
            Color currentColor = spriteRenderer.color;
            currentColor.a = alpha;
            spriteRenderer.color = currentColor;
        }
        else if (renderer != null)
        {
            Color currentColor = renderer.material.color;
            currentColor.a = alpha;
            renderer.material.color = currentColor;
        }
    }

    private void MakeAllBloodstainsInvisible()
    {
        if (_currentBloodStain)
        {
            SetBloodstainAlpha(_currentBloodStain, 0f);
        }

        foreach (GameObject bloodstain in bloodstains)
        {
            SetBloodstainAlpha(bloodstain, 0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var transform1 = transform;
        Gizmos.DrawRay(transform1.position, transform1.forward * raycastRange);
    }
}
