using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class Pinch : MonoBehaviour
{
    public TouchControls touchControls;
    public Slice slice;
    private Camera mainCamera;
    private Vector2 prevPrimaryPos, prevSecondaryPos;
    private float pinchThreshold = 0.9f; // 10% reduction in distance for pinch

    [SerializeField] private float maxMergeDistance = 1f; // Max distance for adjacency
    [SerializeField] private float touchRadius = 0.5f;    // Radius to detect items

    void Awake()
    {
        touchControls = new TouchControls();
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    void OnEnable()
    {
        touchControls.Enable();
        touchControls.Touch.PrimaryContact.started += ctx => CheckPinch();
        touchControls.Touch.SecondaryContact.started += ctx => CheckPinch();
    }

    void OnDisable()
    {
        touchControls.Disable();
        touchControls.Touch.PrimaryContact.started -= ctx => CheckPinch();
        touchControls.Touch.SecondaryContact.started -= ctx => CheckPinch();
    }

    void Update()
    {
        if (touchControls.Touch.PrimaryContact.ReadValue<float>() > 0 && touchControls.Touch.SecondaryContact.ReadValue<float>() > 0)
        {
            Vector2 primaryPos = touchControls.Touch.PrimaryPos.ReadValue<Vector2>();
            Vector2 secondaryPos = touchControls.Touch.SecondaryPos.ReadValue<Vector2>();
            if (IsValidScreenPosition(primaryPos) && IsValidScreenPosition(secondaryPos))
            {
                prevPrimaryPos = primaryPos;
                prevSecondaryPos = secondaryPos;
            }
            else
            {
                Debug.LogWarning($"Invalid touch positions - Primary: {primaryPos}, Secondary: {secondaryPos}");
            }
        }
    }

    private void CheckPinch()
    {
        if (touchControls.Touch.PrimaryContact.ReadValue<float>() > 0 && touchControls.Touch.SecondaryContact.ReadValue<float>() > 0)
        {
            Vector2 primaryPos = touchControls.Touch.PrimaryPos.ReadValue<Vector2>();
            Vector2 secondaryPos = touchControls.Touch.SecondaryPos.ReadValue<Vector2>();

            if (!IsValidScreenPosition(primaryPos) || !IsValidScreenPosition(secondaryPos))
            {
                Debug.LogWarning($"Invalid pinch positions - Primary: {primaryPos}, Secondary: {secondaryPos}");
                return;
            }

            Vector2 primaryWorldPos = Utils.screenToWorld(mainCamera, primaryPos);
            Vector2 secondaryWorldPos = Utils.screenToWorld(mainCamera, secondaryPos);
            Debug.Log($"Primary World: {primaryWorldPos}, Secondary World: {secondaryWorldPos}");

            float prevDistance = Vector2.Distance(prevPrimaryPos, prevSecondaryPos);
            float currDistance = Vector2.Distance(primaryPos, secondaryPos);
            Debug.Log($"Prev Distance: {prevDistance}, Curr Distance: {currDistance}");

            if (prevDistance > 0 && currDistance < prevDistance * pinchThreshold)
            {
                Debug.Log("Pinch detected");
                GameObject item1 = GetItemAtPosition(primaryWorldPos);
                GameObject item2 = GetItemAtPosition(secondaryWorldPos);
                Debug.Log($"Item1: {item1?.name}, Item2: {item2?.name}");

                if (item1 != null && item2 != null)
                {
                    if (AreItemsAdjacent(item1, item2) && AreItemsIdentical(item1, item2))
                    {
                        MergeItems(item1, item2);
                        Debug.Log("Items merged");
                    }
                }
            }
        }
    }

    private bool IsValidScreenPosition(Vector2 screenPos)
    {
        return screenPos.x >= 0 && screenPos.x <= Screen.width &&
               screenPos.y >= 0 && screenPos.y <= Screen.height &&
               !float.IsInfinity(screenPos.x) && !float.IsInfinity(screenPos.y) &&
               !float.IsNaN(screenPos.x) && !float.IsNaN(screenPos.y);
    }

    public GameObject GetItemAtPosition(Vector2 worldPos)
    {
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

    private void MergeItems(GameObject item1, GameObject item2)
    {   
        Debug.Log("Merged");
        Vector3 mergePos = (item1.transform.position + item2.transform.position) / 2;
        Destroy(item1);
        Destroy(item2);
        SpawnMergedItem(mergePos,item1);
    }

    private void SpawnMergedItem(Vector3 position, GameObject item1) {
        var candyPrefabs = slice.getCandyPrefabs();
        int index = candyPrefabs.IndexOf(item1.gameObject) + 2;
        Debug.Log(index);
        var mergedItem = Instantiate(candyPrefabs[index], position, Quaternion.Euler(0, 0, 0));
        Debug.Log(mergedItem);
        
    }
}
