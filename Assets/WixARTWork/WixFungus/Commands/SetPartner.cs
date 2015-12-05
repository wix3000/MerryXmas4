using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("底層指令",
                 "設置隊友",
                 @"設置要外帶(?)的隊友
0 = 無隊友
1 = Usai
2 = Shirogi
3 = Yukari")]
    [AddComponentMenu("")]
    public class SetPartner : Command {

        [Range(0,3)]
        public int 隊員編號;

        public override void OnEnter() {
            Game.globalVariable["Partner"] = 隊員編號;
            Continue();
        }

        public override string GetSummary() {
            string prefix = "設置隊友為 ";
            switch (隊員編號) {
                case 0:
                    prefix += "空";
                    break;
                case 1:
                    prefix += "卯碎";
                    break;
                case 2:
                    prefix += "白儀";
                    break;
                case 3:
                    prefix += "紫";
                    break;
            }
            return prefix;
        }

        public override Color GetButtonColor() {
            return new Color32(210, 210, 210, 255);
        }
    }
}