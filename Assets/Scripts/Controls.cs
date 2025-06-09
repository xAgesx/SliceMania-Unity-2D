using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference primaryTouchAction;
    public InputActionReference swipeAction;
    public InputActionReference pinchAction;

    [Header("Settings")]
    public float swipeThreshold = 50f;

    private void Update(){
        // Debug touchscreen status
        if (Touchscreen.current == null)
            Debug.Log("No touchscreen detected!");
        else if (Touchscreen.current.primaryTouch.press.isPressed)
            Debug.Log("Touch position: " + Touchscreen.current.primaryTouch.position.ReadValue());
        // Tap (if touch just started)
        if (primaryTouchAction.action.WasPressedThisFrame())
        {
            Vector2 touchPos = primaryTouchAction.action.ReadValue<Vector2>();
            Debug.Log("Tap at: " + touchPos);
            TrySlice(touchPos);
        }

        // Swipe (if finger moved)
        Vector2 swipeDelta = swipeAction.action.ReadValue<Vector2>();
        if (swipeDelta.magnitude > swipeThreshold)
        {
            Vector2 touchPos = primaryTouchAction.action.ReadValue<Vector2>();
            Debug.Log("Swipe detected!");
            TrySlice(touchPos);
        }

        // Pinch (if two touches detected)
        if (pinchAction.action.triggered)
        {
            Vector2 touch0Pos = pinchAction.action.ReadValue<Vector2>();
            Vector2 touch1Pos = pinchAction.action.ReadValue<Vector2>();
            Debug.Log("Pinch detected!");
            TryMerge(touch0Pos, touch1Pos);
        }
    }

    private void TrySlice(Vector2 screenPos)
    {
        Debug.Log("Sliced");
    }

    private void TryMerge(Vector2 screenPos0, Vector2 screenPos1)
    {
        Debug.Log("Merged");
    }
}