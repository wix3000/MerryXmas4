using UnityEngine;
using System.Collections;
using System;
using Fungus;
using DG.Tweening;

public class Scene_WorldMap : MonoBehaviour {

    public WorldStop activingStop;
    public RPG_ChatracterController player;
    public Texture[] actorTextures;
    public Flowchart flowchart;

    [SerializeField]
    RectTransform battleMesh;

    bool canCtrl = false;

    // 起始方法
    void Start() {
        Initializion();
        MovePlayerToLastStop();
        SetPlayerTexture();
        StartCoroutine(DoEvent());
    }

    // 初始化方法
    void Initializion() {   
        if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPG_ChatracterController>();
        if (!flowchart) flowchart = FindObjectOfType<Flowchart>();
    }

    // 將玩家移到上一個場景對應的地圖點上
    void MovePlayerToLastStop() {
        WorldStop[] worldStops = FindObjectsOfType<WorldStop>();
        for (int i = 0; i < worldStops.Length; i++) {
            if (worldStops[i].bondingScene == Game.lastScene) {
                player.transform.position = worldStops[i].transform.position;
                activingStop = worldStops[i];
                return;
            }
        }
        activingStop = GameObject.Find("WSHome").GetComponent<WorldStop>();
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
    IEnumerator DoEvent() {
        // 先等0.5秒等該跑的跑完
        yield return 0.5f;

        // 呼叫公用事件
        flowchart.SendFungusMessage("GlobalEvent");
        while (flowchart.selectedBlock.IsExecuting()) {
            yield return null;
        }
        print("C");
        // 呼叫地圖事件
        flowchart.SendFungusMessage("b_" + activingStop.bondingScene);
        while (flowchart.selectedBlock.IsExecuting()) {
            yield return null;
        }

        Game_Item clampItem = Game.FindItem("攜帶式野營道具");
        if (clampItem != null && clampItem.count > 0) {
            flowchart.SendFungusMessage("Clamp");
        }
        else {
            canCtrl = true;
        }
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
        if (!stop) return;
        canCtrl = false;
        StartCoroutine(player.WalkTo(stop.transform.position, null, false, true, () => 
        { //Do something.
            player.FaceByDir(3);
            activingStop = stop;
            LoadMapScene();
        })
        );
    }

    void LoadMapScene() {
        battleMesh.gameObject.SetActive(true);
        battleMesh.DOSizeDelta(new Vector2(3000f, battleMesh.sizeDelta.y), 1.0f).OnComplete(() =>
        {
            Game.lastScene = Application.loadedLevelName;
            print(Game.lastScene);
            Application.LoadLevel(activingStop.bondingScene);
        });
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