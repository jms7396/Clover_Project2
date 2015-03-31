using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public enum GameState
{
	None,
	Setup,
	PlayersChoosing,
	PlayersLocked
};

#region Events

[System.Serializable]
public class GameStateChanged : UnityEvent<Controller2, GameState, GameState> { }

#endregion

public class Controller2 : MonoBehaviour {

	public Player p1;
	public Player p2;
	public GameObject GameOverPanel;
	public GameObject GameOverText;

	public BattleLogger log;

	private GameState state;
	public GameState State
	{
		get { return state; }
		set
		{
			var old = state;
			state = value;
			onGameStateChange.Invoke(this, old, state);
		}
	}

	public GameStateChanged onGameStateChange;

	void Awake()
	{
	}

	void Start()
	{
		SetupGame();
	}

	void OnEnable()
	{
		p1.stateChange.AddListener(OnPlayerStateChange);
		p2.stateChange.AddListener(OnPlayerStateChange);
		this.onGameStateChange.AddListener(OnGameStateChange);
	}

	void OnDisable()
	{
		p1.stateChange.RemoveListener(OnPlayerStateChange);
		p2.stateChange.RemoveListener(OnPlayerStateChange);
		this.onGameStateChange.RemoveListener(OnGameStateChange);
	}

	void Update() { }

	public void SetupAndStartGame()
	{
		StartGame();
	}

	public void SetupGame()
	{
		p1.deck = new Deck();
		p2.deck = new Deck();
		p1.State = PlayerState.Setup;
		p2.State = PlayerState.Setup;
		State = GameState.Setup;
	}

	public void StartGame()
	{
		p1.drawTillFull();
		p2.drawTillFull();
		p1.State = PlayerState.Deciding;
		p2.State = PlayerState.Deciding;
		State = GameState.PlayersChoosing;
	}

	public void OnGameStateChange(Controller2 controller, GameState oldState, GameState newState)
	{
		Utils.assert(() => this == controller);

		Debug.Log("Controller: " + oldState + " -> " + newState);
	}

	public void OnPlayerStateChange(Player player, PlayerState oldState, PlayerState newState)
	{
		if (p1.HasChosen && p2.HasChosen)
		{
			State = GameState.PlayersLocked;
			EvaluateBattle();
		}
		if (p1.IsDead) 
		{
			Reset ();
			Text ttext = GameOverText.GetComponent<Text>();
			ttext.text = "Player 2 Wins!!";
			GameOverPanel.SetActive(true);
		}
		if (p2.IsDead) 
		{
			Reset ();
			Text ttext = GameOverText.GetComponent<Text>();
			ttext.text = "Player 1 Wins!!";
			GameOverPanel.SetActive(true);
		}
	}

	public void EvaluateBattle()
	{
		CardChecks(p1, p2, p1.CardChoice, p2.CardChoice);
		state = GameState.PlayersChoosing;

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

	public void Reset()
	{
		p1.Health = 20;
		p2.Health = 20;
		p1.deck.Shuffle ();
		p2.deck.Shuffle ();
		//p1.State = PlayerState.None;
		//p2.State = PlayerState.None;

	}
}
