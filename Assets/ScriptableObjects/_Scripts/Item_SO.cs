using UnityEngine;

[CreateAssetMenu(fileName = "Item_SO")]
public class Item_SO : ScriptableObject
{
    public string itemName;
    public ItemTypes itemType;

    public Sprite itemSprite;
    public int itemId;
    public int defaultItemAmount;

    //public bool isBomb;


    //private void OnValidate()
    //{
    //    if (!itemSprite) return;
    //    itemName = itemSprite.name;
    //}
}
