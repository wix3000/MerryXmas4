using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Colidertest : MonoBehaviour {

    public FloatText ft;
    public Transform canvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D hit) {
        print(hit.name);
        
    }
}
