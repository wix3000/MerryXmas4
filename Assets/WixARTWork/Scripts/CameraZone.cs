using UnityEngine;
using System.Collections;

public class CameraZone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D hit) {
        if (hit.CompareTag("Player")){
            Camera.main.GetComponent<CameraGM>().active = false;
        }
    }

    void OnTriggerExit2D(Collider2D hit) {
        if (hit.CompareTag("Player")){
            Camera.main.GetComponent<CameraGM>().active = true;
        }
    }
}
