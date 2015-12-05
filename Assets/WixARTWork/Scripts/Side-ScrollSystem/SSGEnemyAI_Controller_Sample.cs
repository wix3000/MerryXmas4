using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SSG {
    public class SSGEnemyAI_Controller_Sample : MonoBehaviour, IPlayerSwitch {

        public List<SkillInputList> skillList = new List<SkillInputList>();

        [SerializeField]
        protected SSGPlayer_Model_Enemy enemyModel;

        protected bool canControl = true;
        protected float timer;
        protected float timerForAttack = 0f;
        protected bool isAttacked;


        // AI
        protected AIState state;
        protected Game_Unit player;

        [SerializeField]
        protected Range attackDistance = new Range(0, 10f);
        [SerializeField]
        protected float attackHeight = 10f;
        [SerializeField]
        protected float WaitBeforeAttack = 0f;
        [SerializeField]
        protected float WaitAfterAttack = 1f;

        public float MinAttackDis { get { return attackDistance.min; } }
        public float MaxAttackDis { get { return attackDistance.max; } }

        // Use this for initialization
        protected virtual void Start() {
            if (!enemyModel) {
                enemyModel = GetComponent<SSGPlayer_Model_Enemy>();
                if (!enemyModel) {
                    Debug.LogError("玩者控制器找不到可用的怪物模組！");       //LogError 控制台顯示訊息 加上Error會在控制台前多一個驚嘆號
                    enabled = false;
                }
            }
            SearchPlayer();
        }

        // Update is called once per frame
        protected virtual void Update() {
            if (Time.timeScale <= 0f) return;
            if (!canControl) return;
            if (!enemyModel.isGround) return;
            StateProcess();
        }

        // 判斷ai狀態
        protected virtual void StateProcess() {
            switch (state) {
                case AIState.move:
                    MovementControl();
                    break;
                case AIState.readyToAttack:
                    Preattack();
                    break;
                case AIState.attack:
                    AttackControl();
                    break;
                case AIState.readyToMove:
                    Proattack();
                    break;
            }
        }

        // 搜索玩家
        protected void SearchPlayer() {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Game_Unit>();
            if (!player) enabled = false;
        }

        // 攻擊控制
        protected virtual void AttackControl() {
            if (enemyModel.ASCheck("攻擊1")) {
                state = AIState.readyToMove;
                return;
            }
            if (Mathf.Abs(transform.position.y - player.transform.position.y) < attackHeight) {
                FacePlayer();
                enemyModel.DoBaseAction(KeyPath.LAttack);
            }
        }
        // 攻擊前控制
        protected virtual void Preattack() {
            timerForAttack += Time.deltaTime;
            if(timerForAttack >= WaitBeforeAttack) {
                timerForAttack = 0f;
                state = AIState.attack;
            }
        }

        protected virtual void Proattack() {
            if (!enemyModel.ASCheck("攻擊1")) { 
                timerForAttack += Time.deltaTime;
                if(timerForAttack > WaitAfterAttack) {
                    isAttacked = false;
                    timerForAttack = 0f;
                    state = AIState.move;
                }
            }
        }

        // 移動控制
        protected virtual void MovementControl() {
            if (attackDistance.Determine(CalculateDistance()) > 0f) {
                enemyModel.axisX = (transform.position.x < player.transform.position.x) ? 1f : -1f;
            }
            else if (attackDistance.Determine(CalculateDistance()) < 0f) {
                enemyModel.axisX = (transform.position.x < player.transform.position.x) ? -1f : 1f;
            }
            else {
                enemyModel.axisX = 0f;
                state = AIState.readyToAttack;
            }
        }

        protected virtual void FacePlayer() {
            enemyModel.axisX = 0f;
            if (Mathf.Approximately(transform.eulerAngles.y, 0f)) {
                if( player.transform.position.x < transform.position.x) {
                    enemyModel.axisX = -0.01f;
                }
            } else {
                if (player.transform.position.x > transform.position.x) {
                    enemyModel.axisX = 0.01f;
                }
            }
        }

        protected float CalculateDistance() {
            return Mathf.Abs(transform.position.x - player.transform.position.x);
        }

        protected virtual void OnAttacken(AttackTrigger AT) {
            isAttacked = false;
            state = AIState.move;
            timerForAttack = 0f;
        }

        public void SetActive(bool b) {
            canControl = b;
            enemyModel.axisX = 0f;
            enemyModel.axisY = 0f;
        }

        protected enum AIState {
            move,
            readyToAttack,
            attack,
            readyToMove
        }
    }
}