using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


namespace Fungus {
    [CommandInfo("Narrative",
                 "泡泡框效果",
                 "設置泡泡對話框要播放的特效")]
    [AddComponentMenu("")]
    public class BubbleDialogEffect : Command {

        public BubbleDialog 對話框;
        public BubbleEffectType 特效類型;
        public float 持續時間 = 1f;
        public float 強度 = 30f;

        public override void OnEnter() {
            if (對話框 == null) 對話框 = BubbleDialog.GetBubbleDialog();
            對話框.PlayEffect(特效類型, 持續時間, 強度);
            Continue();
        }
    }

    public enum BubbleEffectType {
        震動,
        晃動,
        彈動,
        閃光
    }
}