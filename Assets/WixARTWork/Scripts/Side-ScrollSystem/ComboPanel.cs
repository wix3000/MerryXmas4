using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ComboPanel : MonoBehaviour {

    public static int comboValue;
    public static System.Action onChange;
    public float waitTime;

    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    Image[] numberImages = new Image[3];

    [SerializeField]
    Sprite[] numberSprite = new Sprite[10];

    Tweener fadeTweener;

    // Use this for initialization
    void Start() {
        gameObject.SetActive(false);
        onChange += OnChange;
	}

    void OnChange() {
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        if (fadeTweener != null && fadeTweener.IsActive()) fadeTweener.Kill();
        canvasGroup.alpha = 1f;
        fadeTweener = canvasGroup.DOFade(0f, waitTime).SetEase(Ease.InQuad).OnComplete(() =>
        {
            comboValue = 0;
            gameObject.SetActive(false);
        });
        RefreshNumber();
    }

    void RefreshNumber() {
        int tempInt = comboValue;
        for (int i = 0; i < numberImages.Length; i++) {
            if (tempInt <= 0) {
                numberImages[i].gameObject.SetActive(false);
                continue;
            }
            numberImages[i].gameObject.SetActive(true);
            numberImages[i].sprite = numberSprite[tempInt % 10];
            tempInt /= 10;
        }
    }

    void OnDestroy() {
        comboValue = 0;
        onChange -= OnChange;
    }


    public static void KeepCombo(int i = 1) {
        comboValue = Mathf.Clamp(comboValue + i, 0, 999);
        if (onChange != null) onChange();
    }
}
