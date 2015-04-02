using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
	public Slider slider;
	public Player player;
    public Text text;

	private bool warned = false;

	void Start() { }

	void OnEnable()
	{
        if (player) player.healthChange.AddListener(OnPlayerHealthChange);
        else Debug.LogWarning("No player attached to HealthBar!");

        if (!slider) Debug.LogWarning("No slider attached to HealthBar!");
        if (!text) Debug.LogWarning("No text attached to HealthBar!");
	}

	void OnDisable()
	{
		if (player) player.healthChange.RemoveListener(OnPlayerHealthChange);
	}

	void Update()
	{
		if (player)
		{
            if (slider)
            {
                slider.maxValue = player.MaxHealth;
                slider.value = player.Health;
            }

            if (text) text.text = player.Health.ToString();
		}
	}

	public void OnPlayerHealthChange(Player player, int oldHealth, int newHealth)
	{
		if (player && !warned)
		{
			Debug.LogWarning("This health bar did not have a player reference, and thus may not have an accurate range");
			warned = true;
		}

		if (slider) slider.value = newHealth;
        if (text) text.text = player.Health.ToString();
	}
}
