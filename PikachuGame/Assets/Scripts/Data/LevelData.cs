using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Pikachu/LevelData")]
public class LevelData : ScriptableObject
{
    public int Rows;
    public int Cols;
    public int TypeCount; // số loại hình ảnh
    public int[] PredefinedTiles; // có thể để -1 cho ô rỗng, hoặc ID hình
    public float timer;
}
