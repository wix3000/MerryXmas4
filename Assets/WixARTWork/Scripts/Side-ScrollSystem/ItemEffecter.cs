using UnityEngine;
using System.Collections;

namespace SSG {
    public class ItemEffecter : MonoBehaviour {

        static SSGPlayer_Model player;

        [SerializeField]
        SSGPlayer_Model model;

        public static SSGPlayer_Model Player {
            get {
                if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<SSGPlayer_Model>();
                return player;
            }
        }

        // Use this for initialization
        void Start() {
            if (!model) model = GetComponent<SSGPlayer_Model>();
        }

        public static void UseItem(Game_Item item) {
            switch (item.feature) {
                case "HP%":
                    print(item.parameter1);
                    new BuffEffect("Health", 5f).SetEffect(buff =>
                    {
                        player.unit.GivenDamage(player.unit.MaxHp * item.parameter1 * 0.2f * -0.01f * Time.deltaTime);
                    }).SetParticleEffect("HealthPE", "Waist")
                    .Addin(Player);
                    break;
            }
        }
    }
}