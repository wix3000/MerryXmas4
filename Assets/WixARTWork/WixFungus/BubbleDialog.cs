using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

namespace Fungus {

    public class BubbleDialog : MonoBehaviour {

        // 目前用於顯示文字的對話框
        public static BubbleDialog activeBubbleDialog;

        // 最後一個使用的跟隨對象
        public static Character speakingCharacter;

        // 對話框的過渡方式
        public static TransitType transitType = TransitType.Pop;

        public float transitDuration = 0.25f;

        public Button continueButton;
        public Canvas dialogCanvas;
        public Text contentText;
        public Image backGround;
        public CanvasScaler canvasScaler;
        public RectTransform anchorPoint;
        public bool writeBeforeTransit;

        protected float startStoryTextWidth;
        protected float startStoryTextInset;
        protected Vector3 offset;
        protected Transform charactTransform;

        protected WriterAudio writerAudio;
        protected Writer writer;
        protected CanvasGroup canvasGroup;

        protected bool transitWhenDone = true;
        protected float targetAlpha = 0f;
        protected float transitCoolDownTimer = 0f;
        protected bool overturn;

        protected Tweener tweener;
        protected Tweener effectTweener;

        public static BubbleDialog GetBubbleDialog() {
            if (activeBubbleDialog == null) {
                // 使用場景中找到的第一個泡泡視窗(有的話)
                BubbleDialog bd = FindObjectOfType<BubbleDialog>();
                if (bd != null) {
                    activeBubbleDialog = bd;
                }
                else {
                    // 自動從預置物中生成一個泡泡視窗
                    GameObject prefab = Resources.Load<GameObject>("BubbleDialog");
                    if(prefab != null) {
                        GameObject go = Instantiate(prefab) as GameObject;
                        go.SetActive(false);
                        go.name = "BubbleDialog";
                        activeBubbleDialog = go.GetComponent<BubbleDialog>();
                    }
                }
            }

            return activeBubbleDialog;
        }

        protected Writer GetWriter() {
            if (writer == null) {
                writer = GetComponent<Writer>();
                if (writer == null) {
                    writer = gameObject.AddComponent<Writer>();
                }
            }

            return writer;
        }

