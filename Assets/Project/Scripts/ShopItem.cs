using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public KogeiData data;

    public string GetName()
    {
        if (data != null)
        {
            return data.workName;
        }
        return gameObject.name; 
    }
    public int GetRandomPrice()
    {
        if (data != null)
        {
            return Random.Range(data.minPrice, data.maxPrice + 1);
        }
        return 0; 
    }
}