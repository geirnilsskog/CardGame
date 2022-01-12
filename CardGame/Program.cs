using CardGame.Common;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayCardGame();
        }

        private static void PlayCardGame()
        {
            // Set up a card deck with cards minimum value 2 and maximum value 9
            Console.WriteLine("Setting up card deck...");           
            List<Card> cards = SetUpCardDeck(2, 9);          
            Console.WriteLine("Card deck created, the deck contains a total of " + cards.Count() + " cards");

            List<Player> players = new List<Player>();

            // Set up 2 players with different names, strategies and card types
            players.Add(new Player("Player 1", cards, PlayerStrategy.MatchPrizeCard, CardType.Clubs));
            players.Add(new Player("Player 2", cards, PlayerStrategy.RandomCard, CardType.Diamonds));

            // Set up the prize cards and shuffle these cards
            List<Card> prizeCards = ShuffleCards(cards.Where(x => x.CardType == CardType.Spades).ToList());

            int roundNumber = 1;

            while(prizeCards.Count > 0)
            {
                // Pick the next prize card and remove it so that it will only be used this one time
                Card prizeCard  = prizeCards[0];
                prizeCards.Remove(prizeCard);

                Console.WriteLine("");
                Console.WriteLine("------------------------------------------------------------------------------------------");
                Console.WriteLine("ROUND# " + roundNumber.ToString());
                Console.WriteLine("");
                Console.WriteLine("Prize card drawn: " + prizeCard.CardType.ToString() + " " + prizeCard.Value.ToString());

                List<Player> winners = PickWinners(players, prizeCard);

                
                if(winners.Count == 1)
                {
                    Console.WriteLine("");
                    Console.WriteLine("ROUND WINNER: " + winners[0].Name + " (+" + prizeCard.Value + ")");    
                }
                else
                {
                    Console.WriteLine("");
                    string output = "NO ROUND WINNER, EVEN SCORE: ";

                    for(int i=0;i<winners.Count;i++)
                    {
                        if(i > 0) output += " and ";
                        output += winners[0].Name;
                    }

                    output += " (+" + prizeCard.Value + ")";
                    Console.WriteLine(output);
                }

                foreach(Player winner in winners)
                {
                    winner.CardsWon.Add(prizeCard);
                }

                WriteGameStatusToConsole(players, prizeCards);
                roundNumber++;
            }

            WriteGameFinalScoreToConsole(players);
        }

        private static List<Player> PickWinners(List<Player> players, Card prizeCard)
        {
            List<Player> winners = new List<Player>();

            List<PlayerCard> playerCards = new List<PlayerCard>();

            foreach(Player player in players)
            {
                Card card = player.GetBestCard(prizeCard.Value);
                PlayerCard playerCard = new (player, card);
                playerCards.Add(playerCard);

                Console.WriteLine(playerCard.Player.Name + " used " + playerCard.Card.CardType.ToString() + " " + playerCard.Card.Value.ToString());
            }

            // Find the highest card value
            int highestValue = 0;

            foreach(PlayerCard playerCard in playerCards)
            {
                if(playerCard.Card.Value > highestValue) highestValue = playerCard.Card.Value;
            }

            // Players that used a card with highest value will be the winners
            foreach(PlayerCard playerCard in playerCards)
            {
                if(playerCard.Card.Value == highestValue) winners.Add(playerCard.Player);                
            }

            return winners;
        }

        private static void WriteGameStatusToConsole(List<Player> players, List<Card> prizeCards)
        {
            Console.WriteLine("");
            Console.WriteLine("SCOREBOARD (" + prizeCards.Count + " card(s) remaining)");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine(String.Format("| {0,-15} | {1,-15} | {2,-15} |", "Player", "Card Type", "Score"));
            Console.WriteLine("-------------------------------------------------------");
           
            int highestScore = 0;

            foreach(Player player in players)
            {
                if(player.Score > highestScore) highestScore = player.Score;

                Console.WriteLine(String.Format("| {0,-15} | {1,-15} | {2,-15} |", player.Name, player.CardType.ToString(), player.Score.ToString()));
            }

            Console.WriteLine("-------------------------------------------------------");
        }

        private static void WriteGameFinalScoreToConsole(List<Player> players)
        {
            List<Player> winners = new List<Player>();

            Console.WriteLine("");

            // Find the highest card value
            int highestScore = 0;

            foreach(Player player in players)
            {
                if(player.Score > highestScore) highestScore = player.Score;
            }

            foreach(Player player in players)
            {
                if(player.Score == highestScore) winners.Add(player);
            }

            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------------------------------------------");

            if(winners.Count == 1)
            {                   
                Console.WriteLine("GAME WINNER: " + winners[0].Name);    
            }
            else
            {
                string output = "NO WINNER - EVEN SCORES: ";

                for(int i=0;i<winners.Count;i++)
                {
                    if(i > 0) output += " and ";
                    output += winners[i].Name;
                }

                Console.WriteLine(output);   
            }
        }

        private static List<Card> SetUpCardDeck(int minValue, int maxValue)
        {
            List<Card> cards = new List<Card>();

            for(int i=1;i<5;i++) // CardType 1 to 4
            {
                for(int j=minValue;j<maxValue+1;j++) // We're only going to use cards with values 2 to 9
                {
                    CardType type = (CardType)Enum.Parse(typeof(CardType), i.ToString());
                    Card card = new Card(type, j);
                    cards.Add(card);
                }
            }

            return cards;
        }

        private static List<Card> ShuffleCards(List<Card> cards)
        {
            Random random = new Random();
            List<Card> shuffledCards = cards.OrderBy (x => random.Next()).ToList();
            return shuffledCards;
        }
    }    
}
