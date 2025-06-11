using UnityEngine;
using UnityEngine.UIElements;

public class Utils : MonoBehaviour {

    public static Vector3 screenToWorld(Camera camera, Vector3 pos) {
        pos.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(pos);
    }
}
