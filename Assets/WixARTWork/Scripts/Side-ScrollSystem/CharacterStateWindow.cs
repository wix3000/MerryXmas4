using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStateWindow : MonoBehaviour {

    public Game_Unit player;
    public Slider hpSlider;
    public Slider spSlider;

	// Use this for initialization
	void Start () {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Game_Unit>();
        if (!player) enabled = false;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        hpSlider.value = player.health / player.MaxHp * 25f;
        spSlider.value = player.stamina / player.MaxSp * 25f;
    }
}
