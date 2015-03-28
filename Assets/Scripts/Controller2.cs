using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;


public class Controller2 : MonoBehaviour, IPlayerCardEventHandler {
	public Player p1;
	public Player p2;

	public BattleLogger log;

	public enum State
	{
		None,
		PlayersChoosing,
		PlayersLocked
	};

	public State state;

	void Awake()
	{
		p1.deck = new Deck();
		p2.deck = new Deck();
		p1.state = Player.State.Deciding;
		p2.state = Player.State.Deciding;
	}

	void Start()
	{
		p1.drawTillFull();
		p2.drawTillFull();
		state = State.PlayersChoosing;
	}

	void Update()
	{
		switch (state)
		{
			case State.None:
				break;
			case State.PlayersChoosing:
				if (p1.HasChosen && p2.HasChosen)
				{
					state = State.PlayersLocked;
				}
				break;
			case State.PlayersLocked:
				CardChecks(p1, p2, p1.CardChoice, p2.CardChoice);
				state = State.PlayersChoosing;
				p1.state = Player.State.Deciding;
				p2.state = Player.State.Deciding;
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Print a message to the battle log.
	/// </summary>
	/// <param name="message"></param>
	void BattleLog(string message)
	{
		log.Log(message);
	}

	#region Event Handlers

	public void CardDrawn(Player player, Card card)
	{
		BattleLog(player + " draws " + card.cardName);
	}

	public void TookDamage(Player player, int amount)
	{
		throw new NotImplementedException();
	}

	public void CardChosen(Player player, Card card)
	{
		if (player.CanPickCard)
		{
			// if player has not chosen a card yet, notify that they are ready now
			// TODO: move into state property for player(?)
			// TODO: replace with observer pattern
			if (!player.HasChosen) {
				BattleLog(player + " is ready.");
				player.state = Player.State.Chosen;
			}
			player.cardChoice = card;
		}
	}

	public void CardChosen(UICard card)
	{
		CardChosen(card.owner, card.card);
	}

	public void CardPlayed(Player player, Card card)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// React to a selected card.
	/// </summary>
	/// <param name="card"></param>
	public void CardClicked(GameObject card)
	{
		var uiCard = card.GetComponent<UICard>();
		Debug.Log(uiCard.Name + " was clicked");
		CardChosen(uiCard);
	}
	#endregion

	public void CardChecks(Player p1, Player p2, Card p1Card, Card p2Card)
	{
		// Attack Checks

		// TODO: observer pattern, again
		BattleLog(p1 + " now has " + p1.Health + "HP");
		BattleLog(p2 + " now has " + p2.Health + "HP");
	}
}
