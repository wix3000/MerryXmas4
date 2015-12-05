using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Game {

    public static List<Game_Item> items = new List<Game_Item>();
    public static Sprite[] ItemIconAll = Resources.LoadAll<Sprite>("ItemAll");
    public static Hashtable globalVariable = new Hashtable();
	public static int[] SaveEquip={0,0,0,0,0,1};
    public static Action OnAddin;

    public static string lastScene = "Home2";

    public static float[] characterHp = new float[3];

    static Game() {
        globalVariable.Add("Partner", 0);
    }

    public static void AddItem(Game_Item i) {
        CallEvent();
        foreach (Game_Item item in items) {
            if (item.itemIndex == i.itemIndex) {
                item.count++;
                return;
            }
        }
        items.Add(i);
    }

    public static void AddItem(int dbindex) {
        CallEvent();
        foreach (Game_Item item in items) {
            if (item.itemIndex == dbindex) {
                item.count++;
                return;
            }
        }
        items.Add(new Game_Item(dbindex));
    }

    public static void AddItemWithCheck(Game_Item i, out bool isSuccess) {
        isSuccess = true;
        foreach (Game_Item item in items) {
            if (item.itemIndex == i.itemIndex) {
                if (item.count >= item.stack) {
                    isSuccess = false;
                    return;
                }
                item.count++;
                CallEvent();
                return;
            }
        }
        items.Add(i);
        CallEvent();
    }

    public static void AddItemwithCheck(int dbindex, out bool isSuccess) {
        isSuccess = true;
        foreach (Game_Item item in items) {
            if (item.itemIndex == dbindex) {
                if (item.count >= item.stack) {
                    isSuccess = false;
                    return;
                }
                item.count++;
                CallEvent();
                return;
            }
        }
        items.Add(new Game_Item(dbindex));
        CallEvent();
    }

    static void CallEvent() {
        if (OnAddin != null) OnAddin();
    }
}
