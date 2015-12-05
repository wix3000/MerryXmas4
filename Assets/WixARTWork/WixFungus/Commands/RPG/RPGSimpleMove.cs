using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("RPG控制",
                 "簡單移動",
                 "命令RPG遊戲物件移動到指定位置")]
    [AddComponentMenu("")]
    public class RPGSimpleMove : Command {

        [Tooltip("若留空，則自動尋找Player標簽的遊戲物件")]
        public RPG_ChatracterController 玩者;
        [Tooltip("若小於0，會套用物件本身的移動速度")]
        public float 速度;
        public Vector3 移動座標;
        public bool 相對移動 = true;
        public bool 是否轉向 = true;
        public bool 等待完成 = true;

        public override void OnEnter() {
            if (玩者 == null) {
                玩者 = RPG_ChatracterController.GetController;
                if (玩者 == null) {
                    Debug.LogError("找不到帶有Player標籤的RPG控制器！");
                    Continue();
                    return;
                }
            }

            System.Action a = null;
            if (等待完成) a = Continue;

            StartCoroutine(玩者.WalkTo(移動座標, 速度, 相對移動, 是否轉向, a));

            if (!等待完成) {
                Continue();
            }
        }

        public override Color GetButtonColor() {
            return new Color32(255, 221, 170, 255);
        }
    }
}