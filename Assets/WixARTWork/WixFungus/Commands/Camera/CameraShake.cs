using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Fungus {
    [CommandInfo("攝影機控制",
                 "攝影機震動",
                 "命令攝影機震動")]
    [AddComponentMenu("")]
    public class CameraShake : Command {

        [Tooltip("留空則自動選擇主攝影機")]
        public Camera 攝影機;
        public float 力道 = 1f;
        public int 頻率 = 10;
        public float 時間 = 1f;
        public bool 等待震動結束 = false;

        public override void OnEnter() {
            if (!攝影機) {
                攝影機 = Camera.main;
                if (!攝影機) {
                    Debug.LogError("找不到攝影機！");
                    Continue();
                    return;
                }
            }

            Tweener tween = 攝影機.transform.DOShakePosition(時間, 力道, 頻率);

            if (等待震動結束) {
                tween.OnComplete(() => Continue());
            } else {
                Continue();
            }
        }
    }
}