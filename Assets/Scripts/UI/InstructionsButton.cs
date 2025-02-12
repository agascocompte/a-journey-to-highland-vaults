using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsButton : MonoBehaviour
{
	public GameObject instructions;

	public void ShowInstructions()
	{
		instructions.SetActive(true);
	}

	public void CloseInstructions()
	{
		instructions.SetActive(false);
	}
}
