using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

namespace Fungus {

    public class ContinueAnimate : MonoBehaviour {

        public Image image;
        public float fillTime;
        Tweener tweener;

        // Use this for initialization
        void Start() {
            if (image == null) enabled = false;
        }

        // Update is called once per frame
        void Update() {

        }

        void OnEnable() {
            if (tweener != null && tweener.IsActive()) tweener.Kill();
            image.fillAmount = 0f;
            tweener = image.DOFillAmount(1f, fillTime);
        }
    }
}
