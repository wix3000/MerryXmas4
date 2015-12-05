using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("Variable",
                 "讀取公用變數",
                 "從全域的空間中讀取一筆資料為Fungus變數。")]
    [AddComponentMenu("")]
    public class LoadGlobalVariable : Command {
        [Tooltip("變數的儲存欄位名稱")]
        public string 鍵值 = "";

        [Tooltip("Variable to read the value from. Only Boolean, Integer, Float and String are supported.")]
        [VariableProperty(typeof(BooleanVariable),
                          typeof(IntegerVariable),
                          typeof(FloatVariable),
                          typeof(StringVariable),
                          typeof(ColorVariable),
                          typeof(GameObjectVariable),
                          typeof(MaterialVariable),
                          typeof(ObjectVariable),
                          typeof(SpriteVariable),
                          typeof(TextureVariable),
                          typeof(Vector2Variable),
                          typeof(Vector3Variable))]
        public Variable 數值;

        public override void OnEnter() {
            if (鍵值 == "" ||
                數值 == null ||
                !Game.globalVariable.ContainsKey(鍵值)) {
                Continue();
                return;
            }

            Flowchart flowchart = GetFlowchart();

            System.Type variableType = 數值.GetType();
            object value = Game.globalVariable[鍵值];

            #region 很長 縮一下
            if (variableType == typeof(BooleanVariable)) {
                BooleanVariable booleanVariable = 數值 as BooleanVariable;
                if (booleanVariable != null) {
                    booleanVariable.value = (bool)value;
                }
            }
            else if (variableType == typeof(IntegerVariable)) {
                IntegerVariable integerVariable = 數值 as IntegerVariable;
                if (integerVariable != null) {
                    integerVariable.value = (int)value;
                }
            }
            else if (variableType == typeof(FloatVariable)) {
                FloatVariable floatVariable = 數值 as FloatVariable;
                if (floatVariable != null) {
                    floatVariable.value = (float)value;
                }
            }
            else if (variableType == typeof(StringVariable)) {
                StringVariable stringVariable = 數值 as StringVariable;
                if (stringVariable != null) {
                    stringVariable.value = (string)value;
                }
            }
            else if (variableType == typeof(ColorVariable)) {
                ColorVariable colorVariable = 數值 as ColorVariable;
                if (colorVariable != null) {
                    colorVariable.value = (Color)value;
                }
            }
            else if (variableType == typeof(GameObjectVariable)) {
                GameObjectVariable gameObjectVariable = 數值 as GameObjectVariable;
                if (gameObjectVariable != null) {
                    gameObjectVariable.value = value as GameObject;
                }
            }
            else if (variableType == typeof(MaterialVariable)) {
                MaterialVariable materialVariable = 數值 as MaterialVariable;
                if (materialVariable != null) {
                    materialVariable.value = value as Material;
                }
            }
            else if (variableType == typeof(ObjectVariable)) {
                ObjectVariable objectVariable = 數值 as ObjectVariable;
                if (objectVariable != null) {
                    objectVariable.value = value as Object;
                }
            }
            else if (variableType == typeof(SpriteVariable)) {
                SpriteVariable spriteVariable = 數值 as SpriteVariable;
                if (spriteVariable != null) {
                    spriteVariable.value = value as Sprite;
                }
            }
            else if (variableType == typeof(TextureVariable)) {
                TextureVariable textureVariable = 數值 as TextureVariable;
                if (textureVariable != null) {
                    textureVariable.value = value as Texture;
                }
            }
            else if (variableType == typeof(Vector2Variable)) {
                Vector2Variable vector2Variable = 數值 as Vector2Variable;
                if (vector2Variable != null) {
                    vector2Variable.value = (Vector2)value;
                }
            }
            else if (variableType == typeof(Vector3Variable)) {
                Vector3Variable vector3Variable = 數值 as Vector3Variable;
                if (vector3Variable != null) {
                    vector3Variable.value = (Vector3)value;
                }
            }
            #endregion

            Continue();
        }

        public override string GetSummary() {
            if (鍵值.Length == 0) {
                return "錯誤: 未輸入鍵值";
            }

            if (數值 == null) {
                return "錯誤: 未選擇變數";
            }

            return string.Format("讀取'{0}'並存至{1}", 鍵值, 數值.key);
        }

        public override Color GetButtonColor() {
            return new Color32(235, 191, 217, 255);
        }
    }
}