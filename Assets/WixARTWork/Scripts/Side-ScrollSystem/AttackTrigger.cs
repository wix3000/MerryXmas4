using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace SSG {
    public class AttackTrigger : MonoBehaviour {

        public float attackRate = 1f;
        public bool continuousTigger = false;
        public float interval = 0f;
        [Space(12f)]
        public float gameStay = 0.1f;
        public float cameraShakeDuration = 0.1f;
        public float cameraShakeStrength = 3f;
        public int cameraShakeVibrato = 10;
        public Game_Unit unit;
        public ResistantList attackEffect = new ResistantList();
        public GameObject praticleEffect;

        public bool WillStiff {
            get {
                if(attackEffect.stiff > 0f || attackEffect.repulse > 0f || attackEffect.floating > 0f) {
                    return true;
                }
                return false;
            }
        }

        List<ColliderTimer> colliderList = new List<ColliderTimer>();
        float stayTimer;

        void LateUpdate() {
            if (Time.unscaledTime >= stayTimer && Mathf.Abs(stayTimer) > float.Epsilon) {
                Time.timeScale = 1f;
                stayTimer = 0f;
            }
            if (!continuousTigger) return;
            List<ColliderTimer> tmepList = colliderList;
            for (int i = 0; i < tmepList.Count; i++) {
                tmepList[i].timer -= Time.deltaTime;
                if (tmepList[i].timer <= 0f) colliderList.RemoveAt(i);
            }
        }

        void OnTriggerStay2D(Collider2D hit) {
            if (!continuousTigger) return;

            if (!unit) unit = GetComponentInParent<Game_Unit>();
            if (!unit) {
                enabled = false;
                return;
            }

            if (((transform.root.tag == "Player" && hit.transform.root.tag == "Enemy") || (transform.root.tag == "Enemy" && hit.transform.root.tag == "Player")) && !hit.CompareTag("AttackCollider")) {
                for (int i = 0; i < colliderList.Count; i++) {
                    if (colliderList[i].hit == hit) return;
                }
                hit.transform.root.SendMessage("OnAttacken", this);
                colliderList.Add(new ColliderTimer(hit, interval));

                if (cameraShakeDuration > 0f) {
                    Camera.main.GetComponent<CameraGM>().Shake(cameraShakeDuration, cameraShakeStrength, cameraShakeVibrato);
                }
                if (gameStay > 0f) {
                    Time.timeScale = 0f;
                    stayTimer = Time.unscaledTime + gameStay;
                }

                if (praticleEffect) {
                    InstantiatePraticleEffect(hit.transform.position);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D hit) {
            if (continuousTigger) return;

            if (!unit) unit = GetComponentInParent<Game_Unit>();
            if (!unit) enabled = false;

            if (((transform.root.tag == "Player" && hit.transform.root.tag == "Enemy") || (transform.root.tag == "Enemy" && hit.transform.tag == "Player")) && !hit.CompareTag("AttackCollider")) {
                hit.SendMessageUpwards("OnAttacken", this);
                if (transform.root.tag == "Player") ComboPanel.KeepCombo();

                if (cameraShakeDuration > 0f) {
                    Camera.main.GetComponent<CameraGM>().Shake(cameraShakeDuration, cameraShakeStrength, cameraShakeVibrato);
                }
                if (gameStay > 0f) {
                    Time.timeScale = 0f;
                    stayTimer = Time.unscaledTime + gameStay;
                }

                if (praticleEffect) {
                    InstantiatePraticleEffect(hit.transform.position);
                }

            }
        }

        void InstantiatePraticleEffect(Vector3 target) {
            target.y = transform.position.y;
            target += new Vector3(Random.Range(-3f, 3f), Random.Range(0f, 10f));
            Instantiate(praticleEffect, target, Quaternion.identity);
        }

        class
            ColliderTimer {
            public Collider2D hit;
            public float timer;

            public ColliderTimer(Collider2D hit, float timer) {
                this.hit = hit;
                this.timer = timer;
            }
        }
    }
}