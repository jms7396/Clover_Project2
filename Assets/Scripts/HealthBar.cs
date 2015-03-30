using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
	public Slider slider;
	public Player player;

	private bool warned = false;

	void Start() { }

	void OnEnable()
	{
		if (player) player.healthChange.AddListener(OnPlayerHealthChange);
	}

	void OnDisable()
	{
		if (player) player.healthChange.RemoveListener(OnPlayerHealthChange);
	}

	void Update()
	{
		if (player)
		{
			slider.maxValue = player.MaxHealth;
			slider.value = player.Health;
		}
	}

	public void OnPlayerHealthChange(Player player, int oldHealth, int newHealth)
	{
		if (player && !warned)
		{
			Debug.LogWarning("This health bar did not have a player reference, and thus may not have an accurate range");
			warned = true;
		}

		slider.value = newHealth;
	}
}
