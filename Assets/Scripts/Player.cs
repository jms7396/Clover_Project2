using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum PlayerState
{
	None,
	Setup,
	Deciding,
	Chosen,
	Dead
}

#region UnityEvents

/// <summary>
/// An event fired when the player's state changes.
/// Parameters: player, old state, new state
/// </summary>
[System.Serializable]
public class PlayerStateChangedEvent : UnityEvent<Player, PlayerState, PlayerState> { };

/// <summary>
/// An event fired when the player's health changes.
/// Parameters: player, old health, new health
/// </summary>
[System.Serializable]
public class HealthChangedEvent : UnityEvent<Player, int, int> { };

/// <summary>
/// An event fired when the player chooses a card.
/// Parameters: player, old card choice, new card choice.
/// Most listeners will probably only use the new or old value.
/// Note that resetting a player's choice means the new value will be null!
/// </summary>
[System.Serializable]
public class CardChosenEvent : UnityEvent<Player, Card, Card> { };

/// <summary>
/// An event fired when the player draws a card.
/// </summary>
[System.Serializable]
public class CardDrawnEvent : UnityEvent<Player, Card> { };

#endregion

public class Player : MonoBehaviour
{
	#region Fields, Properties
	public Deck deck;
	public Hand hand;

	[SerializeField]
	private string _name;
	public string Name
	{
		get { return _name; }
		set
		{
			gameObject.name = "Player \"" + value + '"';
			_name = value;
		}
	}

	[SerializeField]
	private int health = 20;
	[SerializeField]
	[ContextMenuItem("Set to current health", "setMaxHealthToStarterHealth")]
	public int MaxHealth = 20;
	public int Health
	{
		get { return health; }
		set
		{
			var old = health;
			value = Mathf.Clamp(value, 0, MaxHealth);
			health = value;
			healthChange.Invoke(this, old, value);
		}
	}


	private PlayerState state;
	public PlayerState State
	{
		get { return state; }
		set
		{
			var old = state;
			state = value;
			stateChange.Invoke(this, old, value);
		}
	}


	private Card cardChoice = null;
	public Card CardChoice
	{
		get { return cardChoice; }
		set
		{
			var old = cardChoice;
			cardChoice = value;
			cardChange.Invoke(this, old, value);
		}
	}

	#endregion

	#region Event Listeners
	public HealthChangedEvent healthChange;
	public PlayerStateChangedEvent stateChange;
	public CardChosenEvent cardChange;
	public CardDrawnEvent cardDrawn;
	#endregion

	void Start() { }

	void Update() { }

	public bool CanPickCard
	{
		get
		{
			return state == PlayerState.Deciding || state == PlayerState.Chosen;
		}
	}

	public bool HasChosen
	{
		get
		{
			return state == PlayerState.Chosen;
		}
	}

	/// <summary>
	/// Reset the chosen card and associated player state
	/// </summary>
	public void ResetChoice()
	{
		var oldChoice = cardChoice;
		var oldState = state;
		cardChoice = null;
		state = PlayerState.Deciding;
		cardChange.Invoke(this, oldChoice, cardChoice);
		stateChange.Invoke(this, oldState, state);
	}

	/// <summary>
	/// Pick a card, changing state accordingly.
	/// </summary>
	/// <param name="card"></param>
	public void PickCard(Card card)
	{
		CardChoice = card;
		State = PlayerState.Chosen;
	}

	public void PickCard(UICard card)
	{
		PickCard(card.card);
	}

	public void Discard(Card card)
	{
		hand.Discard(card);
	}

	public bool drawTillFull()
	{
		for (int i = 0; i < Hand.HAND_SIZE; i++)
		{
			if (!draw())
			{
				return false;
			}
		}
		return true;
	}

	// TODO: replace with exception
	/// <summary>
	/// Draw a card from the deck, put it in the hand
	/// </summary>
	/// <returns></returns>
	public bool draw()
	{
		if (hand.IsFull)
		{
			Debug.LogWarning(this + "'s hand is full, but someone tried to draw anyway.");
			return false;
		}

		var card = deck.Draw();

		if (card == null)
		{
			Debug.LogError("Deck was empty when " + this + " drew");
			return false;
		}

		cardDrawn.Invoke(this, card);

		var insertSuccess = hand.Insert(card);

		if (!insertSuccess)
		{
			Debug.LogError("Couldn't add the card " + card + " to " + this + "'s hand for some reason");
			return false;
		}

		return true;
	}

	// meant to handle when the player is struck by enemy card. 
	public void TakeDamage(int i)
	{
		Health -= i;
		Debug.Log("Player " + Name + " Takes " + i + " damage!");
	}

	public override string ToString()
	{
		return (Name.Length != 0) ? "Player " + Name : "An Unnamed Player";
	}

	#region Editor Helper Methods

	public void LogPlayerCardChosen(Player player, Card oldCard, Card newCard)
	{
		Debug.Log(player + " chose " + newCard);
	}

	public void setMaxHealthToStarterHealth()
	{
		MaxHealth = health;
	}
	#endregion
}