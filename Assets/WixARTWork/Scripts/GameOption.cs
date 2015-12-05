using UnityEngine;
using System.Collections;

// 匯整遊戲設定的靜態類
public static class GameOption {
    // 暫時先給初始值，之後再在第一次進入遊戲時賦值。

    #region Input
    public static string Horizontal;
    public static string Vertical;

    public static KeyCode Confirm;
    public static KeyCode Cancel;
    public static KeyCode Up;
    public static KeyCode Down;
    public static KeyCode Left;
    public static KeyCode Right;


    static GameOption() {
        Horizontal = "Horizontal";
        Vertical = "Vertical";
        Confirm = KeyCode.Z;
        Cancel = KeyCode.X;
        Up = KeyCode.UpArrow;
        Down = KeyCode.DownArrow;
        Left = KeyCode.LeftArrow;
        Right = KeyCode.RightArrow;
        HideCursor = false;
    }


    public static void SetInputType(int type) {
        switch (type) {
            case 0:
                Horizontal = "Horizontal";
                Vertical = "Vertical";
                Confirm = KeyCode.Z;
                Cancel = KeyCode.X;
                Up = KeyCode.UpArrow;
                Down = KeyCode.DownArrow;
                Left = KeyCode.LeftArrow;
                Right = KeyCode.RightArrow;
                break;
            case 1:
                Horizontal = "HorizontalLeft";
                Vertical = "VerticalLeft";
                Confirm = KeyCode.K;
                Cancel = KeyCode.L;
                Up = KeyCode.W;
                Down = KeyCode.S;
                Left = KeyCode.A;
                Right = KeyCode.D;
                break;
        }
    }

    //是否隱藏滑鼠指標
    public static bool HideCursor {
        get {
            return !Cursor.visible;
        }
        set {
            Cursor.visible = !value;
        }
    }
    #endregion
    #region Audio

    // 設置總音量
    public static void SetMainVolume(float volume) {
        AudioListener.volume = Mathf.Clamp01(volume);
    }
    #endregion
    #region Screen
        
    #endregion
}