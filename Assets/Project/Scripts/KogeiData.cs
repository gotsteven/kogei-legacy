using UnityEngine;

[CreateAssetMenu(fileName = "NewKogeiData", menuName = "Shop/KogeiData")]
public class KogeiData : ScriptableObject
{
    public string workName; 
    
    [Header("価格設定")]
    public int minPrice = 10;
    public int maxPrice = 200;
}