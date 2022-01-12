namespace CardGame.Common
{
    public class Card
    {
        public CardType CardType;
        public int Value;

        public Card(CardType type, int value)
        {
            CardType = type;
            Value = value;
        }
    }

    public enum CardType
    {
        Invalid = 0,
        Diamonds = 1,
        Hearts = 2,
        Spades = 3,
        Clubs = 4
    }

    public enum PlayerStrategy
    {
        Invalid = 0,
        MatchPrizeCard = 1,
        RandomCard = 2,
    }

    public class PlayerCard
    {
        public Player Player;
        public Card Card;

        public PlayerCard(Player player, Card card)
        {
            Player = player;
            Card = card;
        }
    }

    public class Player
    {
        public string Name = "";
        public PlayerStrategy Strategy = PlayerStrategy.Invalid;
        public CardType CardType = CardType.Invalid;
        public List<Card> CardsAvailable = new List<Card>();
        public List<Card> CardsUsed = new List<Card>();
        public List<Card> CardsWon = new List<Card>();

        public int Score
        {
            get
            {
                int score = 0;
                foreach(Card card in CardsWon)
                {
                    score += card.Value;
                }
                return score;
            }
        }

        public Card GetBestCard(int prizeCardValue)
        {
            if(this.Strategy == PlayerStrategy.MatchPrizeCard)
            {
                // Find a card that matches the prize card value
                List<Card> cards = CardsAvailable.Where(x => x.Value == prizeCardValue).ToList();

                if(cards[0] == null)
                {
                    throw new Exception("Unexpected error: A card with a matching value could not be found.");
                }
                else
                {
                    CardsAvailable.Remove(cards[0]);
                    CardsUsed.Add(cards[0]);
                    return cards[0];
                }
            }
            else if(this.Strategy == PlayerStrategy.RandomCard)
            {
                // Shuffle the available cards, we'll pick the first one after they've been shuffled
                List<Card> cards = CardsAvailable.OrderBy (x => new Random().Next()).ToList();

                if(cards[0] == null)
                {
                    throw new Exception("Unexpected error: Couldn't select another card.");
                }
                else
                {
                    // Remove the selected card from the list of available cards, and then add it to the list of used cards
                    // (So that we don't use the same card twice!)
                    CardsAvailable.Remove(cards[0]);
                    CardsUsed.Add(cards[0]);
                    return cards[0];
                }
            }
            else
            {
                throw new Exception("Unexpected error: The player strategy " + this.Strategy.ToString() + " is not supported.");
            }
        }

        public Player(string name, List<Card> cardsAvailable, PlayerStrategy strategy, CardType cardType)
        {
            Name = name;
            CardType = cardType;
            Strategy = strategy;
            CardsAvailable = cardsAvailable.Where(x => x.CardType == cardType).ToList();
        }
    }
}