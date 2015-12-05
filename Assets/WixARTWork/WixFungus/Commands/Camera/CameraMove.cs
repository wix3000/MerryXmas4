using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Fungus {
    [CommandInfo("攝影機控制",
                 "移動攝影機",
                 "移動攝影機到指定位置")]
    [AddComponentMenu("")]
    public class CameraMove : Command {

        [Tooltip("留空則自動選擇主攝影機")]
        public Camera 攝影機;
        [Tooltip("若目標不為空，則套用目標該瞬間的位置為移動目標\n若為空則套用下方位移參數")]
        public Transform 目標;
        public Vector3 移動距離;
        public bool 相對移動 = true;
        public float 時間 = 1f;
        public bool 等待移動結束 = false;
        public Ease 平滑方式 = Ease.Linear;

        Tweener tweener;

        public override void OnEnter() {
            CameraGM cg;
            Vector3 target;

            // 先抓攝影機
            if (!攝影機) {
                攝影機 = Camera.main;
                if (!攝影機) {
                    Debug.LogError("找不到攝影機！");
                    Continue();
                    return;
                }
            }

            // 關掉攝影機控制
            cg = 攝影機.GetComponent<CameraGM>();
            if(cg) cg.enabled = false;

            // 計算目標點
            target = (目標 != null) ? 目標.position : 移動距離;
            if (!相對移動) target += cg.offset;

            // 建立tweener
            tweener = 攝影機.transform.DOMove(target, 時間).SetEase(平滑方式);
            if (相對移動) tweener.SetRelative();

            // 繼續處理
            if (等待移動結束) {
                tweener.OnComplete(() => Continue());
            }
            else {
                Continue();
            }
        }

        public override Color GetButtonColor() {
            return new Color32(204, 225, 152, 255);
        }
    }
}