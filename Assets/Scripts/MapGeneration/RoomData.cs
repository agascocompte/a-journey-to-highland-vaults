using UnityEngine;

public class RoomData : MonoBehaviour
{
	public int exitsNumber;
	public bool[] hasDoor;
	public Vector3[] playerSpawns;
	public Vector3 center;
	public Sprite sprite;
	public int spriteRotation;
	public Colors highLight = Colors.None;	// No implementado en el resto de scripts todavía.

    public enum Colors
    {
		None,
		Red,
		Blue,
		Green
    }
}
