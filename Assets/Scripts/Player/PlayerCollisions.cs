using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
			case "Tresure":
				//TresureData Tdata = other.GetComponent<TresureData>();
				//Debug.Log(Tdata.name + "\n" + Tdata.description + "\nAtributo: " + Tdata.attribute + "\nValor: " + Tdata.value);
				break;
			case "EnemyProjectile":
				//EnemyProjectileData EPdata = other.GetComponent<EnemyProjectileData>();
				//Debug.Log(EPdata.name + "\n" + EPdata.description + "\nEfecto: " + EPdata.effect + "\nValor: " + EPdata.value + "\nValor del efecto: " + EPdata.effectValue);
				break;
			case "Door":
				GameObject door = other.gameObject;
				MapGenerator.MovePlayer(transform.gameObject, door, Camera.main);
				break;
			case "EndLevel":
				if (other.gameObject.GetComponent<LadderScript>().allowLadder)
				{
					MapGenerator.NewLevel(transform.gameObject, Camera.main);
				}
				break;
			case "Tag...":

				break;
		}
	}
}
