using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus {
    [CommandInfo("Narrative",
                 "泡泡對話",
                 "以泡泡框進行對話。")]
    [AddComponentMenu("")]
    public class BubbleSay : Command, ILocalizable {

        [TextArea(5, 10)]
        public string 文字內容 = "";

        [Tooltip("給與其他人的說明。")]
        public string 描述 = "";

        [Tooltip("對話的角色")]
        public Character 角色;

        [Tooltip("繪製文字時要發出的聲音")]
        public AudioClip 文字聲音;

        [Tooltip("是否將對話框左右翻轉")]
        public bool 翻轉 = false;

        [Tooltip("是否當被重新呼叫時重新顯示")]
        public bool 總是顯示 = true;

        [Tooltip("被重新呼叫時重新顯示的次數")]
        public int 顯示次數 = 1;

        [Tooltip("是否將此段對話追加在上一段對話之後")]
        public bool 追加上文 = false;

        [Tooltip("是否在對話完成而且沒有在等待輸入時淡出對話框")]
        public bool 結束時過渡 = true;

        [Tooltip("是否等待玩家輸入才繼續")]
        public bool 等待輸入 = true;

        [Tooltip("設置用來顯示的對話框")]
        public BubbleDialog 對話框;

        protected int executionCount;

        public override void OnEnter() {
            if (!總是顯示 && executionCount >= 顯示次數) {
                Continue();
                return;
            }

            executionCount++;

            // 有指定對話框時，覆寫
            if (對話框 != null) BubbleDialog.activeBubbleDialog = 對話框;

            BubbleDialog sayDialog = BubbleDialog.GetBubbleDialog();

            if(sayDialog == null) {
                Continue();
                return;
            }

            Flowchart flowchart = GetFlowchart();

            sayDialog.gameObject.SetActive(true);

            sayDialog.SetCharacter(角色, flowchart);

            string displayText = 文字內容;

            foreach(CustomTag ct in CustomTag.activeCustomTags) {
                displayText = displayText.Replace(ct.tagStartSymbol, ct.replaceTagStartWith);
                if (ct.tagEndSymbol != "" && ct.replaceTagEndWith != "") {
                    displayText = displayText.Replace(ct.tagEndSymbol, ct.replaceTagEndWith);
                }
            }

            string subbedText = flowchart.SubstituteVariables(displayText);

            sayDialog.Say(subbedText, !追加上文, 等待輸入, 翻轉, 結束時過渡, 文字聲音, delegate { Continue(); });
        }

        public override string GetSummary() {
            return "\"" + 文字內容 + "\"";
        }

        public override Color GetButtonColor() {
            return new Color32(184, 210, 235, 255);
        }

        public override void OnReset() {
            executionCount = 0;
        }

        //
        // ILocalizable implementation
        //

        public virtual string GetStandardText() {
            return 文字內容;
        }

        public virtual void SetStandardText(string standardText) {
            文字內容 = standardText;
        }

        public virtual string GetDescription() {
            return 描述;
        }

        public virtual string GetStringId() {
            string stringId = "SAY." + GetFlowchartLocalizationId() + "." + itemId + ".";
            if (角色 != null) {
                stringId += 角色.nameText;
            }

            return stringId;
        }
    }
}
