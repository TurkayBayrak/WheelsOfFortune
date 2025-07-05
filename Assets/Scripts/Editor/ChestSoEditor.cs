using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Chest_SO))]
public class ChestSoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var chestSO = target as Chest_SO;

        if (!chestSO.itemSprite) return;

        var _texture2D = AssetPreview.GetAssetPreview(chestSO.itemSprite);

        GUILayout.Space(50);

        GUILayout.Label("", GUILayout.Height(chestSO.itemSprite.rect.height * .4f), GUILayout.Width(chestSO.itemSprite.rect.width * .4f));

        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), _texture2D);
    }
}
