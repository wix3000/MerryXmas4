using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace SSG {

    public class SSGEnemyAI_PosioinRubby : SSGEnemyAI_Controller_Sample {

        public Range lingeredTime;
        public Range intervalTime;
        public Range followTime;
        public float endLingeredChance;

        float axis;
        float stateTime;

        protected override void Start() {
            base.Start();
            axis = Random.Range(-1, 2);
            stateTime = Time.time + lingeredTime.Random;
        }

        protected override void MovementControl() {
            enemyModel.axisX = axis;
            if (Time.time < stateTime) return;
            axis = 0f;
            enemyModel.axisX = 0f;
            stateTime = Time.time + intervalTime.Random;
            state = AIState.readyToAttack;
        }

        protected override void Preattack() {
            if (Time.time < stateTime) return;
            if (Random.value >= endLingeredChance) {
                stateTime = Time.time + lingeredTime.Random;
                axis = Random.Range(-1, 2);
                state = AIState.move;
            }
            else {
                stateTime = Time.time + followTime.Random;
                state = AIState.attack;
            }
        }

        protected override void AttackControl() {
            base.MovementControl();
            if (Time.time < stateTime) return;
            enemyModel.axisX = 0f;
            stateTime = Time.time + intervalTime.Random;
            state = AIState.readyToAttack;
        }
    }
}