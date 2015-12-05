using UnityEngine;
using System.Collections;

namespace SSG {
    public class ItemTrigger : MonoBehaviour {

        [Tooltip("要給予的經驗值")]
        public int exp;
        [Tooltip("道具的生命時間")]
        public float life;
        float timer;

        public bool isRPGItem;
        [SerializeField]
        new Rigidbody2D rigidbody;
        [SerializeField]
        SpriteRenderer sprite;
        [SerializeField]
        GameObject triggerEffect;
        Game_Item giveItem;

        void Awake() {
            if (exp == 0) {
                gameObject.SetActive(false);
            }
        }

        // Use this for initialization
        void Start() {
            if (!isRPGItem) {
                rigidbody.velocity = new Vector2(Random.Range(-20f, 20f), Random.Range(20f, 50f));
            }
        }

        // Update is called once per frame
        void Update() {
            if (timer >= life * 0.5f) {
                sprite.color = new Color(1f, 1f, 1f, 2f - (2f * timer) / life);
            }
            if (timer >= life) {
                Destroy(gameObject);
            }

            timer += Time.deltaTime;

        }

        void OnTriggerEnter2D(Collider2D hit) {
            if (hit.transform.root.CompareTag("Player") && !hit.CompareTag("AttackCollider")) {
                TakeVulue(hit.transform);
            }
        }

        void TakeVulue(Transform hit) {
            bool isSuccess;
            if (giveItem != null) {
                Game.AddItemWithCheck(giveItem, out isSuccess);
                if (!isSuccess) {
                    AddFailure(hit);
                    return;
                }
            } else {
                LevelSystem.AddExp(exp);
            }
            EndProcess();
        }

        void AddFailure(Transform hit) {
            Vector2 v = new Vector2((hit.position.x > transform.position.x) ? -20f : 20f, 30f);
            rigidbody.velocity = v;
        }

        void EndProcess() {
            GameObject PS = null;
            if (triggerEffect) PS = Instantiate(triggerEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(PS, 3f);
            Destroy(gameObject);
        }

        public void SetItem(int number) {
            giveItem = new Game_Item(number);
            if (giveItem == null) return;
            SetSprite();
            gameObject.SetActive(true);
        }

        public void SetItem(Game_Item item) {
            giveItem = item;
            if (giveItem == null) return;
            SetSprite();
            gameObject.SetActive(true);
        }

        void SetSprite() {
            sprite.sprite = giveItem.icon;
        }
    }
}