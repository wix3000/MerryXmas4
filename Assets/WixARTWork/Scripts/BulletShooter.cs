using UnityEngine;
using System.Collections;

public class BulletShooter : MonoBehaviour {

    public GameObject ballet;
    public Transform belletPos;
    
    public void Shoot() {
        GameObject g;
        g = Instantiate(ballet, belletPos.position, belletPos.rotation) as GameObject;
        g.SetActive(true);
    }
}
