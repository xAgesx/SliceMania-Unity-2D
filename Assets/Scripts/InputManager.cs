using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour {

    
    private TouchControls touchControls;
    private Camera mainCamera;

    #region Events
    public delegate void StartTouch(Vector2 pos, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 pos, float time);
    public event EndTouch OnEndTouch;

    #endregion
    void Awake() {
        touchControls = new TouchControls();
        mainCamera = Camera.main;

    }

    void OnEnable() {
        touchControls.Enable();
    }

    void OnDisable() {
        touchControls.Disable();
    }

    void Start() {
        touchControls.Touch.PrimaryContact.started += ctx => StartPrimaryTouch(ctx);
        touchControls.Touch.PrimaryContact.canceled += ctx => EndPrimaryTouch(ctx);
    }

    private void StartPrimaryTouch(InputAction.CallbackContext context) {
    Vector2 screenPos = touchControls.Touch.PrimaryPos.ReadValue<Vector2>();
    if (OnStartTouch != null && IsValidScreenPosition(screenPos)) {
        OnStartTouch(Utils.screenToWorld(mainCamera, screenPos), (float)context.startTime);
    }
}

private void EndPrimaryTouch(InputAction.CallbackContext context) {
    Vector2 screenPos = touchControls.Touch.PrimaryPos.ReadValue<Vector2>();
    if (OnEndTouch != null && IsValidScreenPosition(screenPos)) {
        OnEndTouch(Utils.screenToWorld(mainCamera, screenPos), (float)context.time);
    }
}

   private bool IsValidScreenPosition(Vector2 screenPos) {
    return screenPos.x >= 0 && screenPos.x <= Screen.width &&
           screenPos.y >= 0 && screenPos.y <= Screen.height;
}

public Vector2 primaryPosition() {
    Vector2 screenPos = touchControls.Touch.PrimaryPos.ReadValue<Vector2>();
    return IsValidScreenPosition(screenPos) ? 
           Utils.screenToWorld(mainCamera, screenPos) : 
           Vector2.zero;
}

    
    
}
