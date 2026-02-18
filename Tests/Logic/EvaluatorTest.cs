using PokerGame;
using Xunit;

namespace Tests;

public class EvaluatorTest
{
    [Fact]
    public void EvaluatePlayer_WithStraightFlush_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Two, Suit.Hearts));
        testPlayer.AddCard(new Card(Rank.Three, Suit.Hearts));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Four, Suit.Hearts),
            new Card(Rank.Five, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts),
            new Card(Rank.Seven, Suit.Diamonds),
            new Card(Rank.Eight, Suit.Clubs)
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.StraightFlush, (Evaluator.HandRank)evaluation.Rank);
    }

    [Fact]
    public void EvaluatePlayer_WithFourOfAKind_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Two, Suit.Hearts));
        testPlayer.AddCard(new Card(Rank.Two, Suit.Diamonds));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Two, Suit.Clubs),
            new Card(Rank.Two, Suit.Spades),
            new Card(Rank.Six, Suit.Hearts),
            new Card(Rank.Seven, Suit.Diamonds),
            new Card(Rank.Eight, Suit.Clubs)
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.FourOfAKind, (Evaluator.HandRank)evaluation.Rank);
    }

    [Fact]
    public void EvaluatePlayer_WithFullHouse_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Two, Suit.Hearts));
        testPlayer.AddCard(new Card(Rank.Two, Suit.Diamonds));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Two, Suit.Clubs),
            new Card(Rank.Three, Suit.Spades),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Seven, Suit.Diamonds),
            new Card(Rank.Eight, Suit.Clubs)
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.FullHouse, (Evaluator.HandRank)evaluation.Rank);
    }

    [Fact]
    public void EvaluatePlayer_WithFlush_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Two, Suit.Hearts));
        testPlayer.AddCard(new Card(Rank.Five, Suit.Hearts));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Nine, Suit.Hearts),
            new Card(Rank.King, Suit.Hearts),
            new Card(Rank.Three, Suit.Diamonds),
            new Card(Rank.Jack, Suit.Clubs)
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.Flush, (Evaluator.HandRank)evaluation.Rank);
    }

    [Fact]
    public void EvaluatePlayer_WithStraight_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Two, Suit.Diamonds));
        testPlayer.AddCard(new Card(Rank.Three, Suit.Hearts));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Four, Suit.Hearts),
            new Card(Rank.Five, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts),
            new Card(Rank.Seven, Suit.Diamonds),
            new Card(Rank.Eight, Suit.Clubs)
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.Straight, (Evaluator.HandRank)evaluation.Rank);
    }

    [Fact]
    public void EvaluatePlayer_WithThreeOfAKind_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Two, Suit.Diamonds));
        testPlayer.AddCard(new Card(Rank.Two, Suit.Hearts));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Two, Suit.Clubs),
            new Card(Rank.Four, Suit.Hearts),
            new Card(Rank.Five, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts),
            new Card(Rank.Seven, Suit.Diamonds),
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.ThreeOfAKind, (Evaluator.HandRank)evaluation.Rank);
    }

    [Fact]
    public void EvaluatePlayer_WithTwoPair_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Two, Suit.Diamonds));
        testPlayer.AddCard(new Card(Rank.Two, Suit.Hearts));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Four, Suit.Hearts),
            new Card(Rank.Four, Suit.Spades),
            new Card(Rank.Six, Suit.Hearts),
            new Card(Rank.Seven, Suit.Diamonds),
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.TwoPair, (Evaluator.HandRank)evaluation.Rank);
    }

    [Fact]
    public void EvaluatePlayer_WithOnePair_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Two, Suit.Diamonds));
        testPlayer.AddCard(new Card(Rank.Two, Suit.Hearts));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Four, Suit.Hearts),
            new Card(Rank.Five, Suit.Spades),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Eight, Suit.Diamonds),
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.OnePair, (Evaluator.HandRank)evaluation.Rank);
    }

    [Fact]
    public void EvaluatePlayer_WithHighCard_ReturnsCorrectRank()
    {
        Player testPlayer = new Player("TestPlayer", 0);
        testPlayer.AddCard(new Card(Rank.Ace, Suit.Diamonds));
        testPlayer.AddCard(new Card(Rank.King, Suit.Hearts));
        List<Card> communityCards = new List<Card> {
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Four, Suit.Hearts),
            new Card(Rank.Five, Suit.Spades),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Eight, Suit.Diamonds),
        };

        var evaluation = Evaluator.EvaluatePlayer(testPlayer, communityCards);

        Assert.Equal(Evaluator.HandRank.HighCard, (Evaluator.HandRank)evaluation.Rank);
    }
}
