using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Chest_SO))]
public class ChestSoEditor : Editor
{
    Chest_SO chestSO;

    private void OnEnable()
    {
        chestSO = (Chest_SO)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        if (!chestSO.itemSprite) return;

        var _texture2D = AssetPreview.GetAssetPreview(chestSO.itemSprite);

        GUILayout.Space(50);

        GUILayout.Label("", GUILayout.Height(chestSO.itemSprite.rect.height * .4f), GUILayout.Width(chestSO.itemSprite.rect.width * .4f));

        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), _texture2D);


        EditorGUILayout.LabelField("Probability Value: ", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        DrawSlider("Currecy Item", ref chestSO.currencyValue, ref chestSO.specialValue, ref chestSO.upgradeValue);
        DrawSlider("Special Item", ref chestSO.specialValue, ref chestSO.currencyValue, ref chestSO.upgradeValue);
        DrawSlider("Upgrade Item", ref chestSO.upgradeValue, ref chestSO.currencyValue, ref chestSO.specialValue);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Total: " + (chestSO.currencyValue + chestSO.specialValue + chestSO.upgradeValue));
    }

    void DrawSlider(string label, ref int main, ref int other1, ref int other2)
    {
        EditorGUI.BeginChangeCheck();
        int oldValue = main;
        int newValue = EditorGUILayout.IntSlider(label, oldValue, 0, Chest_SO.TOTAL);

        if (EditorGUI.EndChangeCheck())
        {
            int remaining = Chest_SO.TOTAL - newValue;
            int totalOther = other1 + other2;

            int newOther1 = totalOther > 0 ? Mathf.RoundToInt((float)other1 / totalOther * remaining) : remaining / 2;
            int newOther2 = remaining - newOther1;

            Undo.RecordObject(chestSO, "Slider Change");

            main = newValue;
            other1 = newOther1;
            other2 = newOther2;

            EditorUtility.SetDirty(chestSO);
        }
    }
}
