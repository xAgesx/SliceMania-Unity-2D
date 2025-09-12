using TMPro;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    public GameManager gm;
    public AudioSource lifeLoss;
    void OnCollisionEnter2D(Collision2D collision) {
        gm.HP -= 1;
        if (gm.HP < 0) {
            gm.HP = 0;
        }
        lifeLoss.Play();
        if (collision.gameObject.CompareTag("Item")) {
            Destroy(collision.gameObject);    
        }
        
    }

}
