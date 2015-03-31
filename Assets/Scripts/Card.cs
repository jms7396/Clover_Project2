using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum CardType 
{
	fire,
	water,
	earth,
	metal
}

public class Card{

	public CardType type;
	public string cardName;
	public int atk;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Card(string typ, string nme, string pow)
	{
		switch(typ)
		{
		case "fire":
			type = CardType.fire;
			break;
		case "water":
			type = CardType.water;
			break;
		case "earth":
			type = CardType.earth;
			break;
		case "metal":
			type = CardType.metal;
			break;
		}
		cardName = nme;
		atk = int.Parse (pow);
	}


	public override string ToString()
	{
		return string.Format("{0} ({1}) {2})",
		cardName, type, atk);
	}

	/*
	public static Card ReadFromCSV(string typ)
	{
		switch (typ) {
		case "attack":
			Card.type = CardType.attack;
	*/

	public CardType ReturnType()
	{
		return type;
	}

	public void AffectPlayer(Player target)
	{
		// Deal damage to the other player
		// target.takeDamage(atk);
		target.TakeDamage(this.atk);

		// Use effect on either user or target
		// if(effect1 effects user){user.applyEffect(effect1}
		// if(effect2 effects target){target.applyEffect(effect1}
		// repeat for effect 2
	}
	// for the moment let's call this the theoretical Affect Player method.
}
