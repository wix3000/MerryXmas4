using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game_Enemy : Game_Unit {

    [SerializeField]
    Range[] experience = new Range[3];
    public static GameObject[] expPrefabs;

    [SerializeField]
    DropItem[] dropItems = { };

    /// <summary>
    /// 獲取掉落經驗值數量的方法，會回傳掉落數量的陣列。
    /// </summary>
    /// <returns></returns>
    public int[] GetExps() {
        if (expPrefabs == null) SetExpPrefabs();
        int[] exp = new int[experience.Length];
        for (int i = 0; i < experience.Length; i++) {
            exp[i] = Random.Range((int)experience[i].min, (int)experience[i].max + 1);
        }
        return exp;
    }

    /// <summary>
    /// 獲取掉落道具的方法，會回傳實際掉落道具的清單。
    /// </summary>
    /// <returns></returns>
    public Game_Item[] GetDropItems() {
        List<Game_Item> list = new List<Game_Item>();
        for (int i = 0; i < dropItems.Length; i++) {
            if (Random.Range(0f, 1f) <= dropItems[i].chance) list.Add(new Game_Item(dropItems[i].itemIndex));
        }
        return list.ToArray();
    }

    void SetExpPrefabs() {
         expPrefabs = new GameObject[] {
            Resources.Load<GameObject>("ExpItemSmall"),
            Resources.Load<GameObject>("ExpItem"),
            Resources.Load<GameObject>("UtralExpItem")};
    }

    [System.Serializable]
    class DropItem {
        public int itemIndex;
        public float chance;

        public DropItem(int itemIndex, float chance) {
            this.itemIndex = itemIndex;
            this.chance = chance;
        }
    }
}

[System.Serializable]
public struct Range {
    public float min;
    public float max;

    public float Random {
        get {
            return (min > max) ? UnityEngine.Random.Range(max, min) : UnityEngine.Random.Range(min, max);
        }
    }

    public Range(float min, float max) {
            this.min = min;
            this.max = max;
    }

    public static Range zero {
        get { return new Range(0f, 0f); }
    }

    public static bool operator ^(Range range, float value) {
        if (range.min > range.max) {
            return (value >= range.max && value <= range.min);
        }
        return (value >= range.min && value <= range.max);
    }

    public bool Including(float value) {
        if (min > max) {
            return (value >= max && value <= min);
        }
        return (value >= min && value <= max);
    }

    /// <summary>
    /// 判斷一個值跟範圍的相對關係，若在範圍內則回傳0，否則回傳與大小值的差。
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float Determine(float value) {
        float min, max;
        if(this.min > this.max) {
            min = this.max;
            max = this.min;
        }
        else {
            min = this.min;
            max = this.max;
        }

        if(value < min) {
            return value - min;
        } else if (value > max) {
            return value - max;
        }
        else {
            return 0f;
        }
    }

    /// <summary>
    /// 範圍的長度
    /// </summary>
    public float Length {
        get { return Mathf.Abs(max - min); }
    }
    
}
