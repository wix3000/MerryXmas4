using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public Transform target;
    public float distance;
    public int num;
    public int defDir;
    public Vector3 originPos;
    [SerializeField]
    RPG_ChatracterController _ctrler;
    Rigidbody2D rigid;
    GameObject eventTrigger;

    bool staticable = true;

    RPG_ChatracterController Ctrler {
        get {
            if (!_ctrler) _ctrler = GetComponent<RPG_ChatracterController>();
            return _ctrler;
        }
    }

    void Start() {
        if (!target) target = GameObject.FindGameObjectWithTag("Player").transform;
        rigid = GetComponent<Rigidbody2D>();
        eventTrigger = GetComponentInChildren<EventPoint>().gameObject;
        originPos = transform.position;
        originPos.z = 0f;
    }

    // Update is called once per frame
    void Update() {
        if((int)Game.globalVariable["Partner"] == num) {
            staticable = false;
            Follow();
        } else if (!staticable) {
            DisFollow();
        }
    }

    void Follow() {
        if (Vector2.Distance(transform.position, target.position) < distance) {
            Ctrler.axisX = 0f;
            Ctrler.axisY = 0f;
            return;
        }
        rigid.mass = 10f;
        eventTrigger.SetActive(false);

        Vector2 v = (target.position - transform.position).normalized;
        v *= Vector2.Distance(transform.position, target.position) / (distance * 2);
        Ctrler.axisX = v.x;
        Ctrler.axisY = v.y;
    }

    public IEnumerator DisableFollow() {
        while (Vector2.Distance(transform.position, originPos) > Mathf.Epsilon) {
            Vector2 v = Vector2.ClampMagnitude(originPos - transform.position, 1f);
            Ctrler.axisX = v.x;
            Ctrler.axisY = v.y;
            yield return null;
        }
        Ctrler.axisX = 0f;
        Ctrler.axisY = 0f;
        Ctrler.FaceTo(target.position);
    }

    void DisFollow() {
        /*
        if (Vector2.Distance(transform.position, originPos) < 5f) {
            Ctrler.axisX = 0f;
            Ctrler.axisY = 0f;
            transform.position = originPos;
            Ctrler.FaceTo(target.position);
            staticable = true;
        }
        Vector2 v = Vector2.ClampMagnitude(originPos - transform.position, 1f);
        Ctrler.axisX = v.x;
        Ctrler.axisY = v.y;
        */
        
        StartCoroutine(Ctrler.WalkTo(originPos, 0f, false, true, () =>
        {
            Ctrler.FaceTo(target.position);
            staticable = true;
            rigid.mass = 10000f;
            eventTrigger.SetActive(true);
        }));
    }
}