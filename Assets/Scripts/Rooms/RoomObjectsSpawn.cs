using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomObjectsSpawn : MonoBehaviour
{
	public List<Transform> tableSpawnPoints;
	public List<GameObject> tables;
	public int maxTables = 2;

	public List<Transform> boxesSpawnPoints;
	public List<GameObject> boxes;
	public int maxBoxes = 4;

	public List<Transform> enemiesSpawnPoints;
	public List<GameObject> enemies;
	public int maxEnemies = 0;
	private bool roomFinished = false;

	private int actualEnemies;
	private List<GameObject> enemiesList;

	private List<GameObject> doorsPrefabList;
	private List<GameObject> doorColliderList;

	public AudioClip openDoorsClip;
	private AudioSource audio;


	private void Awake()
	{
		audio = GetComponent<AudioSource>();

		enemiesList = new List<GameObject>();
		doorsPrefabList = new List<GameObject>();
		doorColliderList = new List<GameObject>();
		foreach (Transform item in transform)
		{
			if (item.name.Equals("Doors"))
            {
				foreach (Transform door in item)
				{
					doorColliderList.Add(door.gameObject);
				}
			}
			if (item.name.Equals("DoorPrefabs")) {
                foreach (Transform door in item)
                {
					doorsPrefabList.Add(door.gameObject);
				}
			}
		}

		SpawnTables();
		SpawnBoxes();


		SpawnEnemies();

		actualEnemies = maxEnemies;
	}

	public void KillEnemies()
	{
		foreach (GameObject enemy in enemiesList)
		{
			Destroy(enemy);
		}
		actualEnemies = 0;
	}


	private void Update()
	{
		if (actualEnemies <= 0 && !roomFinished)
		{
			OpenDoors();
			MapGenerator.SpawnLadder();
			roomFinished = true;
		}
	}

	void SpawnTables()
	{
		if (tableSpawnPoints != null && tableSpawnPoints.Count >= maxTables)
		{
			for (var i = 0; i < maxTables; i++)
			{
				int spawnPointIndex = Random.Range(0, tableSpawnPoints.Count);
				int tableTypeIndex = Random.Range(0, tables.Count);
				GameObject tableCreated = Instantiate(tables[tableTypeIndex], tableSpawnPoints[spawnPointIndex].position, tableSpawnPoints[spawnPointIndex].rotation);
				tableCreated.transform.Rotate(0, Random.Range(0, 360), 0);
				tableCreated.transform.position = new Vector3(tableCreated.transform.position.x, 0, tableCreated.transform.position.z);
				tableSpawnPoints.RemoveAt(spawnPointIndex);
			}
		}
	}

	void SpawnBoxes()
	{
		if (boxesSpawnPoints != null && boxesSpawnPoints.Count >= maxBoxes)
		{
			for (var i = 0; i < maxBoxes; i++)
			{
				int spawnPointIndex = Random.Range(0, boxesSpawnPoints.Count);
				int boxTypeIndex = Random.Range(0, boxes.Count);
				GameObject boxesCreated = Instantiate(boxes[boxTypeIndex], boxesSpawnPoints[spawnPointIndex].position, boxesSpawnPoints[spawnPointIndex].rotation);
				boxesCreated.transform.Rotate(0, Random.Range(0, 360), 0);
				boxesCreated.transform.position = new Vector3(boxesCreated.transform.position.x, 0, boxesCreated.transform.position.z);
				boxesSpawnPoints.RemoveAt(spawnPointIndex);
			}
		}
	}

	void SpawnEnemies()
	{
		if (enemiesSpawnPoints != null && enemiesSpawnPoints.Count >= maxEnemies)
		{
			for (var i = 0; i < maxEnemies; i++)
			{
				int spawnPointIndex = Random.Range(0, enemiesSpawnPoints.Count);
				int enemyTypeIndex = Random.Range(0, enemies.Count);
				GameObject enemyCreated = Instantiate(enemies[enemyTypeIndex], enemiesSpawnPoints[spawnPointIndex].position, enemiesSpawnPoints[spawnPointIndex].rotation);
				//enemyCreated.transform.Rotate(0, Random.Range(0, 360), 0);
				enemiesSpawnPoints.RemoveAt(spawnPointIndex);
				enemiesList.Add(enemyCreated);
			}
		}
	}

	public void updateEnemyCount()
	{
		actualEnemies -= 1;
	}

	void OpenDoors()
	{
		foreach (GameObject doorPrefab in doorsPrefabList)
		{
			doorPrefab.SetActive(false);
		}

		foreach (GameObject collider in doorColliderList)
		{
			collider.SetActive(true);
		}
		audio.PlayOneShot(openDoorsClip, 0.7f);
	}
}
