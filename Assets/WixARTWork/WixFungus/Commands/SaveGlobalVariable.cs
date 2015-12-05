using UnityEngine;
using System.Collections;

namespace Fungus {
    [CommandInfo("Variable",
                 "儲存公用變數",
                 @"將一個Fungus變數儲存到一個全域的空間中。
必需要為儲存的資料賦予一個字串名稱，以供日後讀取。")]
    [AddComponentMenu("")]
    public class SaveGlobalVariable : Command {
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
                數值 == null) {
                Continue();
                return;
            }
            System.Type variableType = 數值.GetType();

            #region 很長 縮一下
            if (variableType == typeof(BooleanVariable)) {
                BooleanVariable booleanVariable = 數值 as BooleanVariable;
                if (booleanVariable != null) {
                    AddValue(booleanVariable.value);
                }
            }
            else if (variableType == typeof(IntegerVariable)) {
                IntegerVariable integerVariable = 數值 as IntegerVariable;
                if (integerVariable != null) {
                    AddValue(integerVariable.value);
                }
            }
            else if (variableType == typeof(FloatVariable)) {
                FloatVariable floatVariable = 數值 as FloatVariable;
                if (floatVariable != null) {
                    AddValue(floatVariable.value);
                }
            }
            else if (variableType == typeof(StringVariable)) {
                StringVariable stringVariable = 數值 as StringVariable;
                if (stringVariable != null) {
                    AddValue(stringVariable.value);
                }
            }
            else if (variableType == typeof(ColorVariable)) {
                ColorVariable colorVariable = 數值 as ColorVariable;
                if (colorVariable != null) {
                    AddValue(colorVariable.value);
                }
            }
            else if (variableType == typeof(GameObjectVariable)) {
                GameObjectVariable gameObjectVariable = 數值 as GameObjectVariable;
                if (gameObjectVariable != null) {
                    AddValue(gameObjectVariable.value);
                }
            }
            else if (variableType == typeof(MaterialVariable)) {
                MaterialVariable materialVariable = 數值 as MaterialVariable;
                if (materialVariable != null) {
                    AddValue(materialVariable.value);
                }
            }
            else if (variableType == typeof(ObjectVariable)) {
                ObjectVariable objectVariable = 數值 as ObjectVariable;
                if (objectVariable != null) {
                    AddValue(objectVariable.value);
                }
            }
            else if (variableType == typeof(SpriteVariable)) {
                SpriteVariable spriteVariable = 數值 as SpriteVariable;
                if (spriteVariable != null) {
                    AddValue(spriteVariable.value);
                }
            }
            else if (variableType == typeof(TextureVariable)) {
                TextureVariable textureVariable = 數值 as TextureVariable;
                if (textureVariable != null) {
                    AddValue(textureVariable.value);
                }
            }
            else if (variableType == typeof(Vector2Variable)) {
                Vector2Variable vector2Variable = 數值 as Vector2Variable;
                if (vector2Variable != null) {
                    AddValue(vector2Variable.value);
                }
            }
            else if (variableType == typeof(Vector3Variable)) {
                Vector3Variable vector3Variable = 數值 as Vector3Variable;
                if (vector3Variable != null) {
                    AddValue(vector3Variable.value);
                }
            }
            #endregion

            Continue();
        }

        void AddValue(object value) {
            if (Game.globalVariable.ContainsKey(鍵值)) {
                Game.globalVariable[鍵值] = value;
            }
            else {
                Game.globalVariable.Add(鍵值, value);
            }
        }

        public override string GetSummary() {
            if (鍵值.Length == 0) {
                return "錯誤: 未輸入鍵值";
            }

            if (數值 == null) {
                return "錯誤: 未選擇變數";
            }

            return string.Format("儲存{0}至'{1}'", 數值.key, 鍵值);
        }

        public override Color GetButtonColor() {
            return new Color32(235, 191, 217, 255);
        }
    }
}