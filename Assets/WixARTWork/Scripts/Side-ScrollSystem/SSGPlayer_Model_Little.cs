using UnityEngine;
using System.Collections;

namespace SSG {
    public class SSGPlayer_Model_Little : SSGPlayer_Model {

        #region 技能定義區塊
        void ForwardStep() {
            animator.SetTrigger("Step");
            frameSpeed = transform.right * unit.StepSpeed * unit.MovementRate;
        }

        void BackStep() {
            if (ASCheck("待機", "移動", "浮空中")) {
                unit.stamina += 10f;
                return;
            }
            transform.eulerAngles = (transform.eulerAngles.y == 0f) ? new Vector3(0f, 180f) : Vector3.zero;
            ForwardStep();
        }

        void FlyBall() {
            animator.SetTrigger("FlyBall");
            frameSpeed = transform.right * unit.FlyKickSpeed * 0.35f * unit.ASPD;
            frameSpeed.y = unit.FlyKickSpeed * unit.ASPD;
            verticalSpeed = 15f;
        }

        void GossipingBall() {
            animator.SetTrigger("GossipingBall");
        }

        void DarkBall() {
            animator.SetTrigger("EnterBall");
            animator.SetBool("DarkBall", true);
        }

        void SlideAttack() {
            animator.SetTrigger("SlideAttack");
            frameSpeed = transform.right * unit.SlideSpeed * unit.ASPD;
        }

        public override void CastSkill(string skillname, float cost) {
            base.CastSkill(skillname, cost);
            animator.SetBool("JumpX", false);
            animator.SetBool("DarkBall", false);
        }

        #endregion

        #region 基本動作區塊

        protected override void NomralAttack() {
            ResetTriggers();
            if (ASCheck("球型")) {
                animator.SetBool("DarkBall", false);
                return;
            }
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
            if (ASCheck("浮空中")) {
                animator.SetBool("JumpX", true);
                frameSpeed = transform.right * unit.FlyKickSpeed * unit.ASPD;
                frameSpeed.y = -unit.FlyKickSpeed * unit.ASPD;
                verticalSpeed = -1f;
            }
        }

        protected override void Jump() {
            if (!ASCheck("待機", "移動", "浮空中", "踏步") || secendJumped) return;
            if (!isGround) {
                secendJumped = true;    // 否則如果不在地上，就啟動二段跳
            }
            verticalSpeed = unit.JumpPower; // 如果在地上，就跳。
        }

        #endregion

        private void ResetTriggers() {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
        }

        protected override Vector2 CalculateSpeed() {
            if (ASCheck("球型")) {
                applyGrivity = false;
                verticalSpeed = 20f;
                return Vector3.ClampMagnitude(new Vector3(axisX, axisY), 1f) * unit.Mobility;
            }

            if (ASCheck("攻擊1", "攻擊2", "攻擊3", "空中X", "飛天球") && !isGround) {
                applyGrivity = false;
                return frameSpeed;
            }
            Vector2 v = new Vector2();
            applyGrivity = true;

            if (ASCheck("踏步", "滑行推掌")) {
                v = frameSpeed;
            }
            else if (axisX != 0 && ASCheck("移動", "浮空中")) {
                v.x = axisX * unit.Mobility;
            }

            // Y計算
            v.y = verticalSpeed;

            return v;
        }

        protected override void TimerProcess() {
            base.TimerProcess();
            if (!ASCheck("球型")) return;
            float cast = 25f * Time.deltaTime;
            if (unit.stamina < cast) {
                animator.SetBool("DarkBall", false);
                return;
            }
            unit.stamina -= cast;

        }

        protected override Vector3 CalculateRotation() {
            float eulerY = transform.eulerAngles.y;
            float eulerZ = 0f;

            if (ASCheck("移動", "待機", "浮空中") && axisX != 0) {
                eulerY = (axisX > 0) ? 0 : 180f;
            }
            //if (ASCheck("空中X")) {
            //    eulerZ = -45f;
            //}

            return new Vector3(0f, eulerY, eulerZ);
        }

        protected override void GroundCheck() {
            base.GroundCheck();
            if (ASCheck("空中X") && isGround) {
                animator.SetBool("JumpX", false);
                applyGrivity = true;
            }
        }
    }
}
