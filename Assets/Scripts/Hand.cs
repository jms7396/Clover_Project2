using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class Hand : MonoBehaviour
{
	/// <summary>
	/// Keys listed in order to be associated with the hand of cards
	/// </summary>
	public KeyCode[] keyList = new KeyCode[HAND_SIZE];

	public GameObject playerCardEventHandler;

	public const int HAND_SIZE = 3;

	public Player thisPlayer;

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
			uicard.owner = thisPlayer;
			uicard.buttonText.text = keyList[Count].ToString();

			cardGO.transform.SetParent(this.transform, false);
			cardGO.SetActive(true);

			return true;
		}
		return false;
	}

	void Start()
	{
		thisPlayer = (thisPlayer) ? thisPlayer : GetComponent<Player>();
	}

	// TODO: make each card listen for itself?
	void Update()
	{
		if (!Input.anyKeyDown) return;

		for (int i = 0; i < keyList.Length; i++)
		{
			var key = keyList[i];

			if (Input.GetKeyDown(key))
			{
				var card = this[i];
				ExecuteEvents.ExecuteHierarchy<IPlayerCardEventHandler>(
					playerCardEventHandler,
					null,
					(x, y) => x.CardChosen(card)
				);
			}
		}
	}

}