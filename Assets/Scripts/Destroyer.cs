using TMPro;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    public GameManager gm;
    void OnCollisionEnter2D(Collision2D collision) {
        gm.HP -= 1;
        Destroy(collision.gameObject);

    }

}
