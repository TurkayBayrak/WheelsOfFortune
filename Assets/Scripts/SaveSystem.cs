using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static void SaveInventoryData(Inventory inventory)
    {
        var formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "InventoryData";
        var stream = new FileStream(path, FileMode.Create);

        var inventoryData = new InventoryData(inventory);

        formatter.Serialize(stream, inventoryData);
        stream.Close();
    }


    public static InventoryData LoadInventoryData()
    {
        string path = Application.persistentDataPath + "InventoryData";

        if (File.Exists(path))
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Open);

            var inventoryData = formatter.Deserialize(stream) as InventoryData;

            stream.Close();

            return inventoryData;
        }
        else
        {
            Debug.Log("No Save File");
            return null;
        }
    }
}