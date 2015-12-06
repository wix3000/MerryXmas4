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
        Game_Item item = FindItem(i.itemIndex);
        if(item != null) {
            item.count++;
            return;
        }
        items.Add(i);
    }

    public static void AddItem(int dbindex) {
        CallEvent();
        Game_Item item = FindItem(dbindex);
        if (item != null) {
            item.count++;
            return;
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

    /// <summary>
    /// 用名稱判斷是否擁有指定道具的方法。
    /// </summary>
    /// <param name="itemName">道具名稱</param>
    public static Game_Item FindItem(string itemName) {
        for(int i = 0; i < items.Count; i++) {
            if(items[i].itemName == itemName) {
                return items[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 用編號判斷是否擁有指定道具的方法。
    /// </summary>
    /// <param name="itemIndex">道具編號</param>
    public static Game_Item FindItem(int itemIndex) {
        for (int i = 0; i < items.Count; i++) {
            if (items[i].itemIndex == itemIndex) {
                return items[i];
            }
        }
        return null;
    }
}
