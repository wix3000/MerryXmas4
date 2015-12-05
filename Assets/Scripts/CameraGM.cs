using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraGM : MonoBehaviour {

    [SerializeField]
    Transform myTarget;
    public Vector3 offset;
    public float lrepRate;
    public bool lockY;
	Vector3 m_pos;
    float originY;
    public bool active;
    public Tweener tweener;

    void Start() {
        originY = transform.position.y;
    }

    public Transform Target {
        get {
            if (!myTarget && GameObject.FindGameObjectWithTag("Player")) myTarget = GameObject.FindGameObjectWithTag("Player").transform;
            if (!myTarget) {
                active = false;
                return null;
            }
            return myTarget;
        }
        set {
            myTarget = value;
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (tweener != null && tweener.IsActive()) return;
        if (!active) return;
        //m_pos= Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,m_dis));
        Vector3 camepos = transform.position;
        m_pos = Target.position + offset;
        m_pos.x = Mathf.Lerp(camepos.x, m_pos.x, lrepRate);
        m_pos.y = (lockY) ? originY : Mathf.Lerp(camepos.y, m_pos.y, lrepRate);
        m_pos.z = Mathf.Lerp(camepos.z, m_pos.z, lrepRate);

        transform.position = m_pos;
	}

    public void Shake(float duration, float strength, int vibrato) {
        if (tweener != null && tweener.IsActive()) tweener.Kill();
        tweener = Camera.main.DOShakePosition(duration, strength, vibrato).SetEase(Ease.Linear).SetUpdate(true);
    }
}
