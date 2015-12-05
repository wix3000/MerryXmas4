using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("Narrative",
             "傳統對話(S)",
             "使用Spine臉圖的傳統對話")]
    [AddComponentMenu("")]
    public class Say_WixVer : Say {

        public string animationName;
        public bool loop = true;
        public float timeScale = 1f;

        public override void OnEnter() {
            if (!showAlways && executionCount >= showCount) {
                Continue();
                return;
            }

            executionCount++;

            // Override the active say dialog if needed
            if (setSayDialog != null) {
                SayDialog.activeSayDialog = setSayDialog;
            }

            SayDialog sayDialog = SayDialog.GetSayDialog();

            if (sayDialog == null) {
                Continue();
                return;
            }

            Flowchart flowchart = GetFlowchart();

            sayDialog.gameObject.SetActive(true);

            sayDialog.SetCharacter(character, flowchart);
            if (sayDialog.GetComponent<SpinePortraitSetter>()) {
                sayDialog.GetComponent<SpinePortraitSetter>().SetCharacterSpinePortrait(character, animationName, loop, timeScale);
            }
            

            string displayText = storyText;

            foreach (CustomTag ct in CustomTag.activeCustomTags) {
                displayText = displayText.Replace(ct.tagStartSymbol, ct.replaceTagStartWith);
                if (ct.tagEndSymbol != "" && ct.replaceTagEndWith != "") {
                    displayText = displayText.Replace(ct.tagEndSymbol, ct.replaceTagEndWith);
                }
            }

            string subbedText = flowchart.SubstituteVariables(displayText);

            sayDialog.Say(subbedText, !extendPrevious, waitForClick, fadeWhenDone, voiceOverClip, delegate {
                Continue();
            });
        }
    }
}