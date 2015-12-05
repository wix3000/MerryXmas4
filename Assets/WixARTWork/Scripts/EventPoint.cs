using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Fungus;

public class EventPoint : MonoBehaviour {

    [SerializeField]
    PointType type;

    bool active;
    public Image bubble;
    public RectTransform crysyal;
    public Text text;

    [Tooltip("經典模式用於泡泡框彈出時間，水晶模式用於單次循環時間")]
    public float duration;
    public float offset;

    public Flowchart flowchart;
    public string message;

    Tweener tweener;
    Tweener tempTweener;

    public System.Action theEvent = null;

	// Use this for initialization
	void Start () {
        if (message == "") return;
        if(flowchart == null) {
            flowchart = FindObjectOfType<Flowchart>();
        }
        if (flowchart == null) {
            return;
        }

        theEvent += () => flowchart.SendFungusMessage(message);
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D enter) {
        EnableEffect();
    }

    void OnTriggerExit2D(Collider2D enter) {
        active = false;
        UnenableEffect();
    }

    Tweener EffectTweener {
        get {
            if (tweener == null) {
                tweener = crysyal.DOLocalMoveY(offset, duration).SetRelative().SetEase(Ease.InOutCubic).SetLoops(-1,LoopType.Yoyo);
            }
            return tweener;
        }
    }

    void EnableEffect() {
        switch (type) {
            case PointType.classic:
                ClassicEffectOn();
                break;
            case PointType.crystal:
                CrystalEffectOn();
                break;
        }
    }

    void UnenableEffect() {       
        switch (type) {
            case PointType.classic:
                ClassicEffectOff();
                break;
            case PointType.crystal:
                CrystalEffectOff();
                break;
        }
    }

    void ClassicEffectOn() {
        bubble.gameObject.SetActive(true);
        bubble.DOFade(1f, duration).SetEase(Ease.Linear);
        text.DOFade(1f, duration).SetEase(Ease.Linear);
        tempTweener.Kill();
        tempTweener = bubble.rectTransform.DORotate(Vector3.zero, duration).SetEase(Ease.Linear);
    }

    void ClassicEffectOff() {
        bubble.DOFade(0f, duration).SetEase(Ease.Linear);
        text.DOFade(0f, duration).SetEase(Ease.Linear);
        tempTweener.Kill();
        tempTweener = bubble.rectTransform.DORotate(Vector3.forward * 170f, duration).SetEase(Ease.Linear).OnComplete(() => bubble.gameObject.SetActive(false));
    }

    void CrystalEffectOn() {
        tempTweener.Kill();
        tempTweener = text.DOFade(1f, duration).SetEase(Ease.Linear);
        EffectTweener.Play();
    }

    void CrystalEffectOff() {
        tempTweener.Kill();
        tempTweener = text.DOFade(0f, duration / 2f).SetEase(Ease.Linear);
        EffectTweener.Pause();
    }

    enum PointType {
        classic,
        crystal
    }
}
