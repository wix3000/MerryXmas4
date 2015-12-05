using UnityEngine;
using System.Collections;
using System;

public class LevelSystem : MonoBehaviour {

    public UITest uitest;    // 測試ui用

    public static int playerLevel = 1;     // 玩家等級
    public static int skillPoint;          // 技能點數
    public static int needExperience = 10;      // 需求經驗
    public static int experience;          // 當前經驗
    
    public static int maxHpValue;       // hp最大值
    public static int jumpHeight;       // 跳躍高度
    public static int skillSpeed;       // 技能速度
    public static int moveSpeed;        // 移動速度

    public static float maxHpRate = 0.1f;       // 每級hp比率增加率
    public static float jumpHeightAdd = 0.03f;  // 每級跳躍高度增加量
    public static float skillRate = 0.05f;      // 每級技能速度增加率
    public static float moveSpeedAdd = 1f;      // 每級移動速度增加量

    public static Action OnLevelUp;

    public const int MAX_PLAYER_LEVEL = 60;   // 最大玩家等級

    int levelmax;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        TestForLevelSystem();
    }

    private void TestForLevelSystem() {
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            LevelUp();
            print("目前技能點數: " + skillPoint);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (!CheckSP()) return;
            maxHpValue = Mathf.Clamp((Input.GetKey(KeyCode.LeftShift)) ? maxHpValue - 1 : maxHpValue + 1, 0, 10);
            print("目前HP最大值技能等級: " + maxHpValue);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (!CheckSP()) return;
            jumpHeight = Mathf.Clamp((Input.GetKey(KeyCode.LeftShift)) ? jumpHeight - 1 : jumpHeight + 1, 0, 10);
            print("目前跳躍高度技能等級: " + jumpHeight);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if (!CheckSP()) return;
            skillSpeed = Mathf.Clamp((Input.GetKey(KeyCode.LeftShift)) ? skillSpeed - 1 : skillSpeed + 1, 0, 10);
            print("目前技能速度技能等級: " + skillSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            if (!CheckSP()) return;
            moveSpeed = Mathf.Clamp((Input.GetKey(KeyCode.LeftShift)) ? moveSpeed - 1 : moveSpeed + 1, 0, 10);
            print("目前技能速度技能等級: " + moveSpeed);
        }

        if (Input.anyKeyDown) {
            switch (levelmax) {
                case 0:
                    if (Input.GetKeyDown(KeyCode.W)) levelmax++;
                    else levelmax = 0;
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.I)) levelmax++;
                    else levelmax = 0;
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.X)) {
                        LevelUp(MAX_PLAYER_LEVEL - playerLevel);
                        skillPoint = 0;
                        maxHpValue = 10;
                        jumpHeight = 10;
                        skillSpeed = 10;
                        moveSpeed = 10;
                    }
                    else levelmax = 0;
                    break;
            }
        }

    }

    public static void AddExp(int exp) {
        experience += exp;
        while(experience >= needExperience) {
            experience -= needExperience;
            LevelUp();
        }
        print(experience);
    }


    /// <summary>
    /// 檢查技能點是否足夠。
    /// </summary>
    /// <param name="value">所需技能點</param>
    bool CheckSP(int value = 1) {
        if(skillPoint < value) {
            print("技能點數不足！");
            return false;
        }
        skillPoint -= value;
        return true;
    }


    static void LevelUp(int value = 1) {
        if (value < 0) {
            Debug.LogError("升級數量不可小於0！");
            return;
        }
        value = Mathf.Clamp(value, 0, MAX_PLAYER_LEVEL - playerLevel);    // 限制等級在最大值內
        playerLevel += value;
        skillPoint += value;
        needExperience *= (int)Mathf.Pow(2f, value);
        if (OnLevelUp != null) OnLevelUp();
    }
}
