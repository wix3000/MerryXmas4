using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus {
    
    public class BubbleSayDialog : SayDialog {

        public Transform target;
        public Vector3 offset;
        public Image backGround;
        public CanvasScaler cs;

        public override void SetCharacter(Character character, Flowchart flowchart = null) {
            if(character != null) backGround.color = character.nameColor;
            base.SetCharacter(character, flowchart);
        }

        protected override void LateUpdate() {
            Vector3 v = Camera.main.WorldToScreenPoint((target.position + offset));

            v.x = v.x / Camera.main.pixelWidth * cs.referenceResolution.x;
            v.y = v.y / Camera.main.pixelHeight * cs.referenceResolution.y;

            backGround.rectTransform.anchoredPosition = v;

            base.LateUpdate();
        }

        public void SetBackGround(Sprite sprite) {
            backGround.sprite = sprite;
        }
    }
}
