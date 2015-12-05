using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("攝影機控制",
                 "設定攝影機目標",
                 "設定攝影機的跟隨目標")]
    [AddComponentMenu("")]
    public class CameraTarget : Command {
        public CameraGM 攝影機;
        public Transform 目標;
        public bool 設置完開啟跟隨 = true;

        public override void OnEnter() {
            if (!攝影機) {
                攝影機 = Camera.main.GetComponent<CameraGM>();
                if (!攝影機) {
                    Debug.LogError("找不到攝影機！");
                    Continue();
                    return;
                }
            }

            if (!目標) {
                目標 = GameObject.FindGameObjectWithTag("Player").transform;
                if (!目標) {
                    Debug.LogError("找不到目標！");
                    Continue();
                    return;
                }
            }

            攝影機.Target = 目標;
            if(設置完開啟跟隨) 攝影機.enabled = true;
            Continue();
        }

        public override Color GetButtonColor() {
            return new Color32(204, 225, 152, 255);
        }
    }
}
