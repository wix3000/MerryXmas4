using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour {

    [SerializeField]
    Transform origin;
    [SerializeField]
    float finalHeigh = 100f;

	// Use this for initialization
	void Start () {
        if (!origin) origin = transform.root;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 cast = new Vector3(origin.position.x, 5f, origin.position.z);
        RaycastHit2D hit = Physics2D.Raycast(cast, Vector2.down, 100f, LayerMask.GetMask("Floor"));
        Vector3 v = hit.point;
        v.z = origin.position.z;
        transform.position = v;

        transform.localScale = Vector3.one * (1 - Vector3.Distance(hit.point, origin.position) / finalHeigh);
	}
}
