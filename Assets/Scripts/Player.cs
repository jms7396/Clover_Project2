using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Player : MonoBehaviour
{
	public enum State
	{
		None,
		Deciding,
		Chosen,
		Dead
	}

	public int Health;
	public string Name;
	public Card cardChoice = null;
	public Deck deck;
	public Hand hand;
	public State state;

	public GameObject playerCardEventHandler;

	void Start() { }

	void Update() { }

	public bool CanPickCard
	{
		get
		{
			return state == State.Deciding || state == State.Chosen;
		}
	}

	public bool HasChosen
	{
		get
		{
			return state == State.Chosen;
		}
	}

	public Card CardChoice
	{
		get { return cardChoice; }
		set
		{
			cardChoice = value;
			state = State.Chosen;
		}
	}

	/// <summary>
	/// Reset the chosen card and associated player state
	/// </summary>
	public void ResetChoice()
	{
		state = State.Deciding;
		cardChoice = null;
	}

	public void Discard(Card card)
	{
		hand.Discard(card);
	}

	public bool drawTillFull()
	{
		for (int i = 0; i < Hand.HAND_SIZE; i++)
		{
			if (!draw()) {
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
		return "Player " + Name;
	}
}