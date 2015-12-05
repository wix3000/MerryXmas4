using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class DataSpine
{
    [Tooltip("技能按鍵設定")]
    public List<KeyCode> keyName = new List<KeyCode>();
    [Tooltip("技能名稱"), SpineAnimation]
    public string skillName;
    [Tooltip("技能是否為連續技")]
    public bool isCombo;
}
