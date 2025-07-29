using UnityEngine;

public class PickupAndDrop : MonoBehaviour
{
    private Transform playerHoldPoint;
    private Rigidbody rb;
    private bool isHeld = false;

    [Header("Drop Settings")]
    public float dropForwardForce = 2f;  // how far forward to push the object when dropping

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Called from SelectionManager when picked up
    public void PickUp(Transform holdPoint)
    {
        isHeld = true;
        playerHoldPoint = holdPoint;

        // Disable physics and attach to player hold point
        rb.isKinematic = true;
        transform.SetParent(playerHoldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // Called from SelectionManager when dropped
    public void Drop()
    {
        isHeld = false;

        // Detach and re-enable physics
        transform.SetParent(null);
        rb.isKinematic = false;

        // Push it forward a bit
        rb.AddForce(playerHoldPoint.forward * dropForwardForce, ForceMode.Impulse);

        playerHoldPoint = null;
    }

    private void Update()
    {
        // Optional: add logic if you want to drop object by pressing E again
        // But this will be triggered via SelectionManager when object is already held
    }
}