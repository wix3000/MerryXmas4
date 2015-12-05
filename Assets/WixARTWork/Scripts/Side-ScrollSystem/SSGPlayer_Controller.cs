using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SSG {

    // 橫向捲軸角色控制系統 : Controll層
    // 負責按鍵輸入，以及其他暫時想不到的操作功能。
    public class SSGPlayer_Controller : MonoBehaviour , IPlayerSwitch {

        public List<SkillInputList> skillList = new List<SkillInputList>();  //List是可以輸入大量同樣物品的清單 如果再加上 SkillInputLis 在清單上面再分類好幾條可以輸入的東西 註解:技能輸入介面

        [SerializeField] //序列化 他會讓正下方的變數顯示在ruity中 可以自己設定
        float interval;
        [SerializeField]
        SSGPlayer_Model playerModel;

        List<KeyPath> inputtedKey = new List<KeyPath>();
        bool canControl = true;
        float timer;
        bool isPressed;

        // Use this for initialization
        void Start() {
          if (!playerModel) {                                                 //if (! ) {} 公式為相反判斷內部為false or true, 如:內部為True時才執行中括號內容物,不然執行中括號外部東西,此程式碼外部無東西,所以不執行
                playerModel = GetComponent<SSGPlayer_Model>();                // 公式為 playerModel (取得) SSGPlayer_Model <--類別 (此類別為自己定義的)
                if (!playerModel) {
                    Debug.LogError("玩者控制器找不到可用的玩者模組！");       //LogError 控制台顯示訊息 加上Error會在控制台前多一個驚嘆號
                    enabled = false;   // 找不到player模組就關閉自己          //enabled = false 為關掉此腳本(需在別的腳本再次呼叫啟動)
                }
            }
        }

        // Update is called once per frame
        void Update() {
            //if (Time.timeScale <= 0f) return;
            if (!canControl) return;     //只打一條不用打中括號 ,如果canControl 為true 相反後變成不執行,直接跳過此行執行以下動作  PressProcess();........      補充:return為此函式整個跳過if (!canControl)....SendAxie(); 這段
            PressProcess();
            SkillProcess();
            BaseKeyProcess();
            SendAxie();
        }

        // 按鍵處理
        void PressProcess() {
            isPressed = false; 

            // 將按下的按鍵記錄下來
            if (Input.GetKeyDown(GameOption.Confirm)) InputKey(KeyPath.LAttack); //GameOption為自己定義的類型,可以讓玩家自己改按鍵(右鍵查看定義,在別的腳本)  Confirm 為自己定義的名稱  InputKey 為自己定義的含式(在下面) LAttack為自己定義的列舉(在底下)會在unity裡面顯示讓玩家自己設定用
            if (Input.GetKeyDown(GameOption.Cancel)) InputKey(KeyPath.HAttack);
            if (Input.GetKeyDown(GameOption.Left)) InputKey((Mathf.Approximately(transform.eulerAngles.y, 0f)) ? KeyPath.Back : KeyPath.Forward);  //玩家移動先判斷面向 再判斷輸入的是後還是前   ((Mathf.Approximately())小括弧內維邏輯判斷式,當內容物為真 執行 : 前半段 ,如果為假 執行 : 後半段  助解:transform.eulerAngles.y, 0f 意思為 玩家位子.歐拉角.Y 近似為0 不等於零就是false 
            if (Input.GetKeyDown(GameOption.Right)) InputKey((Mathf.Approximately(transform.eulerAngles.y, 0f)) ? KeyPath.Forward : KeyPath.Back); //玩家移動先判斷面向 再判斷輸入的是後還是前
            if (Input.GetKeyDown(GameOption.Up)) InputKey(KeyPath.Up);
            if (Input.GetKeyDown(GameOption.Down)) InputKey(KeyPath.Down);

            if (Time.time >= timer) inputtedKey.Clear();  //當系統的時間 Time.time 大於>= 自己設定的時間timer 清空inputtedKey內部   補充:當玩家輸入指令的間隔過久就消除 判斷式在下方
        }

        // 記錄按鍵並重設計時器
        void InputKey(KeyPath key) {                //()內key為定義一個空白的抽屜  當玩家按下上列對應按鍵的時候可以裝入 然後啟動以下程式
            inputtedKey.Add(key);

            timer = Time.time + interval; //遊戲的時間+上動作的時間
            isPressed = true;
        }

        // 技能處理
        void SkillProcess() {
            if (!isPressed) return;
            List<SkillInputList> enableList = new List<SkillInputList>();  //List<類名> 取名子 =<--賦予  (new創造一個新的) List<類名 要跟前面的一樣>(); 註解:創造一個新的叫做enableList(自己命名)的空間 用來裝SkillInputList的 List

            // 先挑出有可能的技能清單
            foreach (SkillInputList sl in skillList) {            //節省資源的初步判斷 用來分類輸入兩次的技能or三次的技能or或更多沒設限 註解:如果只輸入兩個,就絕對不會去判斷三個的 
                //配合上方public List<SkillInputList> skillList = new List<SkillInputList>();用 foreach為呼叫出skillList中的每一段技能來進行判斷 比如:當玩家輸入前踏步 S1就會變成前踏步


                // 1. 按鍵數>0
                // 2. 輸入過的按鍵數在清單的按鍵數以上
                if (sl.KeySet.Count > 0 && inputtedKey.Count >= sl.KeySet.Count) {   //KeySet按鍵輸入(在nuity內) Count計算數量
                    enableList.Add(sl);  //如果為真 就裝入enableList中 在上面 用來放輸入成功的技能
                }
            }
            if (enableList.Count == 0) return;

            // 再從可能的清單裡逐個比對其他按鍵是否相同
            foreach (SkillInputList sl in enableList) {
                
                for (int i = 1; i <= sl.KeySet.Count; i++) {  //起始值  最大值(過了就跳出迴圈) i++每次加一
                    if (inputtedKey[inputtedKey.Count - i] != sl.KeySet[sl.KeySet.Count - i]) break; //inputtedKey[inputtedKey.Count - i] 公式為 List(現在命名為inputtedKey)[叫出內部的其中一個東西] List中東西內部的排列為0 1 2 3 4 5
                                                                                                     



                    //inputtedKey[inputtedKey.Count 讓Count回到正確的技能順序判斷上 !=(不等於)

                    if (i == sl.KeySet.Count) {
                        inputtedKey.Clear();
                        playerModel.CastSkill(sl.skillName, sl.cost);
                        return;
                    }
                }
            }
        }

        // 基本按鍵處理
        void BaseKeyProcess() {
            // 如果沒通過技能檢查，就將按下的按鍵傳遞給model層。
            if (!isPressed || inputtedKey.Count == 0) return;
            playerModel.DoBaseAction(inputtedKey[inputtedKey.Count - 1]);
        }

        void SendAxie() {
            playerModel.axisX = Input.GetAxis(GameOption.Horizontal);
            playerModel.axisY = Input.GetAxis(GameOption.Vertical);
        }

        public void SetActive(bool b) {
            canControl = b;
            playerModel.axisX = 0f;
            playerModel.axisY = 0f;
        }
    }

    public enum KeyPath {   //enum
        一,
        Up,
        Down,
        Forward,
        Back,
        LAttack,
        HAttack
    }

    [Serializable]
    public struct SkillInputList {
        [Tooltip("技能按鍵設定")]
        public KeyPath key1, key2, key3, key4, key5, key6;
        [Tooltip("技能名稱")]
        public string skillName;
        [Tooltip("技能耗魔")]
        public float cost;

        public List<KeyPath> KeySet {
            get {
                List<KeyPath> l = new List<KeyPath>() {key1, key2, key3, key4, key5, key6 };
                l.RemoveAll(it => it == KeyPath.一);
                return l;
            }
        }
    }
}