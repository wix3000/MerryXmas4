using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("RPG控制",
                 "顯示氣泡表情",
                 "於指定的遊戲物件身上顯示氣泡表情")]
    [AddComponentMenu("")]
    public class RPGShowExpression : Command {

        [Tooltip("若留空，則自動尋找Player標簽的遊戲物件")]
        public Transform 目標物件;
        [Tooltip("若留空會使用預設圖片，圖片格式請務必切割為8x8")]
        public Texture 使用圖片;
        [Range(1, 8)]
        public int 表情順序 = 1;
        public Vector2 偏移量 = new Vector2(0f, 50f);
        public float 播放速度 = 1f;
        public Color 色調 = Color.white;
        public string 預置物名稱 = "Expression";


        public override void OnEnter() {
            if (目標物件 == null) {
                try {
                    目標物件 = GameObject.FindGameObjectWithTag("Player").transform;
                }
                catch (System.Exception ex) {
                    Debug.LogError(ex);
                    Continue();
                    return;
                }
                if (目標物件 == null) {
                    Debug.LogError("找不到帶有Player標籤的RPG控制器！");
                    Continue();
                    return;
                }
            }

            GameObject exp = Instantiate(Resources.Load(預置物名稱)) as GameObject;
            exp.transform.SetParent(目標物件);
            ((RectTransform)exp.transform).anchoredPosition = 偏移量;

            UnityEngine.UI.RawImage ri = exp.GetComponentInChildren<UnityEngine.UI.RawImage>();
            if (使用圖片) ri.texture = 使用圖片;
            ri.uvRect = new Rect(0f, 1f - 0.125f * 表情順序, 0.125f, 0.125f);
            ri.color = 色調;

            exp.GetComponent<Animator>().speed = 播放速度;

            Continue();
        }
    }
}