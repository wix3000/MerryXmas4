using UnityEngine;
using System.Collections;

namespace SSG {

    public class SSGPlayer_Model_Rabbit : SSGPlayer_Model {

        bool backSteping;

        #region 技能定義區塊
        void ForwardStep() {
            animator.SetTrigger("ForwardStep");
            frameSpeed = transform.right * unit.StepSpeed * unit.MovementRate;
        }

        void BackStep() {
            if(ASCheck("待機", "移動", "浮空中")) {
                unit.stamina += 10f;
                return;
            }
            animator.SetTrigger("BackStep");
            frameSpeed = -transform.right * unit.StepSpeed * unit.MovementRate;
        }

        void FlyKick() {
            animator.SetTrigger("FlyKick");
            frameSpeed = transform.right * unit.FlyKickSpeed * 0.35f * unit.ASPD;
            frameSpeed.y = unit.FlyKickSpeed * unit.ASPD;
            verticalSpeed = 15f;
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
            frameSpeed = transform.right * unit.SlideSpeed * unit.ASPD;
        }

        #endregion

        public override void CastSkill(string skillname, float cost) {
            base.CastSkill(skillname, cost);
            animator.SetBool("InazumaKick", false);
        }

        #region 基本動作區塊

        protected override void NomralAttack() {
            ResetTriggers();
            if (ASCheck("攻擊2", "空中攻擊2")) {
                animator.SetTrigger("Attack3");
                nomralAttackTimer = nomralAttackTime;
            }
            else if (ASCheck("攻擊1", "空中攻擊1")) {
                animator.SetTrigger("Attack2");
                nomralAttackTimer = nomralAttackTime;
            }
            else if (ASCheck("待機", "移動", "浮空中") && nomralAttackTimer <= 0f) {
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
                animator.SetBool("InazumaKick", true);
                frameSpeed = transform.right * unit.FlyKickSpeed * unit.ASPD;
                frameSpeed.y = -unit.FlyKickSpeed * unit.ASPD;
                verticalSpeed = -1f;
            }
        }

        protected override void SpeedFallDown() {
            if (LevelSystem.jumpHeight < 5) return;
            base.SpeedFallDown();
        }

        protected override void Jump() {
            if (!ASCheck("待機", "移動", "浮空中", "前踏步", "後踏步") || secendJumped) return;
            if (!isGround) {
                if (LevelSystem.jumpHeight < 10) return;
                secendJumped = true;    // 否則如果不在地上，就啟動二段跳
            }
            verticalSpeed = unit.JumpPower; // 如果在地上，就跳。
        }

        #endregion
        
        protected override Vector2 CalculateSpeed() {
            if (ASCheck("空中攻擊1", "空中攻擊2", "空中攻擊3", "閃電踢", "飛踢")) {
                applyGrivity = false;
                return frameSpeed;
            }
            Vector2 v = new Vector2();
            applyGrivity = true;
            if (ASCheck("前踏步", "後踏步", "滑踢", "踢完過渡")) {
                v = frameSpeed;
            } else if (axisX != 0 && ASCheck("移動", "浮空中")) {
                v.x = axisX * unit.Mobility;
            }

            // Y計算
            v.y = verticalSpeed;

            return v;
        }
        

        protected override Vector3 CalculateRotation() {
            float eulerY = transform.eulerAngles.y;
            float eulerZ = 0f;

            if (ASCheck("後踏步")) {
                //eulerY = faceOnSkill;
            }
            else if (ASCheck("移動", "待機", "浮空中") && axisX != 0) {
                eulerY = (axisX > 0) ? 0 : 180f;
            }
            if (ASCheck("閃電踢")) {
                eulerZ = -45f;
            }

            return new Vector3(0f, eulerY, eulerZ);
        }

        private void ResetTriggers() {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
        }

        protected override void TimerProcess() {
            if (!ASCheck("攻擊1", "攻擊2", "攻擊3", "空中攻擊1", "空中攻擊2", "空中攻擊3")) nomralAttackTimer -= Time.deltaTime * unit.ASPD;
        }

        protected override void GroundCheck() {
            // 於角色的腳底建立一塊檢查區域，如果有碰撞器在內，則將角色視為接地
            isGround = Physics2D.OverlapArea(transform.position - (Vector3)groundCheckOffect, transform.position + (Vector3)groundCheckOffect, LayerMask.GetMask("Floor"));
            animator.SetBool("isJumping", !isGround);

            if (!isGround) animator.SetFloat("VerticalSpeed", Mathf.Clamp(verticalSpeed * 0.02f, -1f, 1f));
            else {
                if (ASCheck("空中攻擊1", "空中攻擊2", "空中攻擊3")) ResetTriggers();
                if (ASCheck("閃電踢")) {
                    animator.SetBool("InazumaKick", false);
                    applyGrivity = true;
                    StartCoroutine(KickTransiti());
                }
                secendJumped = false;
            }
        }

        protected virtual IEnumerator KickTransiti() {

            while (!ASCheck("踢完過渡")) {
                yield return null;
            }
            while (ASCheck("踢完過渡")) {
                frameSpeed = transform.right * unit.FlyKickSpeed * (1 - ASI.normalizedTime) * 2f;
                yield return null;
            }
        }
    }
}