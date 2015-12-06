using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System;

public class RPG_ChatracterController : MonoBehaviour, IPlayerSwitch {

    public float speed;
    public Animator animator;
    public Transform actor, shadow;
    public RawImage rawImage;

    public float jumpHeigh, jumpTime;

    public float axisX, axisY;

    bool isJumping;
    EventPoint triggedEventPoint;

    public bool active = true;

    public static RPG_ChatracterController GetController {
        get {
            if (!GameObject.FindGameObjectWithTag("Player")) return null;
            RPG_ChatracterController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<RPG_ChatracterController>();
            if (controller == null) Debug.LogError("找不到RPG玩者控制器！");
            return controller;
        }
    }

    // Use this for initialization
    void Start () {
        if (animator == null) animator = GetComponent<Animator>();
        if (actor == null) actor = transform.Find("Actor");
        if (rawImage == null) rawImage = GetComponentInChildren<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
        InputProcess();
        Movement();
    }

    void InputProcess() {
        if (tag != "Player" || !active) return;

        axisX = Input.GetAxis(GameOption.Horizontal);
        axisY = Input.GetAxis(GameOption.Vertical);

        if (Input.GetKey(KeyCode.Space) && !isJumping) {
            Jump();
        }

        // 按下Z時執行目前觸發的事件點的事件
        if (Input.GetKeyDown(KeyCode.Z) && triggedEventPoint != null && triggedEventPoint.theEvent != null) {
            triggedEventPoint.theEvent();
        }
    }

    void Movement() {
        if (Mathf.Abs(axisX) < Mathf.Epsilon && Mathf.Abs(axisY) < Mathf.Epsilon) {
            animator.SetBool("isWalking", false);
            return;
        }

        Vector3 u = Vector3.ClampMagnitude(new Vector3(axisX, axisY), 1f);
        Move(u);
        if (Input.anyKey) Rotate(u);
        animator.SetBool("isWalking", true);
    }

    void Move(Vector3 vector) {
        transform.Translate(vector * Time.deltaTime * speed);
    }

    void Jump() {
        isJumping = true;
        actor.DOLocalMoveY(jumpHeigh, jumpTime).SetRelative().SetLoops(2, LoopType.Yoyo).OnComplete(() => isJumping = false);
        shadow.DOScale(-0.4f, jumpTime).SetRelative().SetLoops(2, LoopType.Yoyo);
    }

    void Rotate(Vector3 vector) {
        int r = Mathf.RoundToInt((Quaternion.FromToRotation(Vector3.up, vector).eulerAngles.z) / 90);
        Rect rect = rawImage.uvRect;
        switch (r) {
            case 0:
                rect.y = 0f;
                break;
            case 1:
                rect.y = 0.5f;
                break;
            case 2:
                rect.y = 0.75f;
                break;
            case 3:
                rect.y = 0.25f;
                break;
            case 4:
                rect.y = 0f;
                break;
        }
        rawImage.uvRect = rect;
    }

    /// <summary>
    /// 移動往指定位置的方法。
    /// </summary>
    /// <param name="target">移動軸</param>
    /// <param name="speed">指定速度，小於0則套用控制器速度</param>
    /// <param name="isRelative">是否相對</param>
    /// <param name="rotate">是否轉向</param>
    /// <param name="callBack">回傳事件，可不填</param>
    public IEnumerator WalkTo(Vector2 target, float? speed = null, bool isRelative = true, bool rotate = true, Action callBack = null) {
        float originSpeed = 0f;
        if (speed.HasValue) {
            originSpeed = this.speed;
            this.speed = speed.Value;
        }
        Vector2 relativeVector = (isRelative) ? target : target - (Vector2)transform.position;
        if(rotate) Rotate(relativeVector);
        float timer = Vector2.Distance(relativeVector, Vector2.zero) / this.speed;
        axisX = relativeVector.normalized.x;
        axisY = relativeVector.normalized.y;
        while(timer > Mathf.Epsilon) {
            timer -= Time.deltaTime;
            yield return null;
        }
        axisX = 0f;
        axisY = 0f;
        if (speed > 0f) this.speed = originSpeed;
        if (callBack != null) callBack();
    }

    /// <summary>
    /// 以指定速度移段一段時間。
    /// </summary>
    /// <param name="axis">角速度</param>
    /// <param name="time">時間</param>
    /// <returns></returns>
    public IEnumerator MoveOnTime(Vector2 axis, float time) {
        axisX = axis.x;
        axisY = axis.y;
        yield return new WaitForSeconds(time);
        axisX = 0f;
        axisY = 0f;
    }

/// <summary>
/// 跳躍至指定位置。
/// </summary>
/// <param name="target">目前座標</param>
/// <param name="isRelative">是否相對移動</param>
/// <param name="rotate">是否轉向</param>
/// <param name="callBack">回調事件</param>
/// <returns></returns>
    public IEnumerator JumpTo(Vector3 target, bool isRelative = true, bool rotate = true,Action callBack = null) {
        // 等待直到落地為止
        while (isJumping) {
            yield return null;
        }
        Jump();
        Vector3 relativeVector = (isRelative) ? target : target - transform.position;
        if (rotate) Rotate(relativeVector);
        float originSpeed = speed;
        speed = Vector3.Distance(target, Vector3.zero) / jumpTime / 2f;
        float timer = jumpTime * 2f;
        axisX = relativeVector.normalized.x;
        axisY = relativeVector.normalized.y;
        while (timer > 0f) {
            timer -= Time.deltaTime;
            yield return null;
        }
        axisX = 0f;
        axisY = 0f;
        speed = originSpeed;
        if (callBack != null) callBack();
    }

    public void FaceTo(Vector3 target) {
        Rotate(target - transform.position);
    }


    void OnTriggerEnter2D(Collider2D enter) {
        if (enter.GetComponent<EventPoint>()) {
            triggedEventPoint = enter.GetComponent<EventPoint>();
        }
    }

    void OnTriggerExit2D(Collider2D exit) {
        if (exit.GetComponent<EventPoint>() && triggedEventPoint != null && exit.name == triggedEventPoint.name) {
            triggedEventPoint = null;
        }
    }

    /// <summary>
    /// 改變面向
    /// </summary>
    /// <param name="dir">0:上 1:右 2:左 3:下</param>
    public void FaceByDir(int dir) {
        Rect rect = rawImage.uvRect;
        rect.y = 0.25f * dir;
        rawImage.uvRect = rect;
    }

    public void SetWalking(bool b) {
        animator.SetBool("isWalking", b);
    }

    public virtual void SetActive(bool b) {
        active = b;
        if (!b) {
            axisX = 0f;
            axisY = 0f;
        }
    }
}
