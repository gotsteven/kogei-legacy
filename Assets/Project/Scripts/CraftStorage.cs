using UnityEngine;
using System.Collections.Generic;

public static class CraftStorage
{
    public static List<GameObject> works = new List<GameObject>();

    public static KogeiData TempResultData; 

    public static void Register(GameObject craft)
    {
        if (craft != null)
        {
            works.Add(craft);
            Object.DontDestroyOnLoad(craft);
        }
    }

    public static GameObject FindByID(string id)
    {
        foreach (var craft in works)
        {
            if (craft == null) continue;

            var item = craft.GetComponent<KogeiItem>();
            if (item != null && item.uniqueID == id)
            {
                return craft;
            }
        }
        return null;
    }
}