        protected CanvasGroup GetCanvasGroup() {
            if (canvasGroup == null) {
                canvasGroup = GetComponent<CanvasGroup>();
                if(canvasGroup == null) {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }

            return canvasGroup;
        }

        protected WriterAudio GetWriterAudio() {
            if (writerAudio == null) {
                writerAudio = GetComponent<WriterAudio>();
                if (writerAudio == null) {
                    writerAudio = gameObject.AddComponent<WriterAudio>();
                }
            }

            return writerAudio;
        }

        protected Transform GetCharactTransform() {
            if(charactTransform == null) {
                if (speakingCharacter == null) {
                    charactTransform = GameObject.FindGameObjectWithTag("Player").transform;
                } else {
                    charactTransform = speakingCharacter.transform;
                }
            }

            return charactTransform;
        }

        protected void Start() {
            
            // 對話框起始不可見，以等待過渡效果。
            switch (transitType) {
                case TransitType.Fade:
                    GetCanvasGroup().alpha = 0f;
                    tweener = GetCanvasGroup().DOFade(0f, 0f).SetAutoKill(false);
                    break;
                case TransitType.Pop:
                    anchorPoint.localScale = Vector3.zero;
                    tweener = anchorPoint.DOScale(1f, transitDuration).SetEase(Ease.OutBack).SetAutoKill(false);
                    break;
            }

            contentText.text = "";

            // Add a raycaster if none already exists so we can handle dialog input
            if (GetComponent<GraphicRaycaster>() == null)
                gameObject.AddComponent<GraphicRaycaster>();
        }

        public virtual void Say(string text, bool clearPrevious, bool waitForInput, bool overturn, bool animateWhenDone, AudioClip audioClip, Action onComplete) {
            StartCoroutine(SayInternal(text, clearPrevious, waitForInput, overturn, animateWhenDone, audioClip, onComplete));
        }

        protected virtual IEnumerator SayInternal(string text, bool clearPrevious, bool waitForInput, bool overturn, bool animateWhenDone, AudioClip audioClip, Action onComplete) {
            Writer writer = GetWriter();

            // 停止所有的對話指令，並改為本指令
            // 這可能會花費1~2幀
            while(writer.isWriting || writer.isWaitingForInput) {
                writer.Stop();
                yield return null;
            }

            this.transitWhenDone = animateWhenDone;

            // 如果沒有指定語音片段，則尋找角色音效
            AudioClip clip = audioClip;
            if (speakingCharacter != null && clip == null) {
                clip = speakingCharacter.soundEffect;
            }

            if (overturn) {
                backGround.transform.localEulerAngles = Vector3.up * 180f;
                contentText.transform.localEulerAngles = Vector3.up * 180f;
            }
            else {
                backGround.transform.localEulerAngles = Vector3.zero;
                contentText.transform.localEulerAngles = Vector3.zero;
            }
            this.overturn = overturn;

            CreateTweener();
            while (tweener.IsPlaying() && !writeBeforeTransit) {
              yield return null;
            }

            if(effectTweener != null && effectTweener.IsActive()) {
                if (effectTweener.IsPlaying()) {
                    effectTweener.Rewind();
                    effectTweener.Kill();
                } else {
                    effectTweener.Play();
                }
            }

            writer.Write(text, clearPrevious, waitForInput, clip, onComplete);
        }

        protected virtual void LateUpdate() {
            UpdateTransition();
            FollowCharacter();
            if (continueButton != null) {
                continueButton.gameObject.SetActive(GetWriter().isWaitingForInput);
            }
        }

        protected virtual void OnEnable() {
            Camera[] cameras = Camera.allCameras;
            for(int i = 0; i < cameras.Length; i++) {
                if (cameras[i].name != "UI Camera") continue;
                GetComponent<Canvas>().worldCamera = cameras[i];
                break;
            }
        }

        /**
         * 如果完成文字寫入則告訴對話框去淡出
         */
        public virtual void FadeOut() {
            transitWhenDone = true;
        }

        /**
         * 停止正在寫入文字的對話框
         */
        public virtual void Stop() {
            transitWhenDone = true;
            GetWriter().Stop();
        }

        protected virtual void UpdateTransition() {
            float trasitDuration = GetBubbleDialog().transitDuration;
            if (GetWriter().isWriting) {
                CreateTweener();
                transitCoolDownTimer = 0.1f;
            }
            else if (transitWhenDone && transitCoolDownTimer == 0f) {
                if (!tweener.IsPlaying()) {
                    tweener.OnStepComplete(() =>
                    {
                        gameObject.SetActive(false);
                        tweener.Kill();
                    }).PlayBackwards();
                }
            }
            else {
                // Add a short delay before we start fading in case there's another Say command in the next frame or two.
                // 這能避免在連續的對話指令之間發生閃爍
                transitCoolDownTimer = Mathf.Max(0f, transitCoolDownTimer - Time.deltaTime);
            }
        }        

        protected virtual void CreateTweener() {
            if (tweener == null || !tweener.IsActive()) {
                contentText.text = "";
                switch (transitType) {
                    case TransitType.Fade:
                        GetCanvasGroup().alpha = 0f;
                        tweener = GetCanvasGroup().DOFade(0f, 0f).SetAutoKill(false);
                        break;
                    case TransitType.Pop:
                        anchorPoint.localScale = Vector3.zero;
                        tweener = anchorPoint.DOScale(1f, transitDuration).SetEase(Ease.OutBack).SetAutoKill(false);
                        break;
                }
            }
        }

        protected virtual void FollowCharacter() {
            Vector3 offset = this.offset;
            offset.x = (overturn) ? -offset.x : offset.x;
            Vector3 v = Camera.main.WorldToScreenPoint(GetCharactTransform().position + offset);

            v.x *= canvasScaler.referenceResolution.x / Camera.main.pixelWidth;
            v.y *= canvasScaler.referenceResolution.y / Camera.main.pixelHeight;

            anchorPoint.anchoredPosition3D = v;
        }

        public virtual void PlayEffect(BubbleEffectType type, float time, float str) {
            switch (type) {
                case BubbleEffectType.震動:
                    effectTweener = backGround.rectTransform.DOShakePosition(time, str,20,90f,true).Pause();
                break;
            }
        }

        public virtual void SetCharacter(Character character, Flowchart flowchart = null) {
            if (character == null) {
                //speakingCharacter = null;
            }
            else {
                speakingCharacter = character;
                charactTransform = character.transform;
                offset = character.offset;
                backGround.color = character.nameColor;
                contentText.color = character.textColor;
            }
        }

        public virtual void Clear() {
            ClearStoryText();

            // 關閉所有寫入的線程
            StopAllCoroutines();
        }

        protected virtual void ClearStoryText() {
            if(contentText != null) {
                contentText.text = "";
            }
        }

        public void SetBackGround(Sprite sprite) {
            if (sprite != null)
                backGround.sprite = sprite;
        }
    }

    public enum TransitType {
        Fade,
        Pop
    }
}
