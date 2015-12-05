using UnityEngine;
using System.Collections;
using System;

public class Scene_WorldMap : MonoBehaviour {

    public WorldStop activingStop;
    public RPG_ChatracterController player;
    public Texture[] actorTextures;

    bool canCtrl = true;

    // 起始方法
    void Start() {
        Initializion();
        MovePlayerToLastStop();
        SetPlayerTexture();
        DoStopEvent();
    }

    // 初始化方法
    void Initializion() {   
        if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPG_ChatracterController>();
    }

    // 將玩家移到上一個場景對應的地圖點上
    void MovePlayerToLastStop() {
        WorldStop[] worldStops = FindObjectsOfType<WorldStop>();
        for (int i = 0; i < worldStops.Length; i++) {
            if (worldStops[i].sceneName == Game.lastScene) {
                player.transform.position = worldStops[i].transform.position;
                activingStop = worldStops[i];
                return;
            }
        }
    }

    // 設置玩家的走路圖
    void SetPlayerTexture() {
        try {
            player.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = actorTextures[(int)Game.globalVariable["Partner"]];
        }
        catch (Exception) {
            throw;
        }
    }

    // 執行地圖點事件
    void DoStopEvent() {
        // TEMP
    }

    // 更新方法
    void Update() {
        InputProcess();
    }

    // 輸入處理
    void InputProcess() {
        if (!canCtrl) return;

        switch (GetInputedKey()) {
            case 0:
                return;
            case 8:
                DoPlayerMove(activingStop.up);
                break;
            case 2:
                DoPlayerMove(activingStop.down);
                break;
            case 4:
                DoPlayerMove(activingStop.left);
                break;
            case 6:
                DoPlayerMove(activingStop.right);
                break;
        }
    }

    // 執行玩者地圖點間的移動
    void DoPlayerMove(WorldStop stop) {
        StartCoroutine(player.WalkTo(stop.transform.position, null, false, true, () => 
        { //Do something.
        })
        );
    }

    // 取得輸入的按鍵
    int GetInputedKey() {
        if (Input.GetKeyDown(GameOption.Up)) return 8;
        if (Input.GetKeyDown(GameOption.Down)) return 2;
        if (Input.GetKeyDown(GameOption.Left)) return 4;
        if (Input.GetKeyDown(GameOption.Right)) return 6;
        return 0;
    }
}