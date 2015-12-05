using UnityEngine;
using System.Collections;

namespace SSG {
    public class SSGPlayer_Model_Oni : SSGPlayer_Model {

        new Collider2D collider;
        float runTimer;
        float lastAxisX;

        protected override void Start() {
            base.Start();
            collider = GetComponent<Collider2D>();
        }

        #region 技能定義區塊

        void Oni_ForwardWhip() {
            animator.SetTrigger("ForwardWhip");
            animator.SetTrigger("Onigiri");
        }

        void Oni_DownWhip() {
            animator.SetTrigger("DownWhip");
            animator.SetTrigger("Onigiri");
        }

        void ForwardWhip() {
            animator.SetTrigger("ForwardWhip");
        }

        void DownWhip() {
            animator.SetTrigger("DownWhip");
        }

        void UpWhip() {
            animator.SetTrigger("UpWhip");
        }

        void Charge() {
            animator.SetTrigger("Charge");
            frameSpeed = transform.right * unit.SlideSpeed;
        }

        void Slam() {
            animator.SetTrigger("Slam");
        }

        #endregion

        #region 基本動作區塊

        protected override void NomralAttack() {
            ResetTriggers();
            if (ASCheck("攻擊2")) {
                animator.SetTrigger("Attack3");
                nomralAttackTimer = nomralAttackTime;
                return;
            }
            if (ASCheck("攻擊1")) {
                animator.SetTrigger("Attack2");
                nomralAttackTimer = nomralAttackTime;
                return;
            }
            if (ASCheck("待機", "移動", "浮空中") && nomralAttackTimer <= 0f) {
                animator.SetTrigger("Attack1");
                nomralAttackTimer = nomralAttackTime;
                if (!isGround) {
                    frameSpeed.x = unit.Mobility * axisX * 0.3f;
                    frameSpeed.y = verticalSpeed * 0.1f;
                }
            }
        }

        protected override void HeavyAttack() {
            if (ASCheck("浮空中", "空中連踢", "空中連踢2") && unit.stamina >= 10f) {
                animator.SetTrigger("HAttackonSky");
                frameSpeed.x = 0f;
                frameSpeed.y = Mathf.Clamp(verticalSpeed * 0.1f, -1.5f, 1.5f);
                unit.stamina -= 10f;
            }
        }

        protected override void Jump() {
            if (!ASCheck("待機", "移動", "浮空中", "奔跑") || secendJumped) return;
            if (!isGround) {
                secendJumped = true;    // 否則如果不在地上，就啟動二段跳
            }
            verticalSpeed = unit.JumpPower; // 如果在地上，就跳。
        }

        protected override void Forward() {
            if(Time.time > runTimer) {
                runTimer = Time.time + 0.4f;
                return;
            }
            animator.SetBool("isRunning", true);
            print("Run");
        }

        #endregion

        void ResetTriggers() {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
        }

        protected override Vector2 CalculateSpeed() {
            if (gameObject.layer == 14) gameObject.layer = 11;
            if (ASCheck("球型")) {
                applyGrivity = false;
                verticalSpeed = 20f;
                return Vector3.ClampMagnitude(new Vector3(axisX, axisY), 1f) * unit.Mobility;
            }

            if (ASCheck("攻擊1", "攻擊2", "攻擊3", "空中連踢", "空中連踢2") && !isGround) {
                applyGrivity = false;
                return frameSpeed;
            }
            if (ASCheck("衝鋒")) {
                applyGrivity = false;
                return frameSpeed;
            }
            Vector2 v = new Vector2();
            applyGrivity = true;

            if (ASCheck("奔跑")) {
                v.x = (axisX < 0f) ? -unit.StepSpeed : unit.StepSpeed;
                gameObject.layer = 14;
            }
            else if (Mathf.Abs(axisX) > Mathf.Epsilon && ASCheck("移動", "浮空中")) {
                v.x = axisX * unit.Mobility;
            }

            // Y計算
            v.y = verticalSpeed;

            return v;
        }

        protected override void TimerProcess() {
            base.TimerProcess();
            if(animator.GetBool("isRunning")) {
                if(Mathf.Abs(lastAxisX) > Mathf.Abs(axisX)) {
                    animator.SetBool("isRunning", false);
                    lastAxisX = 0f;
                    return;
                }
                lastAxisX = axisX;
            }
            if (!ASCheck("奔跑")) return;
            float cast = 20f * Time.deltaTime;
            if (unit.stamina < cast) {
                animator.SetBool("isRunning", false);
                return;
            }
            unit.stamina -= cast;

        }

        protected override Vector3 CalculateRotation() {
            float eulerY = transform.eulerAngles.y;
            float eulerZ = 0f;

            if (ASCheck("移動", "待機", "浮空中") && Mathf.Abs(axisX) > Mathf.Epsilon) {
                eulerY = (axisX > 0) ? 0 : 180f;
            }

            return new Vector3(0f, eulerY, eulerZ);
        }

        protected override void GroundCheck() {
            base.GroundCheck();
            if (ASCheck("空中連踢", "空中連踢2") && isGround) {
                animator.ResetTrigger("HAttackonSky");
                applyGrivity = true;
            }
        }
    }
}