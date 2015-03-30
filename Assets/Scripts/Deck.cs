﻿using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck {

	public List<Card> cards = new List<Card>();
	private static int numCards;
	private int cardsDrawn;

	// Use this for initialization
	public Deck () 
	{
		numCards = 30;
		cardsDrawn = 0;
		GenerateDeck ();
		Shuffle();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void Shuffle()
	{
		for (int i = 0; i < cards.Count; i++) {
			Card temp = cards[i];
			int randomIndex = Random.Range(i, cards.Count);
			cards[i] = cards[randomIndex];
			cards[randomIndex] = temp;
		}
	}

	public Card Draw()
	{
		Card drawnCard = cards[0];
		cards.RemoveAt (0);
		cardsDrawn++;
		cards.Add(drawnCard);
		if(cardsDrawn == 30)
		{
			Shuffle();
		}
		return drawnCard;
	}

	private void GenerateDeck()
	{

		string[] cardInfo = ReadCards ();

		for(int i = 0; i < numCards; i++)
		{
			string[] info = cardInfo[i].Split(',');
			cards.Add(new Card(info[0], info[1], info[2]));
		}
	}

	private string[] ReadCards()
	{
		Debug.Log (Application.dataPath);
		var sr = new StreamReader (Application.dataPath + "/Scripts/TextAsset/CardList.txt");
		var fileContents = sr.ReadToEnd ();
		sr.Close ();

		string [] cardContents = fileContents.Split ("\n"[0]);
		return cardContents;
	}
}
