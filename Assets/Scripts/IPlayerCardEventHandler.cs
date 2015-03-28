using UnityEngine.EventSystems;

interface IPlayerCardEventHandler : IEventSystemHandler
{
	void TookDamage(Player player, int amount);

	void CardDrawn(Player player, Card card);

	void CardChosen(Player player, Card card);
	
	void CardChosen(UICard card);

	void CardPlayed(Player player, Card card);
}