using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed;
    public float rotated;
    public AutoDestoryer FailEffect;
    public AutoDestoryer HitEffect;

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        transform.Rotate(Vector3.forward, -rotated * Time.deltaTime);
	}

    void OnTriggerEnter2D(Collider2D hit) {
        if (hit.CompareTag("Floor") || hit.CompareTag("Player")) {
            if (FailEffect) {
                FailEffect.transform.SetParent(null);
                FailEffect.transform.position = Vector3.forward * -50f;
                FailEffect.enabled = true;
            }
            if (HitEffect) {
                Instantiate(HitEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
