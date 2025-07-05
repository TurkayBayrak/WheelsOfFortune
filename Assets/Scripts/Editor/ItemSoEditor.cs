using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item_SO))]
public class ItemSoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var item_SO = target as Item_SO;

        if (!item_SO.itemSprite) return;

        var _texture2D = AssetPreview.GetAssetPreview(item_SO.itemSprite);

        GUILayout.Space(50);

        GUILayout.Label("", GUILayout.Height(item_SO.itemSprite.rect.height * .4f), GUILayout.Width(item_SO.itemSprite.rect.width * .4f));

        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), _texture2D);
    }
}
