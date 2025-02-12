using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
	public GameObject player;
	public Camera mainCamera;
	public void StartGame()
	{
		foreach (var item in SceneManager.GetActiveScene().GetRootGameObjects())
		{
			item.SetActive(false);
		}
		Scene mainMenu = SceneManager.GetActiveScene();
		MapGenerator.StartGeneration(player, mainCamera, mainMenu);
	}
}
