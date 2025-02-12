using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
	private static readonly bool debug = false;

	private static readonly int mapSize = Configuration.mapSize;
	private static readonly int totalRooms = Configuration.totalRooms;
	private static int remainingRooms;
	private static Scene thisMainMenu;

	private static List<GameObject> allRooms;
	private static GameObject ladder;
	private static Dictionary<Vector2, Scene> scenes;
	private static List<Vector2> filledScenesPositions;
	private static int center;

	private static Vector2 playerCurrentPosition;
	private static List<Vector2> visitedRooms;
	private static readonly Vector2 UP = Vector2.up;
	private static readonly Vector2 RIGHT = Vector2.right;
	private static readonly Vector2 DOWN = Vector2.down;
	private static readonly Vector2 LEFT = Vector2.left;

	public static void StartGeneration(GameObject player, Camera mainCamera, Scene mainMenu)
	{
		thisMainMenu = mainMenu;
		var unprocesedRooms = Resources.LoadAll("Rooms", typeof(GameObject));
		var unprocesedLadder = Resources.LoadAll("LadderNextLevel");
		allRooms = new List<GameObject>();
		scenes = new Dictionary<Vector2, Scene>();
		filledScenesPositions = new List<Vector2>();
		remainingRooms = totalRooms;

		foreach (var room in unprocesedRooms)
		{
			allRooms.Add((GameObject)room);
		}
        foreach (var l in unprocesedLadder)
        {
			ladder = (GameObject) l;
        }

		center = (mapSize - 1) / 2;

		BuildMap(player, mainCamera);
	}

	private static void BuildMap(GameObject player, Camera mainCamera)
	{
		Vector2 startPosition = Configuration.startPosition;
		if (debug) Debug.Log("Start FormMap.");
		FormMap(startPosition);
		if (debug) DebugShowScenes();
		if (debug) Debug.Log("Finished FormMap.");

		if (debug) Debug.Log("Start FillMap.");
		FillMap(startPosition);
		if (debug) DebugShowScenesOnMatrix();
		if (debug) Debug.Log("Finished FillMap.");

		if (debug) Debug.Log("Start UnLoadMap.");
		UnLoadMap();
		if (debug) Debug.Log("Finished UnLoadMap.");
		if (debug) Debug.Log("Start PlacePlayer.");
		PlacePlayer(startPosition, player, mainCamera);
		if (debug) Debug.Log("Finished PlacePlayer.");
	}

	private static void FormMap(Vector2 startPosition)
	{
		scenes[startPosition] = CreateNewScene(startPosition);
		remainingRooms--;

		List<Vector2> borders = new List<Vector2>(GetBorders());

		while (remainingRooms > 0)
		{
			Vector2 selectedBorder = borders[Random.Range(0, borders.Count)];
			Vector2 orientation = GetNextOrientation(selectedBorder);
			Vector2 newPosition = selectedBorder + orientation;
			scenes[newPosition] = CreateNewScene(newPosition);

			borders = new List<Vector2>(GetBorders());
			remainingRooms--;
		}
	}

	//####################################################################################
	//---------------FUNCIONES AUXILIARES DE FormMap(Vector2 startPosition)---------------
	private static Vector2 GetNextOrientation(Vector2 border)
	{
		var orientations = new Vector2[] {UP, RIGHT, DOWN, LEFT};
		var posibleOrientations = new List<Vector2>();
		Vector2 orientatedBorder;

		for (int i = 0; i < orientations.Length; i++)
		{
			orientatedBorder = border + orientations[i];
			if (!IsOut(orientatedBorder) && !scenes.ContainsKey(orientatedBorder)) posibleOrientations.Add(orientations[i]);
		}

		if (posibleOrientations.Count == 0) Debug.LogError("####################-NO-POSIBLE-ORIENTATION-####################");
		int orientation = Random.Range(0, posibleOrientations.Count);
		return posibleOrientations[orientation];
	}

	private static List<Vector2> GetBorders()
	{
		var borders = new List<Vector2>();
		foreach (var position in scenes.Keys)
		{
			if (IsBorder(position)) borders.Add(position);
		}
		return borders;
	}

	private static bool IsBorder(Vector2 position)
	{
		return 
			(	!scenes.ContainsKey(position + UP)		&& !IsOut(position + UP))
			|| (!scenes.ContainsKey(position + RIGHT)	&& !IsOut(position + RIGHT))
			|| (!scenes.ContainsKey(position + DOWN)	&& !IsOut(position + DOWN))
			|| (!scenes.ContainsKey(position + LEFT)	&& !IsOut(position + LEFT))
			;
	}

	private static bool IsOut(Vector2 position)
	{
		return position.y > center
			|| position.x > center
			|| position.y < -center
			|| position.x < -center;
	}

	private static Scene CreateNewScene(Vector2 position)
	{
		return SceneManager.CreateScene(Configuration.currentLevel + "-Scene(" + (int)position.x + "," + (int)position.y + ")");
	}
	//---------------FUNCIONES AUXILIARES DE FormMap(Vector2 startPosition)---------------
	//####################################################################################


	private static void FillMap(Vector2 startPosition)
	{
		remainingRooms = totalRooms;

		while (remainingRooms > 0)
		{
			Vector2 positionToFill = GetNextPositionToFill(startPosition);

			bool[] positionToFillOrientations = GetDoorsOrientation(positionToFill);

			List<GameObject> posibleRooms = GetRoomsWithOrientatedDoors(positionToFillOrientations, allRooms);
			GameObject room = SelectRoom(posibleRooms);

			string positionToFillOrientationsString = "";
			foreach (var item in positionToFillOrientations)
			{
				positionToFillOrientationsString += item + "\t";
			}

			FillScene(room, positionToFill);

			filledScenesPositions.Add(positionToFill);
			remainingRooms--;
		}
	}

	//####################################################################################
	//---------------FUNCIONES AUXILIARES DE FillMap(Vector2 startPosition)---------------

	private static Vector2 GetNextPositionToFill(Vector2 startPosition)
	{
		var visitedPositons = new List<Vector2> { startPosition };

		Vector2[] toCheckPositions = new Vector2[] { UP, RIGHT, DOWN, LEFT };

		foreach (var checkingPosition in toCheckPositions)
		{
			if (scenes.ContainsKey(checkingPosition) && !filledScenesPositions.Contains(checkingPosition))
			{
				visitedPositons.Add(checkingPosition);
				return GetNextPosition(checkingPosition);
			}
		}
		return startPosition;

		Vector2 GetNextPosition(Vector2 position)
		{
			toCheckPositions = new Vector2[] {	position + UP,
												position + RIGHT,
												position + DOWN,
												position + LEFT };

			foreach (var checkingPosition in toCheckPositions)
			{

				if (scenes.ContainsKey(checkingPosition) && !visitedPositons.Contains(checkingPosition))
				{
					visitedPositons.Add(checkingPosition);
					if (!filledScenesPositions.Contains(checkingPosition)) return GetNextPosition(checkingPosition);
				}
			}
			filledScenesPositions.Add(position);
			return position;
		}
	}

	private static List<GameObject> GetRoomsWithOrientatedDoors(bool[] orientations, List<GameObject> rooms)
	{
		var reducedRooms = new List<GameObject>();
		foreach (var room in rooms)
		{
			if (RoomHasOrientatedDoors(orientations, room)) reducedRooms.Add(room);
		}
		return new List<GameObject>(reducedRooms);
	}

	private static bool RoomHasOrientatedDoors(bool[] orientations, GameObject room)
	{
		RoomData roomData = room.GetComponent<RoomData>();
		bool correct = true;
		for (int orientation = 0; orientation < orientations.Length; orientation++)
		{
			correct = correct && roomData.hasDoor[orientation] == orientations[orientation];
		}
		return correct;
	}

	private static bool[] GetDoorsOrientation(Vector2 position)
	{
		bool[] orientations = new bool[] {false, false, false, false};
		var randomDoors = new List<int>();
		var toCheckPositions = new Vector2[] {	position + UP,
												position + RIGHT,
												position + DOWN,
												position + LEFT };

		int doorsNeeded = 1;
		for (int i = 0; i < toCheckPositions.Length; i++)
		{
			Vector2 checkingPosition = toCheckPositions[i];
			

			if (scenes.ContainsKey(checkingPosition))
			{
				if (!filledScenesPositions.Contains(checkingPosition))
				{
					orientations[i] = Random.Range(0, 2) > 0;
					randomDoors.Add(i);
				}
				else
				{
					GameObject room = GetRoomOfScene(scenes[checkingPosition]);
					RoomData roomData = room.GetComponent<RoomData>();
					orientations[i] = roomData.hasDoor[(i+2)%4];
					if (doorsNeeded < 4) doorsNeeded++;
				}
			}
		}

		int doorsToCreate = 0;
		foreach (bool orientation in orientations)
		{
			if (orientation) doorsToCreate++;
		}

		for (int i = doorsToCreate; i < doorsNeeded; i++)
		{
			if (randomDoors.Count == 0) continue;
			int selected = Random.Range(0, randomDoors.Count);
			orientations[randomDoors[selected]] = true;
			randomDoors.Remove(selected);
		}
		if (!(orientations[0] || orientations[1] || orientations[2] || orientations[3]))
		{
			orientations[randomDoors[Random.Range(0, randomDoors.Count)]] = true;
		}

		if (!(orientations[0] || orientations[1] || orientations[2] || orientations[3]))
		{
			Debug.LogError("Se necesita una habitacion sin puertas, lo que no deberia ser posible.");
		}
		return orientations;
	}

	private static GameObject GetRoomOfScene(Scene scene)
	{
		foreach (var item in scene.GetRootGameObjects())
		{
			if (item.CompareTag("Room")) return item;
		}

		throw new System.InvalidOperationException("Unreachable code reached.");
	}

	private static Scene FillScene(GameObject room, Vector2 position)
	{
		Scene toFillScene = scenes[position];
		SceneManager.SetActiveScene(toFillScene);
		Instantiate(room);

		return toFillScene;
	}

	private static GameObject SelectRoom(List<GameObject> rooms)
	{
		if (rooms.Count == 0) Debug.LogError("Lista de habitaciones pasada vacia");
		int roomSelector = Random.Range(0, rooms.Count);
		return rooms[roomSelector];
	}
	//---------------FUNCIONES AUXILIARES DE FillMap(Vector2 startPosition)---------------
	//####################################################################################

	private static void UnLoadMap()
	{
		foreach (var scene in scenes.Values)
		{
			foreach (var item in scene.GetRootGameObjects())
			{
				item.SetActive(false);
			}
		}
	}

	private static void PlacePlayer(Vector2 startPosition, GameObject player, Camera mainCamera)
	{
		playerCurrentPosition = startPosition;
		SceneManager.SetActiveScene(scenes[startPosition]);
		GetRoomOfScene(scenes[startPosition]).GetComponent<RoomObjectsSpawn>().KillEnemies();

		int health = player.GetComponent<PlayerHealth>().currentHealth;
		int shield = player.GetComponent<PlayerHealth>().shield;

		GameObject instamtiatePlayer = Instantiate(player);

		GameObject instamtiateCamera = Instantiate(mainCamera).gameObject;

		instamtiatePlayer.transform.position = GetRoomOfScene(scenes[startPosition]).GetComponent<RoomData>().center;
		instamtiateCamera.transform.position = instamtiatePlayer.transform.position + Configuration.cameraDistance;

		instamtiateCamera.GetComponent<SmoothFollow>().playerTransform = instamtiatePlayer.transform;

		foreach (var item in scenes[startPosition].GetRootGameObjects())
		{
			item.SetActive(true);
		}

		AddToMinimap(startPosition, GetRoomOfScene(scenes[startPosition]));
		var playerMapControll = player.GetComponent<PlayerMapControll>();
		MinimapManager.Preparation(playerMapControll.minimapSize, playerMapControll.playerSprite);
		visitedRooms = new List<Vector2>() { startPosition };

		instamtiatePlayer.GetComponent<PlayerHealth>().currentHealth = health;
		instamtiatePlayer.GetComponent<PlayerHealth>().shield = shield;

		instamtiateCamera.GetComponent<AudioListener>().enabled = Volume.isVolumeActive;
		Debug.Log(Volume.isVolumeActive);
	}

	public static void MovePlayer(GameObject player, GameObject door, Camera mainCamera)
	{
		foreach (var item in scenes[playerCurrentPosition].GetRootGameObjects())
		{
			item.SetActive(false);
		}

		Vector3 playerSpawnPosition = Vector3.zero;
		switch (door.GetComponent<DoorData>().orientation)
		{
			case 0:
				playerCurrentPosition += UP;
				playerSpawnPosition = GetRoomOfScene(scenes[playerCurrentPosition]).GetComponent<RoomData>().playerSpawns[2];
				break;
			case 1:
				playerCurrentPosition += RIGHT;
				playerSpawnPosition = GetRoomOfScene(scenes[playerCurrentPosition]).GetComponent<RoomData>().playerSpawns[3];
				break;
			case 2:
				playerCurrentPosition += DOWN;
				playerSpawnPosition = GetRoomOfScene(scenes[playerCurrentPosition]).GetComponent<RoomData>().playerSpawns[0];
				break;
			case 3:
				playerCurrentPosition += LEFT;
				playerSpawnPosition = GetRoomOfScene(scenes[playerCurrentPosition]).GetComponent<RoomData>().playerSpawns[1];
				break;

		}
		player.transform.position = playerSpawnPosition;
		mainCamera.transform.position = playerSpawnPosition + Configuration.cameraDistance; // la distancia de la camara
		Scene sceneDestino = scenes[playerCurrentPosition];

		SceneManager.MoveGameObjectToScene(player, sceneDestino);
		SceneManager.MoveGameObjectToScene(mainCamera.gameObject, sceneDestino);
		SceneManager.SetActiveScene(sceneDestino);
		foreach (var item in sceneDestino.GetRootGameObjects())
		{
			item.SetActive(true);
			if (item.CompareTag("Room") && !visitedRooms.Contains(playerCurrentPosition))
			{
				visitedRooms.Add(playerCurrentPosition);
				AddToMinimap(playerCurrentPosition, item);
			}
		}

		MinimapManager.NewPlayerPosition((int)playerCurrentPosition.x, (int)playerCurrentPosition.y);
	}

	private static void AddToMinimap(Vector2 position, GameObject room)
	{
		MinimapManager.AddToMinimap((int)position.x+center, (int)position.y+center, room);
	}

	public static void NewLevel(GameObject player, Camera mainCamera)
	{
		player.GetComponentInChildren<PlayerShooting>().ResetPowerUps();
		Configuration.currentLevel++;

		foreach (var item in scenes[playerCurrentPosition].GetRootGameObjects())
		{
			item.SetActive(false);
		}
		Scene tempScene = SceneManager.CreateScene("TempScene");
		player.transform.position = Vector3.zero;
		mainCamera.transform.position = Vector3.zero + Configuration.cameraDistance; // la distancia de la camara

		SceneManager.MoveGameObjectToScene(player, tempScene);
		SceneManager.MoveGameObjectToScene(mainCamera.gameObject, tempScene);

		DestroyMap();
		MinimapManager.ResetMinimap();

		StartGeneration(player, mainCamera, thisMainMenu);
		SceneManager.UnloadSceneAsync(tempScene);
	}

	public static void ReturnToMainManuFromDeath()
    {
		DestroyMap();
		MinimapManager.ResetMinimap();
		SceneManager.SetActiveScene(thisMainMenu);
		foreach (var item in thisMainMenu.GetRootGameObjects())
		{
			item.SetActive(true);			
		}
		ScoreManager.score = 0;
		Configuration.currentLevel = 0;
	}

	private static void DestroyMap()
	{
		foreach (var scene in scenes.Values)
		{
			SceneManager.UnloadSceneAsync(scene);
		}
	}

	public static void SpawnLadder()
    {
		if (visitedRooms.Count >= totalRooms)
		{
			GameObject currentRoom = GetRoomOfScene(scenes[visitedRooms[visitedRooms.Count - 1]]);
			Vector3 roomCenter = currentRoom.GetComponent<RoomData>().center;
			Instantiate(ladder, roomCenter, Quaternion.LookRotation(Vector3.right));
        }
    }

	//-------------------------FUNCIONES AUXILIARES PARA DEBUGEAR-------------------------
	//####################################################################################

	private static void DebugShowScenes()
	{
		string print = "";
		foreach (var scene in scenes.Values)
		{
			print += scene.name + "\n";
		}
		Debug.Log(print);
	}

	private static void DebugShowScenesOnMatrix()
	{
		string no = "-----------";
		string print = "";
		for (int y = 2; y >= -center; y--)
		{
			for (int x = -2; x <= center; x++)
			{
				if (scenes.ContainsKey(new Vector2(x, y)))
				{
					string roomName;
					if (GetRoomOfScene(scenes[new Vector2(x, y)]) == null) roomName = no;
					else roomName = GetRoomOfScene(scenes[new Vector2(x, y)]).name;
					
					print += roomName.Substring(0,11);
				}
				else
				{
					print += no;
				}
				print += "\t";
			}
			print += "\n";
		}
		Debug.Log(print);
	}
}
