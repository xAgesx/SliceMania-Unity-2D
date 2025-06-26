using UnityEngine;

public class Destroyer : MonoBehaviour {

    int HP;
    public Slice sliceScript;

    void Awake() {
        HP = sliceScript.HP;

    }
    void OnCollisionEnter2D(Collision2D collision) {
        HP -= 1;
        Destroy(collision.gameObject);

    }
    void Update() {
        if (HP <= 0) {
            Debug.Log("Game OVER");
        }        
    }

}
