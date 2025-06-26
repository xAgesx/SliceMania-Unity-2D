using UnityEngine;

public class SenconadryTouchTest : MonoBehaviour {

    private TouchControls input;

    void Awake() {
        input = new TouchControls();
    }

    void OnEnable() {
        input.Touch.Enable();

    }
    void Oisable() {
        input.Touch.Disable();
    }

    void Update() {
        if (input.Touch.SecondaryContact.ReadValue<float>() > 0) {
            Debug.Log("Secondary clicked");
        }
    }
}
