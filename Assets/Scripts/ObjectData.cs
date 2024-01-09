using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectData", menuName = "Object Data", order = 51)]
public class ObjectData : ScriptableObject
{
    public List<GameObject> items = new List<GameObject>();

    public bool Combine(GameItems objectA, GameItems objectB)
    {
        if (GameItems.Lezvie == objectA && objectB == GameItems.Stick)
        {
            return true;
        }else if (GameItems.Stick == objectA && objectB == GameItems.Lezvie)
        {
            return true;
        }
        else if ((GameItems.Lezvie != objectA && objectB != GameItems.Stick) && objectA == objectB)
        {
            return true;
        }else if((GameItems.Stick != objectA && objectB != GameItems.Lezvie) && objectA == objectB)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Collect(Dictionary<int, GameObject> prefabDictionary) 
    {
        prefabDictionary.Add((int)GameItems.Stick, items[0]);
        prefabDictionary.Add((int)GameItems.Lezvie, items[1]);
        prefabDictionary.Add((int)GameItems.StonePickaxe, items[2]);
        prefabDictionary.Add((int)GameItems.GoldPickaxe, items[3]);
    }
}