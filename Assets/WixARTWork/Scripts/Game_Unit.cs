using UnityEngine;
using System.Collections;

public class Game_Unit : MonoBehaviour {

    public Game_UnitData sourceData;

    [SerializeField]
    int maxHp = 50;   // 基礎最大hp
    [SerializeField]
    int maxSp = 100;   // 基礎最大sp

    public float health;       // hp
    public float stamina;      // sp

    [SerializeField]
    float spRate = 10f;  // 基礎sp回復速度

    [SerializeField]
    int attack = 10;                // 基礎攻擊力
    [SerializeField]
    float attackSpeed = 1f;         // 基礎攻擊速度
    [SerializeField]
    int defence = 5;                // 基礎防禦力
    [SerializeField]
    float mobility = 25f;           // 移動速度
    [SerializeField]
    float jumpPower = 25f;         // 基礎跳躍力
    [SerializeField]
    float slideSpeed = 90f;         // 基礎滑步速度
    [SerializeField]
    float flyKickSpeed = 60f;       // 飛踢上升速度
    [SerializeField]
    float stepSpeed = 75f;          // 基礎踏步速度

    [SerializeField]
    ResistantList resistant = new ResistantList();

    public int MaxHp { get; set; }
    public int MaxSp { get; set; }
    public int Attack { get; set; }
    public float SpRate { get; set; }
    public float ASPD { get; set; }
    public int Defence { get; set; }
    public float Mobility { get; set; }
    public float SlideSpeed { get; set; }
    public float JumpPower { get; set; }
    public float FlyKickSpeed { get; set; }
    public float StepSpeed { get; set; }
    public ResistantList Resistant { get; set; }
    public float MovementRate { get { return Mobility / mobility; }}
    public bool IsDead { get; private set; }

    public System.Action onDeath;

    protected virtual void Awake() {
        Initialize();
        health = MaxHp;
        stamina = maxSp * 0.7f;
    }

    protected virtual void Initialize() {
        MaxHp = maxHp;
        MaxSp = maxSp;
        Attack = attack;
        SpRate = spRate;
        ASPD = attackSpeed;
        Defence = defence;
        Mobility = mobility;
        SlideSpeed = slideSpeed;
        JumpPower = jumpPower;
        FlyKickSpeed = flyKickSpeed;
        StepSpeed = stepSpeed;
        Resistant = resistant;
    }

    protected virtual void Update() {
        stamina = Mathf.Clamp(stamina + SpRate * Time.deltaTime, 0, MaxSp);

        if(gameObject.tag != "Player" && Input.GetKeyDown(KeyCode.Y)) {
            GivenDamage(10f);
        }
    }

    public virtual void GivenDamage(float damage) {
        health -= damage;
        if (health <= 0f) OnDeath();
    }

    protected virtual void OnDeath() {
        IsDead = true;
        if (onDeath != null) {
            onDeath();
        }
    }
}

[System.Serializable]
public struct ResistantList {
    /// <summary>
    /// 硬直
    /// </summary>
    public float stiff;
    /// <summary>
    /// 浮空
    /// </summary>
    public float floating;
    /// <summary>
    /// 中毒
    /// </summary>
    public float poisoning;
    /// <summary>
    /// 擊退
    /// </summary>
    public float repulse;

    public static ResistantList zero = new ResistantList(0f, 0f, 0f, 0f);

    public ResistantList(float stiff, float floating, float poisoning, float repulse) {
        this.stiff = stiff;
        this.floating = floating;
        this.poisoning = poisoning;
        this.repulse = repulse;
    }

}
