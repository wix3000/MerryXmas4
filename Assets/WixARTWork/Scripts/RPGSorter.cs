using UnityEngine;
using System.Collections;

public class RPGSorter : MonoBehaviour {
    void LateUpdate() {
        for(int i = 0; i < transform.childCount - 1; i++) {
            if(transform.GetChild(i).position.y < transform.GetChild(i + 1).position.y) {
                transform.GetChild(i).SetSiblingIndex(i + 1);
            }
        }
    }
}
