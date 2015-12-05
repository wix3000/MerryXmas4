using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("橫向捲軸控制",
                 "跳躍",
                 "命令有SSG模組層的遊戲跳躍")]
    [AddComponentMenu("")]
    public class SSGForceJump : Command {

        [Tooltip("若留空，則自動尋找Player標簽的遊戲物件")]
        public SSG.SSGPlayer_Model 玩者;
        public float 跳躍力;

        public override void OnEnter() {
            if (玩者 == null) {
                玩者 = GameObject.FindGameObjectWithTag("Player").GetComponent<SSG.SSGPlayer_Model>();
                if (玩者 == null) {
                    Debug.LogError("找不到帶有Player標籤的SSG模組層！");
                    Continue();
                    return;
                }
            }
            玩者.ForceJump(跳躍力);
            Continue();
        }

        public override Color GetButtonColor() {
            return new Color32(135, 217, 255, 255);
        }
    }
}