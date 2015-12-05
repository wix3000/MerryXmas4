using UnityEngine;
using System.Collections;

namespace UnityEngine.UI {
    public class FloatText : MonoBehaviour {
        public Text text;
        public RectTransform rectTransform;
        Vector2 floatSpeed = new Vector2(0, 100f);
        float lifeTime;
        float timer;

        void Start() {
            //gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update() {
            timer += Time.deltaTime;
            Color c = text.color;
            c.a = Mathf.Lerp(1f, 0f, timer / lifeTime);
            text.color = c;

            rectTransform.anchoredPosition += floatSpeed * Time.deltaTime;
        }

        public void Display(string content, float lifeTime = 2f) {
            text.text = content;
            this.lifeTime = lifeTime;
            gameObject.SetActive(true);
            Destroy(gameObject, lifeTime);
        }

        public void Display(string content, Vector2 floatSpeed, float lifeTime = 2f) {
            text.text = content;
            this.lifeTime = lifeTime;
            this.floatSpeed = floatSpeed;
            gameObject.SetActive(true);
            Destroy(gameObject, lifeTime);
        }
    }
}
