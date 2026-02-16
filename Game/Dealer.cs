namespace PokerGame;

public class Dealer
{
    private Deck Deck;//change back to private
    public List<Player> Players;
    public List<Card> Board { get; private set; }
    public Dealer(List<Player> players)
    {
        Deck = NewDeck();
        Players = players;
        Board = new List<Card>();
    }
    private Deck NewDeck()
    {
        Deck deck = new Deck();
        deck.Shuffle();
        return deck;
    }
    public void DealPlayerCards()
    {
        for(int i=0; i<2; i++)
        {
            foreach(Player player in Players)
            {
            player.AddCard(Deck.Deal());
            }
        }
    }
    public void DealBoardCards()
    {
        if(Board.Count == 0)
        {
           Board.Add(Deck.Deal());
           Board.Add(Deck.Deal());
           Board.Add(Deck.Deal());
        }
        else if(Board.Count == 3)
            Board.Add(Deck.Deal());
        else if(Board.Count == 4)
            Board.Add(Deck.Deal());    
        else
            Console.Write("Board full!");
    }
    public void ClearBoard()
    {
        Board.Clear();
        foreach (Player p in Players)
            p.Hand.Clear();
        Deck = NewDeck();
    }
    public void TestBoard()
    {
        Board.Clear();
        Board.Add(new Card(Rank.Four, Suit.Diamonds));
        Board.Add(new Card(Rank.Four, Suit.Diamonds));
        Board.Add(new Card(Rank.Four, Suit.Diamonds));
        Board.Add(new Card(Rank.Four, Suit.Diamonds));
        Board.Add(new Card(Rank.Two, Suit.Diamonds));
    }
    public void TestHand()
    {
        Players[0].Hand.Clear();
        Players[0].Hand.Add(new Card(Rank.Eight, Suit.Diamonds));
        Players[0].Hand.Add(new Card(Rank.Two, Suit.Diamonds));
    }
    public void TestOneMillion()
    {
       int[] handRankCounts = new int[10]; 

        int simulations = 1_000_000;

        for (int i = 0; i < simulations; i++)
        {
            
            DealPlayerCards();
            DealBoardCards();
            DealBoardCards();
            DealBoardCards();

            foreach (var player in Players)
            {
                var eval = Evaluator.EvaluatePlayer(player, Board);

                // check for Royal Flush
                if (eval.Rank == 8 && eval.PrimaryValue == 14) // Ace-high straight flush
                {
                    handRankCounts[9]++; // Royal Flush
                }
                else
                {
                    handRankCounts[eval.Rank]++;
                }
            }

            ClearBoard();
        }

        // Print results
        Console.WriteLine("Simulation results after 1,000,000 deals:");
        string[] rankNames = { "High Card", "One Pair", "Two Pair", "Three of a Kind", "Straight",
                            "Flush", "Full House", "Four of a Kind", "Straight Flush", "Royal Flush" };

        for (int rank = 0; rank < handRankCounts.Length; rank++)
        {
            Console.WriteLine($"{rankNames[rank]}: {handRankCounts[rank]}");
        }
 
    }
}
