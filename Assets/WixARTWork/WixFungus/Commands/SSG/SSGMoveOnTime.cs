using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("橫向捲軸控制",
                 "移動",
                 "命令有SSG模組層的遊戲物件移動一段時間")]
    [AddComponentMenu("")]
    public class SSGMoveOnTime : Command {

        [Tooltip("若留空，則自動尋找Player標簽的遊戲物件")]
        public SSG.SSGPlayer_Model 玩者;
        public float X軸向;
        public float 時間;
        public bool 等待完成;

        public override void OnEnter() {
            if (玩者 == null) {
                玩者 = GameObject.FindGameObjectWithTag("Player").GetComponent<SSG.SSGPlayer_Model>();
                if (玩者 == null) {
                    Debug.LogError("找不到帶有Player標籤的SSG模組層！");
                    Continue();
                    return;
                }
            }
            StartCoroutine( 玩者.MoveOnTime(X軸向, 時間));

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
            return new Color32(135, 217, 255, 255);
        }
    }
}