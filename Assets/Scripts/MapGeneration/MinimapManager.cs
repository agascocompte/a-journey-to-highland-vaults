using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
	private static bool started = false;
	private static GameObject minimapCanvas;
	private static GameObject minimapPanel;
	private static float minimapSize;
	private static Sprite playerSprite;
	private static GameObject playerMark;
	private static Sprite[][] minimapSprites;
	private static int[][] minimapSpritesRotations;
	private static float[] playerPosition;
	private static readonly float correctigFactor = 30f / 32f;	// esto provoca que los bordes se sobrepongan (y el jugador siga bien el mapa),
														// cada imagen es de 32x32 pixeles, al haces 30/32 es como si la imagen fuera de 30x30
														// como los bordes son de 2 pixeles esto provoca el efecto de que se sobreponen.

	public static void Preparation(float _minimapSize, Sprite _playerSprite)
	{
		if (started) return;
		started = true;

		minimapSize = _minimapSize;
		playerSprite = _playerSprite;

		Vector2 startPosition = Configuration.startPosition;

		NewPlayerPosition((int)startPosition.x, (int)startPosition.y);
	}
	private static void CreateMinimapCanvas()
	{
		minimapCanvas = new GameObject();
		minimapCanvas.name = "MinimapCanvas";
		minimapCanvas.AddComponent<RectTransform>();
		minimapCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		minimapCanvas.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPhysicalSize;
		minimapCanvas.AddComponent<GraphicRaycaster>();
		minimapCanvas.transform.position = new Vector3(0, 0, 0);
	}

	private static void CreateMinimapPanel()
	{
		minimapPanel = new GameObject();
		minimapPanel.name = "MinimapPanel";
		minimapPanel.AddComponent<RectTransform>();
		minimapPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minimapCanvas.GetComponent<RectTransform>().rect.width);
		minimapPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minimapCanvas.GetComponent<RectTransform>().rect.height);

		minimapPanel.AddComponent<CanvasRenderer>();
		minimapPanel.AddComponent<Image>().color = new Color(100, 100, 100, .3f);
		minimapPanel.transform.SetParent(minimapCanvas.transform);
		minimapPanel.SetActive(true);
	}

	private static void CreatePlayerMark()
	{
		playerMark = new GameObject();
		playerMark.name = "PlayerMark";
		playerMark.tag  = "PlayerMark";
		playerMark.AddComponent<Image>().sprite = playerSprite;
		playerMark.transform.localScale = new Vector3(minimapSize, minimapSize, minimapSize);
		playerMark.transform.position = new Vector3(playerPosition[0], playerPosition[1], 0);
		playerMark.transform.SetParent(minimapPanel.transform);
	}

	public static void ResetMinimap()
	{
		minimapSprites = null;
		NewPlayerPosition(0, 0);
	}

	private static void InitializeSpriteInformation()
	{
		int mapSize = Configuration.mapSize;
		minimapSprites = new Sprite[mapSize][];
		minimapSpritesRotations = new int[mapSize][];
		for (int i = 0; i < mapSize; i++)
		{
			minimapSprites[i] = new Sprite[mapSize];
			minimapSpritesRotations[i] = new int[mapSize];
		}
	}

	// Update is called once per frame
	public static void ActiveMinimap(bool active)
	{
		if (active)
		{
			CreateMinimapCanvas();
			CreateMinimapPanel();
			Draw();
			minimapPanel.SetActive(active);
		}
		else
		{
			minimapPanel.SetActive(active);
			Destroy(minimapCanvas);
		}
	}

	public static void AddToMinimap(int x, int z, GameObject room)
	{
		RoomData roomData = room.GetComponent<RoomData>();

		if (minimapSprites == null) InitializeSpriteInformation();
		minimapSprites[x][z] = roomData.sprite;
		minimapSpritesRotations[x][z] = roomData.spriteRotation;
	}

	public static void NewPlayerPosition(int x, int y)
	{
		playerPosition = new float[] { minimapSize * x * 100 * correctigFactor, minimapSize * y * 100 * correctigFactor, 0 };
	}

	public static void Draw()
	{
		CreatePlayerMark();
		ClearMinimap();
		FillMinimap();
	}

	private static void ClearMinimap()
	{
		foreach (Transform child in minimapPanel.transform)
		{
			if(child.tag != "PlayerMark") Destroy(child);
		}
	}

	private static void FillMinimap()
	{
		for (int i = 0; i < minimapSprites.Length; i++)
		{
			for (int j = 0; j < minimapSprites.Length; j++)
			{
				if (minimapSprites[i][j] != null)
				{
					CreateTile(i, j, minimapSprites[i][j], minimapSpritesRotations[i][j]);
				}
			}
		}
		HighlightTile(0, 0);
	}

	private static void CreateTile(int x, int y, Sprite sprite, int rotation)
	{
		int center = (minimapSprites.Length - 1) / 2;
		x -= center;
		y -= center;

		GameObject tile = new GameObject();
		tile.name = "tile[" + x + "]" + "[" + y + "]";

		Image tileImage = tile.AddComponent<Image>();
		tileImage.sprite = sprite;

		tile.transform.localScale = new Vector3(minimapSize, minimapSize, minimapSize);
		tile.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
		tile.transform.position = new Vector3(minimapSize * x * 100 * correctigFactor, minimapSize * y * 100 * correctigFactor, 0);
		tile.transform.SetParent(minimapPanel.transform);
	}

	private static void HighlightTile(int x, int y)
	{
		GameObject tileHighlight = new GameObject();
		tileHighlight.name = "tile-Highlight[" + x + "]" + "[" + y + "]";

		Image tileHighlightImage = tileHighlight.AddComponent<Image>();
		tileHighlightImage.color = Color.blue - new Color(0, 0, 0, 0.8f);

		tileHighlight.transform.localScale = new Vector3(minimapSize * correctigFactor, minimapSize * correctigFactor, minimapSize * correctigFactor);
		tileHighlight.transform.position = new Vector3(minimapSize * x * 100 * correctigFactor, minimapSize * y * 100 * correctigFactor, 0);
		tileHighlight.transform.SetParent(minimapPanel.transform);
	}
}
