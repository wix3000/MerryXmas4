using UnityEngine;
using System.Collections;

namespace SSG {
    public class SSGPlayer_Model_Enemy : SSGPlayer_Model {

        Game_Enemy enemy { get { return (Game_Enemy)unit; } }

        protected override void OnDeath() {
            base.OnDeath();
            int[] exps = enemy.GetExps();
            for(int i = 0; i < exps.Length; i++) {
                for(int j = 0; j < exps[i]; j++) {
                    Instantiate(Game_Enemy.expPrefabs[i], transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
                }
            }
            Game_Item[] items = enemy.GetDropItems();
            for (int i = 0; i < items.Length; i++) {
                GameObject item = Instantiate(Resources.Load("ItemItem"), transform.position, Quaternion.identity) as GameObject;
                item.GetComponent<ItemTrigger>().SetItem(items[i]);
            }
        }
    }
}