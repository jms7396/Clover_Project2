using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// A way of bundling a card and a player together,
/// and attaching a card to a gameobject without monobehavior for it.
/// </summary>
public class UICard : MonoBehaviour {

	public Card card;

	[HideInInspector]
	public Player owner;

	public Text text;

	void Start () { }
	
	void Update () { }

	public string Name
	{
		get
		{
			return text.text;
		}
		set
		{
			text.text = value;
			this.name = value;
		}
	}

	public Card Card
	{
		set
		{
			card = value;
			Name = card.cardName;
		}
	}

	public static implicit operator Card(UICard self)
	{
		return self.card;
	}
}
