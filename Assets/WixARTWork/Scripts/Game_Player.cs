using UnityEngine;
using System.Collections;

public class Game_Player : MonoBehaviour {

    [SerializeField]
    int base_MaxHp = 50;   // 基礎最大hp
    [SerializeField]
    int base_MaxSp = 100;   // 基礎最大sp

    public float health;       // hp
    public float stamina;      // sp

    [SerializeField]
    float base_SpRate = 10f;  // 基礎sp回復速度

    [SerializeField]
    int base_Attack = 10;  // 基礎攻擊力
    [SerializeField]
    float base_ASPD = 1f;    // 基礎攻擊速度
    [SerializeField]
    int base_Defence = 5; // 基礎防禦力
    [SerializeField]
    float base_Movement = 25f;    // 基礎移動速度
    [SerializeField]
    float base_JumpPower = 0.6f;   // 基礎跳躍力
    [SerializeField]
    float base_SlideSpeed = 90f;    // 基礎滑步速度
    [SerializeField]
    float base_FlyKickSpeed = 60f;  // 飛踢上升速度
    [SerializeField]
    float base_StepSpeed = 75f; // 基礎踏步速度

    float buff_MaxHp;
    int buff_MaxSp;
    float buff_SpRate;
    int buff_Attack;
    float buff_ASPD;
    int buff_Defence;
    float buff_Movement;
    float buff_JumpPower;
    float buff_SlideSpeed;
    float buff_FlyKickSpeed;
    float buff_StepSpeed;

    public PlayerController controller;    // 玩家控制器
    public LevelSystem lvSystem;            // 等級系統

    public int MaxHP { get { return (int)(base_MaxHp + buff_MaxHp * base_MaxHp); } }
    public int MaxSP { get { return base_MaxSp + buff_MaxSp; } }
    public int Attack { get { return base_Attack + buff_Attack; } }
    public float SpRate { get { return base_SpRate + buff_SpRate; } }
    public float ASPD { get { return base_ASPD + buff_ASPD; } }
    public int Defence { get { return base_Defence + buff_Defence; } }
    public float Movement { get { return base_Movement + buff_Movement; } }
    public float SlideSpeed { get { return base_SlideSpeed + buff_SlideSpeed; } }
    public float JumpPower { get { return base_JumpPower + buff_JumpPower; } }
    public float FlyKickSpeed { get { return base_FlyKickSpeed + buff_FlyKickSpeed; } }
    public float StepSpeed { get { return base_StepSpeed + buff_StepSpeed; } }
    public float MovementRate { get { return Movement / base_Movement; } }


    // Use this for initialization
    void Start() {
        RefreshState();
        health = MaxHP;
        stamina = MaxSP * 0.5f;
    }

    // Update is called once per frame
    void Update() {
        stamina = Mathf.Clamp(stamina + SpRate * Time.deltaTime, 0, MaxSP);
        RefreshState();
    }

    void RefreshState() {
        buff_MaxHp = LevelSystem.maxHpValue * LevelSystem.maxHpRate;
        buff_JumpPower = LevelSystem.jumpHeight * LevelSystem.jumpHeightAdd;
        buff_ASPD = LevelSystem.skillSpeed * LevelSystem.skillRate;
        buff_Movement = LevelSystem.moveSpeed * LevelSystem.moveSpeedAdd;
    }
}
