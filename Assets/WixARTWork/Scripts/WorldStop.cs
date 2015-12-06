using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WorldStop : MonoBehaviour {

    [SerializeField]
    Texture gizmosTexture;

    public WorldStop up, down, left, right;
    [Space(15f)]
    public WorldStop spUp;
    public WorldStop spDown;
    public WorldStop spLeft;
    public WorldStop spRight;

    public int battleHard;
    public string bondingScene;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow)) Move(up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Move(down);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Move(right);
    }

    void Move(WorldStop target) {
        /*
        if (!target) return;
        Scene_WorldMap.actor.WalkTo(target.transform.position, 0f, false, true, () => OnEnter(target));
        enabled = false;
        */
    }

    /*
    void OnEnter(WorldStop target) {
        if (target.battleHard == 0) {
            //Application.LoadLevelAdditive(target.sceneName);
            print("進入場景!");
        }
        else {
            print("進入戰鬥!");
        }
        target.enabled = true;
    }
    /*
    void OnDrawGizmos() {
        //Gizmos.DrawGUITexture(new Rect(-16, -16, 32, 32), gizmosTexture);
        DrawString(sceneName, transform.position, Color.black);
    }


    static void DrawString(string text, Vector3 worldPos, Color? colour = null) {
        UnityEditor.Handles.BeginGUI();
        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height - size.y * 1.5f , size.x, size.y), text);
        UnityEditor.Handles.EndGUI();
    }
    */
}
