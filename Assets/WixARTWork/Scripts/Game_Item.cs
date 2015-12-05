using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Item {

    // 資料庫參數
    static string dbName = DBAccess.DB_NAME;
    static string tableName = "ItemTable";
    DBAccess db;
    List<List<object>> dbData;

    // 屬性
    public int itemIndex { get; private set; }
    public string iconFile { get; private set; }
    public int iconIndex { get; private set; }
    public Sprite icon { get; private set; }
    public string itemName { get; private set; }
    public string itemInfo { get; private set; }
    public ItemType itemType { get; private set; }
    public int stack { get; private set; }
    public float price { get; private set; }
    public float cost1 { get; private set; }
    public float cost2 { get; private set; }
    public string feature { get; private set; }
    public float parameter1 { get; private set; }
    public float parameter2 { get; private set; }

    public Game_Item() {
    }
     
    public int count;   // 實際數量

    
    public Game_Item(int dbIndex) {
        db = new DBAccess();
        db.OpenDB(dbName);
        dbData = db.SelectFullLine(tableName, "Numble", "=", dbIndex.ToString());

        if (dbData.Count == 0) {
            Debug.LogErrorFormat("資料庫中找不到編號為{0}的道具。", dbIndex);
            return;
        }

        List<object> sourceData = dbData[0];

        itemIndex = Convert.ToInt32(sourceData[0]);
        itemName = sourceData[1].ToString();
        iconFile = sourceData[2].ToString();
        if (sourceData[3].ToString() != "") iconIndex = Convert.ToInt32(sourceData[3]);
        if(iconFile != "" && iconIndex != 0) icon = Resources.LoadAll<Sprite>(iconFile)[iconIndex];
        itemInfo = sourceData[4].ToString();
        itemType = (ItemType)Convert.ToInt32(sourceData[5]);
        stack = Convert.ToInt32(sourceData[6]);
        price = Convert.ToInt32(sourceData[7]);
        cost1 = Convert.ToInt32(sourceData[8]);
        cost2 = Convert.ToInt32(sourceData[9]);
        feature = sourceData[10].ToString();
        if (sourceData[11].ToString() != "") parameter1 = Convert.ToSingle(sourceData[11]);
        if (sourceData[12].ToString() != "") parameter2 = Convert.ToSingle(sourceData[12]);

        db.CloseDB();

        count = 1;
    }

    public enum ItemType {
        health,
        functional,
        non_functional,
        precious
    }
}
