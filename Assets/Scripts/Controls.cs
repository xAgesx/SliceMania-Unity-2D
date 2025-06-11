
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour{

    private Touchscreen touchscreen;
    private Vector2 initialTouchPos;
    private float sliceThreshold = 20;

    private void OnEnable() {
        // Get the touchscreen device
        touchscreen = Touchscreen.current;
        if (touchscreen == null) {
            Debug.LogError("No touchscreen detected!");
            return;
        }

        // Enable direct reading from touchscreen
        InputSystem.EnableDevice(touchscreen);
    }

    private void Update() {
        if (touchscreen == null) return;
        Vector2 touchPos = touchscreen.primaryTouch.position.ReadValue();

        // Check for touch/click
        if (touchscreen.primaryTouch.press.wasPressedThisFrame) {
            Debug.Log($"Touch at: {touchPos} (Screen coordinates)");
            initialTouchPos = touchPos;


        } else if(touchscreen.primaryTouch.press.wasReleasedThisFrame){
            if ((touchPos - initialTouchPos).magnitude > sliceThreshold) {
            TrySlice(touchPos);
        }
        }
        
    }

    private void TrySlice(Vector2 screenPos){
        
            
                Debug.Log("Slice !");
            
        
    }
}