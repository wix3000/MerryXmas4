using UnityEngine;
using System.Collections;


namespace Fungus {
    [CommandInfo("底層指令",
                 "開關玩者控制",
                 "開啟/關閉玩者的控制權")]
    [AddComponentMenu("")]
    public class CharacterSwitch : Command {

        [Tooltip("若留空，則自動尋找Player標簽的遊戲物件")]
        public GameObject 玩者;
        public bool 是否可控制;

        public override void OnEnter() {
            if (玩者 == null) {
                玩者 = GameObject.FindGameObjectWithTag("Player");
                if (玩者 == null) {
                    Debug.LogError("找不到帶有Player標籤的遊戲物件！");
                    Continue();
                    return;
                }
            }
            IPlayerSwitch player = 玩者.GetComponent<IPlayerSwitch>();
            if (player != null) {
                player.SetActive(是否可控制);
            }
            Continue();
        }

        public override string GetSummary() {
            string prefix = (是否可控制) ? "開啟 " : "關閉 ";
            return prefix + "玩者控制權";
        }

        public override Color GetButtonColor() {
            return new Color32(210, 210, 210, 255);
        }
    }
}