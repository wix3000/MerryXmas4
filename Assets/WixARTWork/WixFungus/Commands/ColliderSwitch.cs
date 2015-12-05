using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("RPG控制",
                 "開關碰撞",
                 "設置是否無視碰撞")]
    [AddComponentMenu("")]
    public class ColliderSwitch : Command {

        [Tooltip("若留空，則自動尋找Player標簽的遊戲物件")]
        public GameObject 玩者;
        public bool 碰撞器開關;

        public override void OnEnter() {
            if (玩者 == null) {
                玩者 = GameObject.FindGameObjectWithTag("Player");
                if (玩者 == null) {
                    Debug.LogError("找不到帶有Player標籤的遊戲物件！");
                    Continue();
                    return;
                }
            }

            if(玩者.GetComponent<Collider2D>()) 玩者.GetComponent<Collider2D>().enabled = 碰撞器開關;
            Continue();
        }
    }
}