using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Fungus {

    // 用於實現以鍵盤控制fungus內建選單系統的組件
    public class MenuInputer : MonoBehaviour {

        public Button[] cachedButtons;
        public Image indexImg;
        public int index;
        public RectTransform buttonGroup;

        List<Button> enabledButtons = new List<Button>();
        Vector2 target;

        void Awake() {
            if (cachedButtons == null) cachedButtons = GetComponentsInChildren<Button>();
            if (buttonGroup == null) buttonGroup = transform.Find("ButtonGroup").GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update() {
            if (enabledButtons.Count == 0) return;

            // 讓指標以內插法去接近目前選中的選項
            Vector3 v = indexImg.rectTransform.position;
            v.y = Mathf.Lerp(v.y, enabledButtons[index].transform.position.y, 0.20f);
            indexImg.rectTransform.position = v;

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                index = (int)Mathf.Repeat(index - 1, enabledButtons.Count);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                index = (int)Mathf.Repeat(index + 1, enabledButtons.Count);
            }

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space)) {
                enabledButtons[index].onClick.Invoke();
            }
        }

        public void SetIndex(int i) {
            index = i;
        }

        //每當選單被啟用時，抓取所有已啟用的按扭並將游標復位。
        void OnEnable() {
            enabledButtons.Clear();
            index = 0;
            indexImg.GetComponent<CanvasGroup>().alpha = 0f;
            StartCoroutine(WaitForShowIndex());
        }

        IEnumerator WaitForShowIndex() {
            yield return null;
            float width = 0;

            // 求出所有按扭中寬度最寬的值
            foreach (Button b in cachedButtons) {
                if(b.gameObject.activeSelf) {
                    width = Mathf.Max(width, b.GetComponentInChildren<Text>().preferredWidth);
                    enabledButtons.Add(b);
                }
            }

            // 將指標設得比最寬的按扭再寬一點，以顯示指標圖示
            indexImg.rectTransform.sizeDelta = new Vector2(width + 100f, indexImg.rectTransform.sizeDelta.y);
            indexImg.rectTransform.position = new Vector3(indexImg.rectTransform.position.x, enabledButtons[index].transform.position.y, indexImg.transform.position.z);
            indexImg.GetComponent<CanvasGroup>().alpha = 1f;

            // 順便為了無意義的滑鼠選擇系統，也改變按鈕的大小
            buttonGroup.sizeDelta = new Vector2(width + 100f, buttonGroup.sizeDelta.y);
        }
    }
}