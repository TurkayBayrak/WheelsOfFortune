using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpecialItem_SO))]
public class SpecialItemSoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var specialItem_SO = target as SpecialItem_SO;

        if (!specialItem_SO.itemSprite) return;

        var _texture2D = AssetPreview.GetAssetPreview(specialItem_SO.itemSprite);

        GUILayout.Space(50);

        GUILayout.Label("", GUILayout.Height(specialItem_SO.itemSprite.rect.height * .4f), GUILayout.Width(specialItem_SO.itemSprite.rect.width * .4f));

        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), _texture2D);
    }
}
