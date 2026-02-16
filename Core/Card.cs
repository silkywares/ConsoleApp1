namespace PokerGame;
public class Card
{
    public Suit Suit {get;}
    public Rank Rank {get;}
    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }
    public override string ToString()
    {
        string suitSymbol = Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => "?"
        };

        string RankSymbol = Rank switch
        {
            Rank.Two   => "2",
            Rank.Three => "3",
            Rank.Four  => "4",
            Rank.Five  => "5",
            Rank.Six   => "6",
            Rank.Seven => "7",
            Rank.Eight => "8",
            Rank.Nine  => "9",
            Rank.Ten   => "T",
            Rank.Jack  => "J",
            Rank.Queen => "Q",
            Rank.King  => "K",
            Rank.Ace   => "A",
            _ => "?"
        };

        return $"[{RankSymbol}{suitSymbol}]";
    }
    public void PrintCard()
    {
        Console.ForegroundColor =
            Suit == Suit.Hearts || Suit == Suit.Diamonds
            ? ConsoleColor.Red
            : ConsoleColor.White;

        Console.Write(ToString());
        Console.ResetColor();
    }
}
