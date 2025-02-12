using UnityEngine;

public class Configuration : MonoBehaviour
{
    public static int mapSize = 5;
    public static int totalRooms = mapSize * 2;
    public static int currentLevel = 0;
    public static float factorIncremento = 0.05f;
    public static Vector2 startPosition = new Vector2(0, 0);
    //public static Vector3 cameraDistance = new Vector3(0, 8.23f, -9.38f);
    public static Vector3 cameraDistance = new Vector3(0, 10f, -13.5f);
    
}
