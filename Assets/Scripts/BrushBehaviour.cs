using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushBehaviour : MonoBehaviour
{
    [SerializeField] private float fingerprintAlpha;
    [SerializeField] private float alphaChangeDuration;
    [SerializeField] private LayerMask fingerprintLayer;
    [SerializeField] private LayerMask powderContainerLayer;
    [SerializeField] private Material brushMaterial;
    [SerializeField] private string activatedBrushHexColor = "D7D7D7";
    [SerializeField] private string initialBrushHexColor = "1B1B1B";

    private bool isBrushActivated = false;
    private bool isBrushColorChanged = false;
    private List<GameObject> fingerprints = new List<GameObject>();
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    private void Start()
    {
        fingerprints.AddRange(GameObject.FindGameObjectsWithTag("Fingerprint"));
        MakeAllFingerprintsInvisible();
        ChangeBrushColor(initialBrushHexColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the brush touches the powder container
        if (IsInLayerMask(other.gameObject.layer, powderContainerLayer))
        {
            isBrushActivated = true;
            if (!isBrushColorChanged)
            {
                ChangeBrushColor(activatedBrushHexColor);
                isBrushColorChanged = true;
            }
        }
        // Check if the brush touches a fingerprint
        else if (isBrushActivated && IsInLayerMask(other.gameObject.layer, fingerprintLayer))
        {
            if (!activeCoroutines.ContainsKey(other.gameObject))
            {
                activeCoroutines[other.gameObject] = StartCoroutine(ChangeFingerprintAlphaOverTime(other.gameObject, alphaChangeDuration));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stop the alpha change when the brush stops touching the fingerprint
        if (IsInLayerMask(other.gameObject.layer, fingerprintLayer))
        {
            if (activeCoroutines.ContainsKey(other.gameObject))
            {
                StopCoroutine(activeCoroutines[other.gameObject]);
                activeCoroutines.Remove(other.gameObject);
            }
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    private IEnumerator ChangeFingerprintAlphaOverTime(GameObject fingerprint, float duration)
    {
        SpriteRenderer spriteRenderer = fingerprint.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color currentColor = spriteRenderer.color;
            float startAlpha = currentColor.a;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float newAlpha = Mathf.Lerp(startAlpha, fingerprintAlpha, elapsed / duration);
                currentColor.a = newAlpha;
                spriteRenderer.color = currentColor;

                elapsed += Time.deltaTime;
                yield return null;
            }

            currentColor.a = fingerprintAlpha;
            spriteRenderer.color = currentColor;
            activeCoroutines.Remove(fingerprint);
        }
    }

    private void MakeAllFingerprintsInvisible()
    {
        foreach (GameObject fingerprint in fingerprints)
        {
            SetFingerprintAlpha(fingerprint, 0f);
        }
    }

    private void SetFingerprintAlpha(GameObject fingerprint, float alpha)
    {
        SpriteRenderer spriteRenderer = fingerprint.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color currentColor = spriteRenderer.color;
            currentColor.a = alpha;
            spriteRenderer.color = currentColor;
        }
    }

    private void ChangeBrushColor(string hexColor)
    {
        if (brushMaterial != null && ColorUtility.TryParseHtmlString("#" + hexColor, out Color color))
        {
            brushMaterial.color = color;
        }
    }
}