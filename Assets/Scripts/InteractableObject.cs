using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string itemName = "Item";
    public float dropForwardForce = 2f;

    private Rigidbody rb;
    private Transform holdPoint;
    private bool isHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public string GetItemName()
    {
        return itemName;
    }

    public void PickUp(Transform hold)
    {
        // Attach to hold point instead of disabling
        if (isHeld) return;

        isHeld = true;
        holdPoint = hold;

        // Disable physics so it doesn't fall
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Parent to the hold point and reset position
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        if (!isHeld) return;

        isHeld = false;
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // Instead of a huge force, set a small forward velocity
            rb.linearVelocity = holdPoint.forward * 3f; // adjust speed (3 m/s feels natural)
        }

        holdPoint = null;
    }
}