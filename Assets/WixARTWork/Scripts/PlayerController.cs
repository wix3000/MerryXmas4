using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public Game_Player game_Player;

    public SkillInputList[] skillList;
    public Animator animator;
    public CharacterController controller;
    public SkeletonAnimator skeletonAnimator;
    // 跳躍速度 #要大於重力
    // 輸入間隔
    public float interval = 0.3f;
    // 重力加速度
    public float grivity = 9.8f;
    public bool setTOSetupPos;
    public float nomralAttackTime;  // 普通攻擊一輪之間的間隔
    float nomralAttackTimer;

    List<KeyPath> inputedKey = new List<KeyPath>();

    bool secendJumped;
    bool isPressed;
    bool backSteping;

    float verticalSpeed;
    AnimatorStateInfo ASI;

    float timer;
    int lastHash;
    float lockedZ;

    // 轉向技能發動時的面向...真是個爛方法 = =a
    int faceOnSkill;

    // Use this for initialization
    void Start () {
        lockedZ = transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {
        ASI = animator.GetCurrentAnimatorStateInfo(0);
        if (!ASCheck("受傷", "死亡")) {
            PressProcess();
            SkillProcess();
            BaseKeyProcess();
        }
        Movement();
        GrivityProcess();
        SetAnimatorArgs();
        if(setTOSetupPos) SetToSetupPose();        
	}

    void FixedUpdate() {
        
    }

    private void SetAnimatorArgs() {
        animator.SetFloat("SkillSpeed", game_Player.ASPD);
        animator.SetFloat("MoveSpeed", game_Player.MovementRate);
        if(!ASCheck("攻擊1", "攻擊2", "攻擊3", "空中攻擊1","空中攻擊2", "空中攻擊3")) nomralAttackTimer -= Time.deltaTime * game_Player.ASPD;
    }

    // 技能處理
    private void SkillProcess() {
        if (!isPressed) return;
        List<SkillInputList> enableList = new List<SkillInputList>();

        // 先挑出有可能的技能清單
        foreach(SkillInputList sl in skillList) {
            if (sl.keyName[sl.keyName.Count -1] == inputedKey[inputedKey.Count -1]) {
                enableList.Add(sl);
            }
        }
        if (enableList.Count == 0) return;

        // 再逐個比對
        foreach(SkillInputList sl in enableList) {
            for (int i = 0; i < sl.keyName.Count; i++) {
                if (inputedKey.Count < sl.keyName.Count || inputedKey[inputedKey.Count - sl.keyName.Count + i] != sl.keyName[i]) break;
                //if () break;
                if (i == sl.keyName.Count - 1) {
                    try {
                        if (game_Player.stamina >= sl.cost) {
                            inputedKey.Clear();
                            game_Player.stamina -= sl.cost;
                            SendMessage(sl.skillName);
                            return;
                        }
                    }
                    catch {}
                }
            }
        }
    }

    #region 技能定義區塊
    void ForwardStep() {
        animator.SetTrigger("ForwardStep");
    }

    void BackStep() {
        animator.SetTrigger("BackStep");
        faceOnSkill = (Mathf.Approximately(transform.localEulerAngles.y, 0)) ? 180 : 0;
    }

    void FlyKick() {
        animator.SetTrigger("FlyKick");
    }

    void SkyKick() {
        animator.SetTrigger("SkyKick");
    }

    void MegaKick() {
        animator.SetTrigger("MegaKick");
    }

    void HalfFlyKick() {
        animator.SetTrigger("HalfFlyKick");
    }

    void DownKick() {
        animator.SetTrigger("DownKick");
    }

    void SlideKick() {
        animator.SetTrigger("SlideKick");
    }

    #endregion


    // 基本按鍵處理
    void BaseKeyProcess() {
        if (!isPressed || inputedKey.Count == 0) return;
        ResetTriggers();

        switch (inputedKey[inputedKey.Count - 1]) {
            case KeyPath.LAttack:   // Z : 攻擊
                NomralAttack();
                break;
            case KeyPath.HAttack:   // X : 重攻擊?
                HeavyAttack();
                break;
            case KeyPath.Forward:   // ← : 前
                //毫無反應，就是往前
                break;
            case KeyPath.Up:        // ↑ : 跳
                Jump();
                break;
            case KeyPath.Down:      // ↓ : 急降下 or 蹲下
                SpeedFallDown();
                break;
            case KeyPath.Back:      // → : 後
                //毫無反應，就是往後
                break;
        }
    }

    #region 基本動作區塊

    void NomralAttack() {
        if (ASCheck("攻擊2", "空中攻擊2")) {
            animator.SetTrigger("Attack3");
            nomralAttackTimer = nomralAttackTime;
        }
        else if (ASI.fullPathHash == Animator.StringToHash("Base Layer.攻擊1") || ASI.fullPathHash == Animator.StringToHash("Base Layer.空中攻擊1")) {
            animator.SetTrigger("Attack2");
            nomralAttackTimer = nomralAttackTime;
        }
        else if (nomralAttackTimer <= 0f) {
            animator.SetTrigger("Attack1");
            nomralAttackTimer = nomralAttackTime;
        }
        
    }

    void HeavyAttack() {
        if (ASI.fullPathHash == Animator.StringToHash("Base Layer.浮空中")) {
            animator.SetBool("InazumaKick", true);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 45f);
            return;
        }
    }

    void SpeedFallDown() {
        if (LevelSystem.jumpHeight < 5) return;
        if (controller.isGrounded) Crouch();    // 蹲下
        else verticalSpeed = 3.5f;  // 急降下
    }

    void Crouch() {
        // 尚未實裝
        return;

    }

    void Jump() {
        if (!ASCheck("待機", "移動", "浮空中", "前踏步", "後踏步") || secendJumped) return;
        if (!controller.isGrounded) {
            if (LevelSystem.jumpHeight < 10) return;
            secendJumped = true;    // 否則如果不在地上，就啟動二段跳
        }
        animator.SetBool("isJumping", true);    // 如果在地上，就跳。
        verticalSpeed = -game_Player.JumpPower;
    }

    #endregion

    private void ResetTriggers() {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("ForwardStep");
        animator.ResetTrigger("BackStep");
    }

    // 重力處理
    private void GrivityProcess() {
        if (controller.isGrounded && verticalSpeed > 0) {
            verticalSpeed = 0;
            animator.SetBool("isJumping", false);
            animator.SetBool("InazumaKick", false);
            secendJumped = false;
        } else {
            animator.SetBool("isJumping", true);
            animator.SetFloat("VerticalSpeed", Mathf.Clamp(verticalSpeed * Time.deltaTime, -1f, 1f));
        }

        verticalSpeed += grivity * Time.deltaTime;
    }

    // 位移處理
    private void Movement() {
        float AxisX = Input.GetAxis("Horizontal");
        float eulerY = transform.localEulerAngles.y;
        float eulerZ = 0f;
        Vector3 v = Vector3.zero;

        // X計算
        if (ASI.fullPathHash == Animator.StringToHash("Base Layer.前踏步")) {
            v = -transform.right * game_Player.StepSpeed * game_Player.MovementRate * Time.deltaTime;
        }
        else if (ASI.fullPathHash == Animator.StringToHash("Base Layer.下下重擊")) {
            v = -transform.right * game_Player.SlideSpeed * Time.deltaTime * game_Player.ASPD;
        }
        else if (ASI.fullPathHash == Animator.StringToHash("Base Layer.上上攻擊")) {
            v = -transform.right * game_Player.FlyKickSpeed * Time.deltaTime * 0.35f * game_Player.ASPD;
        }
        else if (ASI.fullPathHash == Animator.StringToHash("Base Layer.閃電踢")) {
            v = -transform.right * game_Player.FlyKickSpeed * Time.deltaTime * game_Player.ASPD;
        }
        else if (ASI.fullPathHash == Animator.StringToHash("Base Layer.踢完過渡")) {
            v = -transform.right * game_Player.FlyKickSpeed  * Time.deltaTime * game_Player.ASPD * (1 - ASI.normalizedTime);
        }
        else if (ASI.fullPathHash == Animator.StringToHash("Base Layer.後踏步")) {
            v = transform.right * game_Player.StepSpeed * game_Player.MovementRate * Time.deltaTime;
        }
        else if (AxisX != 0 && (ASI.fullPathHash == Animator.StringToHash("Base Layer.移動") || ASI.fullPathHash == Animator.StringToHash("Base Layer.浮空中"))) {
            v.x = AxisX * game_Player.Movement * Time.deltaTime;
        }

        
        v.y = -verticalSpeed * Time.deltaTime;
        // Y計算
        if (ASI.fullPathHash == Animator.StringToHash("Base Layer.上上攻擊")) {
            v.y = game_Player.FlyKickSpeed * Time.deltaTime * game_Player.ASPD;
            verticalSpeed = -15f;
        }
        else if (ASI.fullPathHash == Animator.StringToHash("Base Layer.閃電踢")) {
            v.y = -game_Player.FlyKickSpeed * Time.deltaTime * game_Player.ASPD;
            verticalSpeed = 5f;
        }
        else if (ASCheck("空中攻擊1", "空中攻擊2", "空中攻擊3")) {
            v.y = -2f * Time.deltaTime;
            verticalSpeed = 5f;
        }

        // 轉向處理
        if (ASCheck("移動", "待機", "浮空中") && AxisX != 0) {
            eulerY = (AxisX > 0) ? 180f : 0f;
        }
        else if (ASCheck("後踏步")) {
            eulerY = faceOnSkill;
        } else if (ASCheck("閃電踢")) {
            eulerZ = 45f;
        }

        transform.localEulerAngles = new Vector3(0, eulerY, eulerZ);

        v.z = (lockedZ - transform.position.z);

        controller.Move(v);
        animator.SetBool("isMoving", (AxisX != 0));
    }

    // 按鍵處理
    private void PressProcess() {
        isPressed = false;
        if (Input.GetKeyDown(KeyCode.Z)) AddInputedKey(KeyPath.LAttack);
        if (Input.GetKeyDown(KeyCode.X)) AddInputedKey(KeyPath.HAttack);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) AddInputedKey((Mathf.Approximately(transform.eulerAngles.y, 0f)) ? KeyPath.Forward : KeyPath.Back);
        if (Input.GetKeyDown(KeyCode.RightArrow)) AddInputedKey((Mathf.Approximately(transform.eulerAngles.y, 0f)) ? KeyPath.Back : KeyPath.Forward);
        if (Input.GetKeyDown(KeyCode.UpArrow)) AddInputedKey(KeyPath.Up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) AddInputedKey(KeyPath.Down);

        /*   50CC↓ 特殊表達式看不懂再問吧ㄎㄎ
        AddInputedKey((Mathf.Approximately(transform.eulerAngles.y, 180f)) ? KeyPath.Back : KeyPath.Forward);

        if (Mathf.Approximately(transform.eulerAngles.y, 180)) AddInputedKey(KeyPath.Back);
        else AddInputedKey(KeyPath.Forward);
        上下兩段功能是相等的
        */



        timer += Time.deltaTime;
        if (timer >= interval) inputedKey.Clear();
    }

    // 添加按鍵
    private void AddInputedKey(KeyPath key) {
        inputedKey.Add(key);
        timer = 0;
        isPressed = true;
    }

    private void SetToSetupPose() {
        if (ASI.fullPathHash != lastHash) {
                skeletonAnimator.skeleton.SetToSetupPose();
            lastHash = ASI.fullPathHash;

        }
    }

    /// <summary>
    /// 非常偷懶用的一次檢查是否在數個動畫狀態下的方法。
    /// </summary>
    /// <param name="s">動畫樹名稱</param>
    /// <returns></returns>
    private bool ASCheck(params string[] s) {
        foreach (string clip in s) {
            if (ASI.fullPathHash == Animator.StringToHash("Base Layer." + clip)) return true;
        }
        return false;
    }
}

public enum KeyPath {
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
    public List<KeyPath> keyName;
    [Tooltip("技能名稱")]
    public string skillName;
    [Tooltip("技能耗魔")]
    public float cost;
}

