using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;
using System;

public class Hand : MonoBehaviour
{
	/// <summary>
	/// Keys listed in order to be associated with the hand of cards
	/// </summary>
	[ContextMenuItem("Player 1 presets", "setKeyListPresetPlayer1")]
	[ContextMenuItem("Player 2 presets", "setKeyListPresetPlayer2")]
	public KeyCode[] keyList = new KeyCode[HAND_SIZE];

	public void setKeyListPresetPlayer1()
	{
		keyList = new KeyCode[HAND_SIZE] { KeyCode.Q, KeyCode.A, KeyCode.Z };
	}
	public void setKeyListPresetPlayer2()
	{
		keyList = new KeyCode[HAND_SIZE] { KeyCode.O, KeyCode.K, KeyCode.M };
	}


	public const int HAND_SIZE = 3;

	public Player Player;

	public GameObject cardPrototype;

	public int Count
	{
		get
		{
			return GetComponentsInChildren<UICard>().Count((x) => x != null);
		}
	}

	public bool IsFull
	{
		get
		{
			return Count >= HAND_SIZE;
		}
	}

	public bool IsEmpty
	{
		get
		{
			return Count <= 0;
		}
	}

	public UICard this[int value]
	{
		get
		{
			return GetComponentsInChildren<UICard>()[value];
		}
	}

	/// <summary>
	/// Puts a card in this hand, if able to.
	/// </summary>
	/// <param name="card"></param>
	/// <returns>True if successful, false if full.</returns>
	public bool Insert(Card card)
	{
		if (!IsFull) {
			var cardGO = Instantiate(cardPrototype) as GameObject;
			var uicard = cardGO.GetComponent<UICard>();

			uicard.Card = card;
			uicard.owner = Player;

			cardGO.transform.SetParent(this.transform, false);
			cardGO.SetActive(true);

			setupButtonText();

			return true;
		}
		return false;
	}

	/// <summary>
	/// Make sure the keyboard buttons marked on each child card is accurate
	/// </summary>
	public void setupButtonText()
	{
		int i = 0;
		foreach (var uiCard in GetComponentsInChildren<UICard>())
		{
			uiCard.buttonText.text = keyList[i].ToString();
			i++;
		}
	}
		
	public void Discard(Card card)
	{
		foreach (var uiCard in GetComponentsInChildren<UICard>())
		{
			if (uiCard.card == card)
			{
				uiCard.gameObject.SetActive(false);
				Destroy(uiCard.gameObject);
			}
		}

		setupButtonText();
	}

	void Start()
	{
		Player = (Player) ? Player : GetComponent<Player>();
	}

	void PickCard(Card card)
	{
		Player.PickCard(card);
	}

	void PickCard(GameObject obj)
	{
		var card = obj.GetComponent<UICard>();

		if (card == null) { throw new Exception("Not a valid card object"); }

		PickCard(card);
	}

	void Update()
	{
		if (!Input.anyKeyDown) return;

		for (int i = 0; i < keyList.Length; i++)
		{
			var key = keyList[i];

			if (Input.GetKeyDown(key))
			{
				// TODO/FIXME: possible null ref exception?
				PickCard(this[i]);
				return;
			}
		}
	}
}