using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("RPG控制",
                 "時間移動",
                 "命令RPG遊戲物件以指定速度移動一段時間")]
    [AddComponentMenu("")]
    public class RPGMoveOnTime : Command {
        [Tooltip("若留空，則自動尋找Player標簽的遊戲物件")]
        public RPG_ChatracterController 玩者;
        public Vector2 速度向量;
        public float 時間;
        public bool 等待完成;

        public override void OnEnter() {
            if (玩者 == null) {
                玩者 = RPG_ChatracterController.GetController;
                if (玩者 == null) {
                    Debug.LogError("找不到帶有Player標籤的RPG控制器！");
                    Continue();
                    return;
                }
            }

            StartCoroutine(玩者.MoveOnTime(速度向量, 時間));

            if (等待完成) {
                Invoke("CallContinue", 時間);
            }
            else {
                Continue();
            }
        }

        void CallContinue() {
            Continue();
        }

        public override Color GetButtonColor() {
            return new Color32(255, 221, 170, 255);
        }
    }
}
