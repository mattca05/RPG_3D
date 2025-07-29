using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TooltipManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject tooltipUI;
    public TextMeshProUGUI nameText;

    [Header("Detection Settings")]
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);
    public Vector3 boxOffset = new Vector3(0f, 0f, 1.5f);
    public LayerMask tooltipLayers; // Tooltip + Interactable

    private List<InteractableObject> detectedObjects = new List<InteractableObject>();

    private void Start()
    {
        tooltipUI.SetActive(false);
    }

    private void Update()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
        Collider[] hits = Physics.OverlapBox(boxCenter, boxSize / 2f, transform.rotation, tooltipLayers);

        detectedObjects.Clear();

        foreach (Collider hit in hits)
        {
            // Skip Terrain
            if (hit.GetComponent<Terrain>() != null) continue;

            // Get InteractableObject (parent-safe for animated prefabs)
            var interactable = hit.GetComponentInParent<InteractableObject>();
            if (interactable != null)
            {
                detectedObjects.Add(interactable);
            }
        }

        if (detectedObjects.Count > 0)
        {
            tooltipUI.SetActive(true);
            InteractableObject closest = GetClosest(detectedObjects);
            nameText.text = closest.GetItemName();
        }
        else
        {
            tooltipUI.SetActive(false);
        }
    }

    private InteractableObject GetClosest(List<InteractableObject> objects)
    {
        float closestDistance = Mathf.Infinity;
        InteractableObject closest = null;

        foreach (var obj in objects)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = obj;
            }
        }

        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, boxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}