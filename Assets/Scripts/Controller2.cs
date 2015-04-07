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
    public GameObject image;
    public GameObject image2;
    public GameObject damage;
    public GameObject damage2;

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
		SetupGame();
	}

	void Start()
	{
        StartGame();
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

	public void SetupGame()
	{
		p1.deck = new Deck();
		p2.deck = new Deck();
		p1.State = PlayerState.Setup;
		p2.State = PlayerState.Setup;
		State = GameState.Setup;
        image = GameObject.Find("Player1Atk");
        image2 = GameObject.Find("Player2Atk");
        damage = GameObject.Find("Damages1");
        damage2= GameObject.Find("Damages2");
        image.SetActive(false);
        image2.SetActive(false);
        //damage.SetActive(false);
        //damage2.SetActive(false);
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
            image.SetActive(false);
            image2.SetActive(false);
            damage.SetActive(false);
            damage2.SetActive(false);
		}
		if (p2.IsDead) 
		{
			Reset ();
			Text ttext = GameOverText.GetComponent<Text>();
			ttext.text = "Player 1 Wins!!";
			GameOverPanel.SetActive(true);
            image.SetActive(false);
            image2.SetActive(false);
            damage.SetActive(false);
            damage2.SetActive(false);
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
                    StartCoroutine(AnimatePlay1());
					p1Card.atk -= p2Card.atk;
					p1Card.AffectPlayer(p2);
					p1Card.atk += p2Card.atk;
				}
				else if (p1Card.atk < p2Card.atk)
				{
                    StartCoroutine(AnimatePlay2());
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
                StartCoroutine(AnimatePlay2());
				p2Card.AffectPlayer(p1);
				break;
			case CardType.earth:
                StartCoroutine(AnimatePlay1());
				p1Card.AffectPlayer(p2);
				break;
			}
			break;
		case CardType.water:
			switch(p2Card.type)
			{
			case CardType.fire:
                StartCoroutine(AnimatePlay1());
				p1Card.AffectPlayer(p2);
				break;
			case CardType.water:
				if(p1Card.atk > p2Card.atk)
				{
                    StartCoroutine(AnimatePlay1());
					p1Card.atk -= p2Card.atk;
					p1Card.AffectPlayer(p2);
					p1Card.atk += p2Card.atk;
				}
				else if (p1Card.atk < p2Card.atk)
				{
                    StartCoroutine(AnimatePlay2());
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
                StartCoroutine(AnimatePlay2());
				p2Card.AffectPlayer(p1);
				break;
			}
			break;
		case CardType.earth:
			switch(p2Card.type)
			{
			case CardType.fire:
                StartCoroutine(AnimatePlay2());
				p2Card.AffectPlayer(p1);
				break;
			case CardType.water:
                StartCoroutine(AnimatePlay1());
				p1Card.AffectPlayer(p2);
				break;
			case CardType.earth:
				if(p1Card.atk > p2Card.atk)
				{
                    StartCoroutine(AnimatePlay1());
					p1Card.atk -= p2Card.atk;
					p1Card.AffectPlayer(p2);
					p1Card.atk += p2Card.atk;
				}
				else if (p1Card.atk < p2Card.atk)
				{
                    StartCoroutine(AnimatePlay2());
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

    IEnumerator AnimatePlay1()
    {
        //damage2.SetActive(true);
        image.SetActive(true);
        image.animation.Play();
        //damage2.animation.Play();
        yield return new WaitForSeconds(1.5f);
        image.SetActive(false);
        //damage2.SetActive(false);
    }

    IEnumerator AnimatePlay2()
    {
        //damage.SetActive(true);
        image2.SetActive(true);
        image2.animation.Play();
        //damage.animation.Play();
        yield return new WaitForSeconds(1.5f);
        image2.SetActive(false);
        //damage.SetActive(false);
    }
}
