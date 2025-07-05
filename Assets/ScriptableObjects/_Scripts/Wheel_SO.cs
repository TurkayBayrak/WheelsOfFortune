using UnityEngine;

[CreateAssetMenu(fileName = "Wheel_SO")]
public class Wheel_SO : ScriptableObject
{
    public Sprite wheelSprite;
    public Sprite indicatorSprite;

    public Item_SO[] item_SOs;
}
