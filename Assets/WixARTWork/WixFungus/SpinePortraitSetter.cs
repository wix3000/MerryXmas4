using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus {

    public class SpinePortraitSetter : MonoBehaviour {

        public SkeletonAnimation SpinePortrait { get; private set; }

        [SerializeField]
        private Transform _facePos;

        public Transform FacePos {
            get {
                if (!_facePos) _facePos = transform.Find("Panel/FacePos");
                return _facePos;
            }
        }

        public void SetCharacterSpinePortrait(Character character, string animationName, bool loop, float timeScale) {
            // 給空物件則清空子物件
            if (!character) {
                DestroyPortrait();
                return;
            }
            SkeletonAnimation portraitPrefab = character.spineProfile;
            // 是否更換臉圖
            if (SpinePortrait == null || SpinePortrait.skeletonDataAsset != portraitPrefab.skeletonDataAsset) {
                DestroyPortrait();
                SpinePortrait = Instantiate(portraitPrefab) as SkeletonAnimation;
                SpinePortrait.transform.SetParent(FacePos);
                SpinePortrait.transform.localPosition = portraitPrefab.transform.position;
                SpinePortrait.transform.rotation = portraitPrefab.transform.localRotation;
                SpinePortrait.transform.localScale = portraitPrefab.transform.localScale;
            }

            if (animationName == "") {
                return;
            }
           
            SpinePortrait.AnimationName = animationName;
            SpinePortrait.loop = loop;
            SpinePortrait.timeScale = timeScale;
            SpinePortrait.Skeleton.SetToSetupPose();
        }

        void DestroyPortrait() {
            if (SpinePortrait) Destroy(SpinePortrait.gameObject);
        }

        void OnDisable() {
            if(SpinePortrait) Destroy(SpinePortrait.gameObject);
        }
    }
}