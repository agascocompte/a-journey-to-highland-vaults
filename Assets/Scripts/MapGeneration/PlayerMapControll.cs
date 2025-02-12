using UnityEngine;

public class PlayerMapControll : MonoBehaviour
{
	public float minimapSize = 1f;
	public Sprite playerSprite;
	private bool active = false;
	private bool toggle = false;		// esta funcionalidad (en true) esta buggeada, pero es increiblemente util para testing
	private bool samePulsation = false;

	void FixedUpdate()
	{
		// Store the input of tab. Holding down = true, not so = false
		bool tab = Input.GetButton("Minimap");

		// if tab then show Minimap, else hide Minimap
		if (!toggle)
        {
			if (tab)
			{
				if (!active) MinimapManager.ActiveMinimap(tab);
				active = true;
			}
			else
			{
				if (active) MinimapManager.ActiveMinimap(tab);
				active = false;
			}
		}
		else
        {
			if (tab)
            {
				if (!samePulsation && !active) MinimapManager.ActiveMinimap(true);
				if (!samePulsation &&  active) MinimapManager.ActiveMinimap(false);
				
				samePulsation = true;
			}
			else
            {
				active = !active;
				samePulsation = false;
			}
        }
		
	}        
}
