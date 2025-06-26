using UnityEngine;
using UnityEngine.InputSystem;

public class MergeController : MonoBehaviour {
    [SerializeField] private InputAction primaryContact; // Detects first touch
    [SerializeField] private InputAction primaryPos;    // Position of first touch
    [SerializeField] private InputAction secondaryContact; // Detects second touch
    [SerializeField] private InputAction secondaryPos;    // Position of second touch
    [SerializeField] private float maxMergeDistance = 1f; // Max distance for adjacency
    [SerializeField] private float touchRadius = 0.5f;    // Radius to detect items
    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void OnEnable() {
        primaryContact.Enable();
        primaryPos.Enable();
        secondaryContact.Enable();
        secondaryPos.Enable();
    }

    private void OnDisable() {
        primaryContact.Disable();
        primaryPos.Disable();
        secondaryContact.Disable();
        secondaryPos.Disable();
    }

    private void Update() {
        if (primaryContact.ReadValue<float>() > 0 && secondaryContact.ReadValue<float>() > 0) {
            Vector2 primaryTouchPos = primaryPos.ReadValue<Vector2>();
            Vector2 secondaryTouchPos = secondaryPos.ReadValue<Vector2>();
            Vector2 primaryWorldPos = mainCamera.ScreenToWorldPoint(primaryTouchPos);
            Vector2 secondaryWorldPos = mainCamera.ScreenToWorldPoint(secondaryTouchPos);

            GameObject item1 = GetItemAtPosition(primaryWorldPos);
            GameObject item2 = GetItemAtPosition(secondaryWorldPos);

            if (item1 != null && item2 != null) {
                if (AreItemsAdjacent(item1, item2) && AreItemsIdentical(item1, item2)) {
                    MergeItems(item1, item2);
                }
            }
        }
    }

    private GameObject GetItemAtPosition(Vector2 worldPos) {
        Collider2D hit = Physics2D.OverlapCircle(worldPos, touchRadius, LayerMask.GetMask("Items"));
        return hit != null ? hit.gameObject : null;
    }

    private bool AreItemsAdjacent(GameObject item1, GameObject item2) {
        float distance = Vector2.Distance(item1.transform.position, item2.transform.position);
        return distance <= maxMergeDistance;
    }

    private bool AreItemsIdentical(GameObject item1, GameObject item2) {
        Item item1Data = item1.GetComponent<Item>();
        Item item2Data = item2.GetComponent<Item>();
        return item1Data != null && item2Data != null && item1Data.itemType == item2Data.itemType;
    }

    private void MergeItems(GameObject item1, GameObject item2) {
        Vector3 mergePos = (item1.transform.position + item2.transform.position) / 2;
        Destroy(item1);
        Destroy(item2);
        SpawnMergedItem(mergePos); // Simplified spawn
    }

    private void SpawnMergedItem(Vector3 position) {
        // Placeholder: Instantiate a basic merged item (e.g., Lollipop)
        GameObject mergedItem = GameObject.CreatePrimitive(PrimitiveType.Cube); // Replace with prefab
        mergedItem.transform.position = position;
        mergedItem.name = "MergedItem";
    }
}