using UnityEngine;
using System.Collections;

public class AutoDestoryer : MonoBehaviour {

    public float time;

    void Start() {
        Destroy(gameObject, time);
    }
}
