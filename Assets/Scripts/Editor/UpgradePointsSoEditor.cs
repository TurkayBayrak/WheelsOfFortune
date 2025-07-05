using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradePoints_SO))]
public class UpgradePointsSoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var upgradePoints_SO = target as UpgradePoints_SO;

        if (!upgradePoints_SO.itemSprite) return;

        var _texture2D = AssetPreview.GetAssetPreview(upgradePoints_SO.itemSprite);

        GUILayout.Space(50);

        GUILayout.Label("", GUILayout.Height(upgradePoints_SO.itemSprite.rect.height * .4f), GUILayout.Width(upgradePoints_SO.itemSprite.rect.width * .4f));

        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), _texture2D);
    }
}
