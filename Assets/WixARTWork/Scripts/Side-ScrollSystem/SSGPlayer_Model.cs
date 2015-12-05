using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSG {

    // 橫向捲軸角色控制系統 : Model層 + View層
    // 負責底層運算，功能實作。
    // View層的功能大部份由Animator實現，故合併於此類。
    public class SSGPlayer_Model : MonoBehaviour {

        public Game_Unit unit;
        public SkeletonAnimator skeletonAnimator;
        public Animator animator;
        protected AnimatorStateInfo ASI;
        int lastHash;

        public float axisX;
        public float axisY;
        public Vector2 groundCheckOffect;
        public float nomralAttackTime;  // 普通攻擊一輪之間的間隔
        protected float nomralAttackTimer;

        public bool isGround;  // 是否接地
        protected bool secendJumped;

        protected Vector2 frameSpeed;       // 受控制時的幀速度
        protected float verticalSpeed;    // 垂直速度 (用於實現重力)
        protected float grivity = 58.8f;   // 預設重力值
        protected bool applyGrivity = true;   // 是否應用重力

        protected float timer;
        protected bool isStiffing;

        public List<BuffEffect> buffs = new List<BuffEffect>();

        public bool setToSetupPose;

        // 轉向技能發動時的面向...真是個爛方法 = =a
        protected float faceOnSkill;


        // Use this for initialization
        protected virtual void Start() {
            if (!unit) {
                unit = GetComponent<Game_Unit>();
                if (!unit) {
                    unit = gameObject.AddComponent<Game_Unit>();
                }
            }
            if (!skeletonAnimator) skeletonAnimator = GetComponentInChildren<SkeletonAnimator>();
            if (!animator) animator = GetComponentInChildren<Animator>();
            unit.onDeath += () => OnDeath();
        }

        // Update is called once per frame
        protected void Update() {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;
            ASI = animator.GetCurrentAnimatorStateInfo(0);
            if (setToSetupPose) SetToSetupPose();
            if (unit.IsDead) return;
            Movement();
            BuffProcess();
            SetAnimatorArgs();
            TimerProcess();
        }

        protected virtual void BuffProcess() {
            isStiffing = false;
            for(int i = 0; i < buffs.Count; i++) {
                buffs[i].OnEffect();
            }
        }

        protected virtual void TimerProcess() {
            if (!ASCheck("攻擊1", "攻擊2", "攻擊3")) nomralAttackTimer -= Time.deltaTime * unit.ASPD;
        }

        protected void LateUpdate() {
            GroundCheck();
            GrivityProcess();
        }

        protected virtual void Movement() {
            Vector2 speed = CalculateSpeed();
            Vector3 euler = CalculateRotation();

            transform.eulerAngles = euler;
            transform.Translate( speed * Time.deltaTime, Space.World);
            animator.SetBool("isMoving", (Mathf.Abs(axisX) > float.Epsilon));
        }

        protected virtual Vector2 CalculateSpeed() {
            Vector2 v = new Vector2();
            // X計算
            if (Mathf.Abs(axisX) > float.Epsilon && ASCheck("移動", "浮空中")) {
                v.x = axisX * unit.Mobility;
            }

            // Y計算
            v.y = verticalSpeed;

            return v;
        }

        protected virtual Vector3 CalculateRotation() {
            float eulerY = transform.eulerAngles.y;
            float eulerZ = 0f;

            if (ASCheck("移動", "待機", "浮空中") && Mathf.Abs(axisX) > float.Epsilon) {
                eulerY = (axisX > 0) ? 0 : 180f;
            }
            return new Vector3(0f, eulerY, eulerZ);
        }

        // 重力處理
        protected virtual void GrivityProcess() {
            if (!applyGrivity) return;
            if (isGround && verticalSpeed <= 0f) {
                verticalSpeed = 0f;
            }
            verticalSpeed -= grivity * Time.deltaTime;
        }

        public virtual void CastSkill(string skillname, float cost) {
            if (unit.stamina >= cost) {
                unit.stamina -= cost;
                Invoke(skillname, 0f);
            }
        }

        public virtual void DoBaseAction(KeyPath key) {
            switch (key) {
                case KeyPath.LAttack:   // Z : 攻擊
                    NomralAttack();
                    break;
                case KeyPath.HAttack:   // X : 重攻擊?
                    HeavyAttack();
                    break;
                case KeyPath.Forward:   // ← : 前
                    Forward();                    //毫無反應，就是往前
                    break;
                case KeyPath.Up:        // ↑ : 跳
                    Jump();
                    break;
                case KeyPath.Down:      // ↓ : 急降下
                    SpeedFallDown();
                    break;
                case KeyPath.Back:      // → : 後
                                        //毫無反應，就是往後
                    break;
            }
        }

        #region 基本動作區塊

        protected virtual void SpeedFallDown() {
            if(!isGround) verticalSpeed = -175f;  // 急降下
        }

        protected virtual void Jump() {
            if (!ASCheck("待機", "移動", "浮空中") || secendJumped) return;
            if (!isGround) {
                secendJumped = true;    // 否則如果不在地上，就啟動二段跳
            }
            verticalSpeed = unit.JumpPower; // 如果在地上，就跳。
        }

        protected virtual void HeavyAttack() {
            
        }

        protected virtual void NomralAttack() {
            if(nomralAttackTimer <= 0f) {
                animator.SetTrigger("Attack1");
                nomralAttackTimer = nomralAttackTime;
            }
        }

        protected virtual void Forward() {

        }

        #endregion

        public virtual void OnAttacken(AttackTrigger AT) {
            ResistantList r = new ResistantList();
            float damage = AT.unit.Attack * AT.attackRate * (1f + (AT.unit.Attack - unit.Defence) / (unit.Defence + 50f));
            r.stiff = AT.attackEffect.stiff * 0.2f * (2f - unit.Resistant.stiff / 3f);
            r.floating = AT.attackEffect.floating * 0.2f * (2f - unit.Resistant.floating / 3f);
            r.poisoning = AT.attackEffect.poisoning * 0.2f * (2f - unit.Resistant.poisoning / 3f);
            r.repulse = AT.attackEffect.repulse * 0.2f * (2f - unit.Resistant.repulse / 3f);
            Debug.LogFormat("傷害{0}，暈眩值{1}，浮空值{2}，中毒值{3}，擊退值{4}", damage, r.stiff, r.floating, r.poisoning, r.repulse);
            Stiff(r.stiff);
            Float(r.floating);
            Poisoning(r.poisoning);
            Repulse(r.repulse, (Mathf.Approximately(AT.transform.root.eulerAngles.y, 0f)) ? 160f : -160f);
            unit.GivenDamage(damage);
            verticalSpeed = 2f;
        }


        // 硬直
        protected virtual void Stiff(float value) {
            if (value <= 0f) return;
            animator.SetTrigger("Damage");
            animator.SetBool("isStiffing", true);
            new BuffEffect("Stiff", value).SetEffect( 
                buff => {
                isStiffing = true;
            }).Addin(this);
        }

        // 浮空
        protected virtual void Float(float value) {
            if (value <= 0f) return;
            verticalSpeed = 10f;
            new BuffEffect("Float", value / 2f).SetEffect( buff => 
            {
                applyGrivity = false;
                transform.Translate(Vector3.up * 70f * Time.deltaTime, Space.World);
            }).SetExit( buff =>
            {
                applyGrivity = true;
            }).Addin(this);
        }

        // 中毒
        protected virtual void Poisoning(float value) {
            if (value <= 0f) return;
            new BuffEffect("Poisoning", 20f).SetEnter(buff =>
            {
                buff.arg1 = Time.time + 1f;
            }).SetEffect(buff =>
            {
                if(Time.time > buff.arg1) {
                    unit.GivenDamage(unit.MaxHp * 0.03f);
                    buff.arg1++;
                }
            }).SetParticleEffect("Bio","Head")
            .Addin(this);
        }

        // 擊退
        protected virtual void Repulse(float value, float speed) {
            if (value <= 0f) return;
            new BuffEffect("Repulse", value / 3f).SetEffect(buff =>
            {
                if (isGround) {
                    transform.Translate(Vector3.right * (1f - buff.NomralizatTime) * speed * Time.deltaTime, Space.World);
                } else {
                    transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
                }
            }).Addin(this);
        }

        protected virtual void OnDeath() {
            if (tag == "Player") {
                GameObject.Find("Flowchart").GetComponent<Fungus.Flowchart>().SendFungusMessage("PlayerDeath");
            } else if (tag == "Enemy") {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 1) {
                    GameObject.Find("Flowchart").GetComponent<Fungus.Flowchart>().SendFungusMessage("EnemyClear");
                }
            }
            tag = "Dead";
            GetComponent<IPlayerSwitch>().SetActive(false);
            Destroy(gameObject, 1f);
            animator.SetBool("isDead", true);
            
        }

        protected virtual void SetAnimatorArgs() {
            animator.SetBool("isStiffing", isStiffing);
        }

        protected virtual void GroundCheck() {
            // 於角色的腳底建立一塊檢查區域，如果有碰撞器在內，則將角色視為接地
            isGround = Physics2D.OverlapArea(transform.position - (Vector3)groundCheckOffect, transform.position + (Vector3)groundCheckOffect, LayerMask.GetMask("Floor"));
            animator.SetBool("isJumping", !isGround);
            if (!isGround) animator.SetFloat("VerticalSpeed", Mathf.Clamp(verticalSpeed * 0.02f, -1f, 1f));
            else secendJumped = false;
        }

        void SetToSetupPose() {
            if (ASI.fullPathHash != lastHash) {
                skeletonAnimator.skeleton.SetToSetupPose();
                lastHash = ASI.fullPathHash;
            }
        }

        // 演出用

        /// <summary>
        /// 轉向
        /// </summary>
        /// <param name="angle">0: 右 1: 左</param>
        public virtual void Rotate(int angle) {
            transform.eulerAngles = Vector3.up * 180f * angle;
        }

        /// <summary>
        /// 移動一段時間
        /// </summary>
        /// <param name="axisX">x軸</param>
        /// <param name="time">時間</param>
        /// <returns></returns>
        public virtual IEnumerator MoveOnTime(float axisX, float time) {
            this.axisX = axisX;
            yield return new WaitForSeconds(time);
            this.axisX = 0f;
        }

        /// <summary>
        /// 強制跳躍
        /// </summary>
        /// <param name="jumpPower">跳躍力，輸入0則代入角色資料內的高度</param>
        public virtual void ForceJump(float jumpPower) {
            verticalSpeed = (Mathf.Abs(jumpPower) > float.Epsilon) ? jumpPower : unit.JumpPower;
        }
        
        /// <summary>
        /// 施展技能
        /// </summary>
        /// <param name="skillname">技能名</param>
        public virtual void UseSkill(string skillname) {
            Invoke(skillname, 0f);
        }

        /// <summary>
        /// 非常偷懶用的一次檢查是否在數個動畫狀態下的方法。
        /// </summary>
        /// <param name="s">動畫樹名稱</param>
        /// <returns></returns>
        public bool ASCheck(params string[] s) {
            foreach (string clip in s) {
                //if (ASI.fullPathHash == Animator.StringToHash("Base Layer." + clip)) return true;
                if (ASI.IsName("Base Layer." + clip)) return true;
            }
            return false;
        }
    }
}