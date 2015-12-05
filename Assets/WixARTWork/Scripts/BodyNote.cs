using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BodyNote : MonoBehaviour {

    public float transitiTime = 0.75f;
    public RPG_ChatracterController controller;
    public GameObject pick;
    public Transform body;
    public Transform likeValue;
    public BodyDisplayWindow[] bodyWindows = new BodyDisplayWindow[4];
    public EventPoint eventPoint;

    List<Tweener> tweeners = new List<Tweener>();

    bool effectEnd;

	// Use this for initialization
	void Start () {
        if (!controller) controller = RPG_ChatracterController.GetController;
        eventPoint.theEvent += () => Enable();        
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.X)&& effectEnd) {
            StartCoroutine(Unenable());
        }
    }

    void Enable() {
        if(controller) controller.SetActive(false);
        pick.SetActive(true);
        tweeners.Add(
            body.DOLocalMoveY(0f, transitiTime).SetAutoKill(false).SetEase(Ease.OutBack).OnComplete(() => {
                foreach(BodyDisplayWindow bd in bodyWindows) {
                    tweeners.Add(
                        bd.transform.DOScale(1f, transitiTime).SetAutoKill(false).SetEase(Ease.OutBack).OnComplete(() => effectEnd = true)
                        );
                }
            })
            );
        tweeners.Add(
            likeValue.DOLocalMoveX(0f, transitiTime).SetAutoKill(false)
            );
    }

    IEnumerator Unenable() {
        foreach(Tweener t in tweeners) {
            t.PlayBackwards();
            t.SetAutoKill(true);
        }
        effectEnd = false;
        yield return new WaitForSeconds(transitiTime);
        if (controller) controller.SetActive(true);
    }
}
