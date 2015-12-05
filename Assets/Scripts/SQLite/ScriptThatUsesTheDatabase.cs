using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScriptThatUsesTheDatabase : MonoBehaviour {
    public string DatabaseName = "TestDB.sqdb";

    // This is the name of the table we want to use
    public string TableName = "TestTable";
    private DBAccess db;

    void Start() {
        // Give ourselves a dbAccess object to work with, and open it
        db = new DBAccess();
        db.OpenDB(DatabaseName);
        // Let's make sure we've got a table to work with as well!
        string tableName = TableName;
        string[] columnNames = new string[] { "firstName", "lastName" };
        string[] columnValues = new string[] { "text", "text" };
        try {
            db.CreateTable(tableName, columnNames, columnValues);
        }
        catch (Exception e) {
            // Do nothing - our table was already created
            //- we don't care about the error, we just don't want to see it
            Debug.LogError(e.ToString());
        }
    }

    // These variables just hold info to display in our GUI
    private string firstName = "First Name";
    private string lastName = "Last Name";
    private int DatabaseEntryStringWidth = 100;
    private Vector2 scrollPosition;
    private List<List<object>> databaseData = new List<List<object>>();

    // This GUI provides us with a way to enter data into our database
    //  as well as a way to view it
    void OnGUI() {
        GUI.Box(new Rect(25, 25, Screen.width - 50, Screen.height - 50), "");
        GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height - 100));
        // This first block allows us to enter new entries into our table
        GUILayout.BeginHorizontal();
        firstName = GUILayout.TextField(firstName, GUILayout.Width(DatabaseEntryStringWidth));
        lastName = GUILayout.TextField(lastName, GUILayout.Width(DatabaseEntryStringWidth));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Add to database")) {
            // Insert the data
            InsertRow(firstName, lastName);
            // And update the readout of the database
            databaseData = ReadFullTable();
        }
        // This second block gives us a button that will display/refresh the contents of our database
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Read Database")) {
            databaseData = ReadFullTable();
        }
        if (GUILayout.Button("Clear")) {
            databaseData.Clear();
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Database Contents");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(100));

        foreach (List<object> line in databaseData) {
            GUILayout.BeginHorizontal();
            foreach (object s in line) {
                GUILayout.Label(s.ToString(), GUILayout.Width(DatabaseEntryStringWidth));
            }
            GUILayout.EndHorizontal();
        }


        GUILayout.EndScrollView();
        if (GUILayout.Button("Delete All Data")) {
            DeleteTableContents();
            databaseData = ReadFullTable();
        }

        GUILayout.EndArea();
    }

    // Wrapper function for inserting our specific entries into our specific database and table for this file
    private void InsertRow(string firstName, string lastName) {
        string[] values = new string[] { ("'" + firstName + "'"), ("'" + lastName + "'") };
        db.InsertInto(TableName, values);
    }

    // Wrapper function, so we only mess with our table.
    private List<List<object>> ReadFullTable() {
        return db.ReadFullTable(TableName);
    }

    // Another wrapper function...
    private void DeleteTableContents() {
        db.DeleteTableContents(TableName);
    }
}