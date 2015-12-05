using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game_UnitData {

    // 資料庫參數
    static string dbName = DBAccess.DB_NAME;
    static string tableName = "UnitTable";
    DBAccess db;
    List<List<object>> dbData;

    int _level = 1;
    public int level {
        get { return _level; }
        private set { _level = value; }
    }

    public int maxHp { get; private set; }
    public int maxSp { get; private set; }

    public float health { get; private set; }

    public float spRate { get; private set; }

    public float attack { get; private set; }
    public float defence { get; private set; }
    public float mobility { get; private set; }
    public float jumpPower { get; private set; }

    public ResistantList resistant { get; private set; }



}
