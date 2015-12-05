using System.Data;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;// we import sqlite

class DBAccess {
    public const string DB_NAME = "SQLdatabase.sqdb";
    // variables for basic query access
    private string connection;
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    private IDataReader reader;

    /// <summary>
    /// 打開名為p的資料庫檔案。
    /// </summary>
    /// <param name="p"></param>
    public void OpenDB(string p) {
        connection = @"Data Source=" + Application.dataPath + "/" + p; // we set the connection to our database
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
    }

    public IDataReader BasicQuery(string q, bool r) {
        // run a baic Sqlite query
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = q; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        if (r) { // if we want to return the reader
            return reader; // return the reader
        }

        return null;
    }

    /// <summary>
    /// 讀取整張表單的方法。
    /// </summary>
    /// <param name="tableName">表單名稱</param>
    /// <returns></returns>
    public List<List<object>> ReadFullTable(string tableName) {
        string query;
        query = "SELECT * FROM " + tableName;
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();
        List<List<object>> readArray = new List<List<object>>();
        while (reader.Read()) {
            List<object> lineArray = new List<object>();
            for (int i = 0; i < reader.FieldCount; i++) {
                lineArray.Add(reader.GetValue(i));
            } // This reads the entries in a row
            readArray.Add(lineArray); // This makes an array of all the rows
        }
        return readArray; // return matches
    }

    /// <summary>
    /// 刪除整個表單資料的方法。 ※務必謹慎使用※
    /// </summary>
    /// <param name="tableName">表單名稱</param>
    public void DeleteTableContents(string tableName) {
        string query;
        query = "DELETE FROM " + tableName;
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();
    }

    /// <summary>
    /// 創建新表單的方法。
    /// </summary>
    /// <param name="name">表單名稱</param>
    /// <param name="col">包含所有欄位名稱的陣列</param>
    /// <param name="colType">包含所有欄位類型的陣列</param>
    public void CreateTable(string name, string[] col, string[] colType) { // Create a table, name, column array, column type array
        string query;
        query = "CREATE TABLE " + name + "(" + col[0] + " " + colType[0];
        for (int i = 1; i < col.Length; i++) {
            query += ", " + col[i] + " " + colType[i];
        }
        query += ")";
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = query; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader

    }


    public void InsertIntoSingle(string tableName, string colName, string value) { // single insert 
        string query;
        query = "INSERT INTO " + tableName + "(" + colName + ") " + "VALUES (" + value + ")";
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = query; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader
    }

    public void InsertIntoSpecific(string tableName, string[] col, string[] values) { // Specific insert with col and values
        string query;
        query = "INSERT INTO " + tableName + "(" + col[0];
        for (int i = 1; i < col.Length; i++) {
            query += ", " + col[i];
        }
        query += ") VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++) {
            query += ", " + values[i];
        }
        query += ")";
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();
    }

    public void InsertInto(string tableName, string[] values) {
        // basic Insert with just values
        string query;
        query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++) {
            query += ", " + values[i];
        }
        query += ")";
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();
    }

    // This function reads a single column
    //  wCol is the WHERE column, wPar is the operator you want to use to compare with, 
    //  and wValue is the value you want to compare against.
    //  Ex. - SingleSelectWhere("puppies", "breed", "earType", "=", "floppy")
    //  returns an array of matches from the command: SELECT breed FROM puppies WHERE earType = floppy;
    /// <summary>
    /// 尋找單格資料的方法。
    /// </summary>
    /// <param name="tableName">來源表單名稱</param>
    /// <param name="itemToSelect">目標欄位名稱</param>
    /// <param name="wCol">比對欄位名稱</param>
    /// <param name="wPar">比對符號</param>
    /// <param name="wValue">比對值</param>
    /// <returns></returns>
    public List<object> SingleSelectWhere(string tableName, string itemToSelect, string wCol, string wPar, string wValue) {
        // Selects a single Item
        string query;
        query = "SELECT " + itemToSelect + " FROM " + tableName + " WHERE " + wCol + wPar + '"' + wValue + '"';
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();
        List<object> readArray = new List<object>();
        while (reader.Read()) {
            readArray.Add(reader.GetString(0)); // Fill array with all matches
        }
        return readArray; // return matches
    }

    /// <summary>
    /// 讀取所有符合條件的行的方法。
    /// </summary>
    /// <param name="tableName">來源表單名稱</param>
    /// <param name="wCol">比對欄位名稱</param>
    /// <param name="wPar">比對符號</param>
    /// <param name="wValue">比對值</param>
    /// <returns></returns>
    public List<List<object>> SelectFullLine(string tableName, string wCol, string wPar, string wValue) {
        string query;
        query = "SELECT * FROM " + tableName + " WHERE " + wCol + wPar + '"' + wValue + '"';
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();
        List<List<object>> readArray = new List<List<object>>();
        while (reader.Read()) {
            List<object> lineArray = new List<object>();
            for (int i = 0; i < reader.FieldCount; i++) {
                lineArray.Add(reader.GetValue(i));
            } // This reads the entries in a row
            readArray.Add(lineArray); // This makes an array of all the rows
        }
        return readArray; // return matches
    }
    

    public void CloseDB() {
        reader.Close(); // clean everything up
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
    }
}