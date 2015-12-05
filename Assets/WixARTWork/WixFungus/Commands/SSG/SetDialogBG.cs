using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Fungus {
    [CommandInfo("Narrative",
                 "設置背景",
                 "設置泡泡對話的背景")]
    [AddComponentMenu("")]
    public class SetDialogBG : Command {

        public BubbleDialog 對話框;
        public Sprite 圖片;

        public override void OnEnter() {
            if (對話框 == null) 對話框 = BubbleDialog.GetBubbleDialog();
            對話框.SetBackGround(圖片);

            Continue();
        }
    }
}