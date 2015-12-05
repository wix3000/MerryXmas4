using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITest : MonoBehaviour {

    public Game_Player player;
    public Camera uiCamera;

    public Slider slider;
    public Text lvText;
    public Text hpText;

    public Image[] hpImages = new Image[3];
    public Sprite[] chNumber = new Sprite[10];

	// Use this for initialization
	void Start () {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Game_Player>();
        LevelSystem.OnLevelUp += LevelUp;
        ChangeMaxSp(player.MaxSP);
    }
	
	// Update is called once per frame
	void Update () {
        DisplayHp();
        slider.value = player.stamina;


        if (Input.GetKeyDown(KeyCode.E)) {
            for (int i = 0; i < Random.Range(1, 6); i++) {
                Instantiate(Resources.Load("ExpItem"), player.transform.position + new Vector3(0f, 13f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            }
            for (int i = 0; i < Random.Range(3, 6); i++) {
                Instantiate(Resources.Load("ExpItemSmall"), player.transform.position + new Vector3(0f, 13f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            }
            for (int i = 0; i < Random.Range(0, 2); i++) {
                Instantiate(Resources.Load("UtralExpItem"), player.transform.position + new Vector3(0f, 13f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            GameObject item = Instantiate(Resources.Load("ItemItem"), player.transform.position + new Vector3(0f, 13f), Quaternion.identity) as GameObject;
            item.GetComponent<SSG.ItemTrigger>().SetItem(new Game_Item(Random.Range(1,9)));
        }
    }

    void DisplayHp() {
        int hp = (int)player.health;
        if (hp < 0) hp *= -1;

        for(int i = 0; i < hpImages.Length; i++) {
            if (hp <= 0 && i > 0) {
                hpImages[i].gameObject.SetActive(false);
                continue;
            }
            hpImages[i].gameObject.SetActive(true);
            hpImages[i].sprite = chNumber[hp % 10];
            hp /= 10;
        }
    }

    public void ChangeMaxSp(int maxSp) {
        slider.maxValue = maxSp;
    }

    public void LevelUp() {
        lvText.text = "Lv. " + LevelSystem.playerLevel.ToString("00");
    }
}
