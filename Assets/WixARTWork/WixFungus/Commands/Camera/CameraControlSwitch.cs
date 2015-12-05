using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("攝影機控制",
                 "開關攝影機控制",
                 "開啟或關閉攝影機的跟隨")]
    [AddComponentMenu("")]
    public class CameraControlSwitch : Command {

        public CameraGM 攝影機;
        public bool 開關;

        public override void OnEnter() {
            if (!攝影機) {
                攝影機 = Camera.main.GetComponent<CameraGM>();
                if (!攝影機) {
                    Debug.LogError("找不到攝影機！");
                    Continue();
                    return;
                }
            }

            攝影機.enabled = 開關;
            Continue();
        }

        public override Color GetButtonColor() {
            return new Color32(204, 225, 152, 255);
        }
    }
}