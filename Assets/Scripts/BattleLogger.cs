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
		Debug.Log("BattleLog: " + message);

		if (this.enabled)
		{
			var txt = Instantiate(textPrototype) as GameObject;

			txt.GetComponent<Text>().text = message;

			txt.transform.SetParent(logWindow, false);
		}
		else
		{
			Debug.Log("... but disabled.");
		}
	}
}
