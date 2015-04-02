using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerReadyVisual : MonoBehaviour {

    public Toggle toggle;

	// Use this for initialization
	void Start () {
        if (!toggle) toggle = GetComponent<Toggle>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlayerStateChange(Player player, PlayerState oldState, PlayerState newState)
    {
        toggle.isOn = player.HasChosen;
    }
}
