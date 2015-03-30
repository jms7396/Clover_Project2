using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;


public class Controller2 : MonoBehaviour {
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
		p1.State = PlayerState.Setup;
		p2.State = PlayerState.Setup;
	}

	void Start()
	{
		p1.drawTillFull();
		p2.drawTillFull();
		p1.State = PlayerState.Deciding;
		p2.State = PlayerState.Deciding;
		state = State.PlayersChoosing;
	}

	void OnEnable()
	{
		Debug.Log("cont onen");
		p1.stateChange.AddListener(OnPlayerStateChange);
		p2.stateChange.AddListener(OnPlayerStateChange);
	}

	void OnDisable()
	{
		p1.stateChange.RemoveListener(OnPlayerStateChange);
		p2.stateChange.RemoveListener(OnPlayerStateChange);
	}

	void Update()
	{
	}

	public void OnPlayerStateChange(Player player, PlayerState oldState, PlayerState newState)
	{
		if (p1.HasChosen && p2.HasChosen)
		{
			state = State.PlayersLocked;
			EvaluateBattle();
		}
	}

	public void EvaluateBattle()
	{
		CardChecks(p1, p2, p1.CardChoice, p2.CardChoice);
		state = State.PlayersChoosing;

		p1.Discard(p1.CardChoice);
		p2.Discard(p2.CardChoice);

		p1.ResetChoice();
		p2.ResetChoice();

		p1.draw();
		p2.draw();
	}

	public void CardChecks(Player p1, Player p2, Card p1Card, Card p2Card)
	{

		// Attack Checks
		switch(p1Card.type)
		{
		case CardType.fire:
			switch(p2Card.type)
			{
			case CardType.fire:
				if(p1Card.atk > p2Card.atk)
				{
					p1Card.atk -= p2Card.atk;
					p1Card.AffectPlayer(p2);
					p1Card.atk += p2Card.atk;
				}
				else if (p1Card.atk < p2Card.atk)
				{
					p2Card.atk -= p1Card.atk;
					p2Card.AffectPlayer(p1);
					p2Card.atk += p1Card.atk;
				}
				else
				{
					log.Log("Nothing Happened!");
				}
				break;
			case CardType.water:
				p2Card.AffectPlayer(p1);
				break;
			case CardType.earth:
				p1Card.AffectPlayer(p2);
				break;
			}
			break;
		case CardType.water:
			switch(p2Card.type)
			{
			case CardType.fire:
				p1Card.AffectPlayer(p2);
				break;
			case CardType.water:
				if(p1Card.atk > p2Card.atk)
				{
					p1Card.atk -= p2Card.atk;
					p1Card.AffectPlayer(p2);
					p1Card.atk += p2Card.atk;
				}
				else if (p1Card.atk < p2Card.atk)
				{
					p2Card.atk -= p1Card.atk;
					p2Card.AffectPlayer(p1);
					p2Card.atk += p1Card.atk;
				}
				else
				{
					log.Log("Nothing Happened!");
				}
				break;
			case CardType.earth:
				p2Card.AffectPlayer(p1);
				break;
			}
			break;
		case CardType.earth:
			switch(p2Card.type)
			{
			case CardType.fire:
				p2Card.AffectPlayer(p1);
				break;
			case CardType.water:
				p1Card.AffectPlayer(p2);
				break;
			case CardType.earth:
				if(p1Card.atk > p2Card.atk)
				{
					p1Card.atk -= p2Card.atk;
					p1Card.AffectPlayer(p2);
					p1Card.atk += p2Card.atk;
				}
				else if (p1Card.atk < p2Card.atk)
				{
					p2Card.atk -= p1Card.atk;
					p2Card.AffectPlayer(p1);
					p2Card.atk += p1Card.atk;
				}
				else
				{
					log.Log("Nothing Happened!");
				}
				break;
			}
			break;
		}
	}
}
