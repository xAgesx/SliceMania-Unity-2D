
using UnityEngine;

public class RGB : MonoBehaviour {

    [SerializeField] private float hue;
    [SerializeField] private float speed;
    public float x1, x2;


    // Update is called once per frame
    void Update() {
        hue = (hue + Time.deltaTime * speed) % 1;
        Color rgb = Color.HSVToRGB(hue, x1, x2);
        GetComponent<SpriteRenderer>().color = rgb;
    }
}
