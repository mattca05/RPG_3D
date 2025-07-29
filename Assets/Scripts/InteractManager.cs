using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject interactUI;
    public TextMeshProUGUI actionText;

    [Header("Detection Settings")]
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);
    public Vector3 boxOffset = new Vector3(0f, 0f, 1.5f);
    public LayerMask pickableLayer; // Only the Pickable layer

    [Header("Pickup Settings")]
    public Transform holdPoint;

    private PlayerControls controls;
    private InteractableObject currentTarget;
    private InteractableObject heldObject;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Interact.performed += _ => HandleInteract();
    }

    private void OnDisable()
    {
        controls.Player.Interact.performed -= _ => HandleInteract();
        controls.Disable();
    }

    private void Start()
    {
        interactUI.SetActive(false);
    }

    private void Update()
    {
        // Start each frame with UI hidden
        interactUI.SetActive(false);

        // Show Drop UI if holding something
        if (heldObject != null)
        {
            interactUI.SetActive(true);
            actionText.text = "[E] Drop";
            return;
        }

        // Detection box center
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);

        // Check only Pickable layer
        Collider[] hits = Physics.OverlapBox(
            boxCenter,
            boxSize / 2f,
            transform.rotation,
            pickableLayer
        );

        InteractableObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {

            // Get interactable
            var interactable = hit.GetComponentInParent<InteractableObject>();
            if (interactable != null)
            {
                float dist = Vector3.Distance(transform.position, interactable.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = interactable;
                }
            }
        }

        // If we found a valid target, show Pick up text
        if (closest != null)
        {
            currentTarget = closest;
            interactUI.SetActive(true);
            actionText.text = "[E] Pick up";
        }
        else
        {
            // Explicitly clear state when no valid pickable object
            currentTarget = null;
        }
    }

    private void HandleInteract()
    {
        if (heldObject != null)
        {
            heldObject.Drop();
            heldObject = null;
        }
        else if (currentTarget != null)
        {
            currentTarget.PickUp(holdPoint);
            heldObject = currentTarget;
        }
    }
}