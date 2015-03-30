using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleLogger : MonoBehaviour
{
	public GameObject textPrototype;
	public RectTransform logWindow;

	void Start() { }
	void Update() { }

	public void Log(string message)
	{
		if (this.enabled)
		{
			var txt = Instantiate(textPrototype) as GameObject;

			txt.GetComponent<Text>().text = message;

			txt.transform.SetParent(logWindow, false);
		}
	}

	public void LogPlayerHealthChange(Player player, int oldHealth, int newHealth)
	{
		Debug.Log(player + " health " + oldHealth + " -> " + newHealth);
		Log(player + " now has " + newHealth + "HP");
	}

	public void LogPlayerStateChange(Player player, PlayerState oldState, PlayerState newState)
	{
		Debug.Log(player + " state " + oldState + " -> " + newState);

		if (newState == PlayerState.Chosen && oldState == PlayerState.Deciding)
		{
			Log(player + " is ready.");
		}
	}

	public void LogPlayerDrewCard(Player player, Card card)
	{
		string msg = player + " drew " + card;
		Debug.Log(msg);
		if (player.State != PlayerState.Setup)
		{
			Log(msg);
		}
	}
}