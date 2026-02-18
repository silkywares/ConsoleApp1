namespace PokerGame;

public class Evaluator
{
    static string[] rankNames = { "High Card", "One Pair", "Two Pair", "Three of a Kind", "Straight",
                            "Flush", "Full House", "Four of a Kind", "Straight Flush", "Royal Flush" };

    public enum HandRank { HighCard, OnePair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush }

    /*
    High Card → kicker(s) matter (top 4 remaining cards)

    One Pair → top 3 remaining cards are kickers

    Two Pair → 1 kicker (fifth card outside the two pairs)

    Three of a Kind → 2 kickers (highest remaining cards)

    Straight → kicker usually not needed (high card of straight determines it)

    Flush → top 5 suited cards matter (primary = highest, secondary = next highest, etc.)

    !Full House → no kicker (trips + pair fully determine hand)

    !Four of a Kind → 1 kicker (fifth card outside quads)

    Straight Flush / Royal Flush → kicker irrelevant
    */
    public class PlayerResult
    {
        public Player Player { get; set; }
        public HandEvaluation Evaluation { get; set; }
        public PlayerResult(Player player)
        {
            Player = player;
            Evaluation = new HandEvaluation();
        }
    }
    public class HandEvaluation
    {
        public int Rank { get; set; }      // e.g. StraightFlush, FullHouse, etc.
        public int PrimaryValue { get; set; }   // e.g. rank of the trips in a full house
        public int SecondaryValue { get; set; } // e.g. rank of the pair in a full house
        public int[] Kicker { get; set; } = Array.Empty<int>();  // remaining high cards sorted
    }
    public static List<PlayerResult> EvaluateBoard(List<Player> players, List<Card> board)
    {
        var results = new List<PlayerResult>();

        foreach (var p in players)
        {
            var evaluation = EvaluatePlayer(p, board);
            results.Add(new PlayerResult(p) { Evaluation = evaluation });
        }
        return results;
    }
    public static HandEvaluation EvaluatePlayer(Player p, List<Card> board)
    {
        var cards = p.Hand.Concat(board)
                            .OrderByDescending(c => c.Rank)
                            .ToList();

        var rankGroups = cards.GroupBy(c => c.Rank)
                            .OrderByDescending(g => g.Count())
                            .ThenByDescending(g => g.Key)
                            .ToList();

        var suitGroups = cards.GroupBy(c => c.Suit)
                            .ToList();
        var kicker = Array.Empty<int>();


        if (IsStraightFlush(suitGroups, out var sfHigh, out kicker))
            return new HandEvaluation { Rank = (int)HandRank.StraightFlush, PrimaryValue = sfHigh };

        if (IsFourOfAKind(rankGroups, out var quad, out kicker))
            return new HandEvaluation { Rank = (int)HandRank.FourOfAKind, PrimaryValue = quad, Kicker = kicker };

        if (IsFullHouse(rankGroups, out var trips, out var pair, out kicker))
            return new HandEvaluation { Rank = (int)HandRank.FullHouse, PrimaryValue = trips, SecondaryValue = pair };

        if (IsFlush(suitGroups, out var flushHigh))
            return new HandEvaluation { Rank = (int)HandRank.Flush, PrimaryValue = flushHigh };

        if (IsStraight(cards, out var straightHigh, out kicker))
            return new HandEvaluation { Rank = (int)HandRank.Straight, PrimaryValue = straightHigh };

        if (IsThreeOfAKind(cards, out var tripsRank, out var secondaryValue, out kicker))
            return new HandEvaluation { Rank = (int)HandRank.ThreeOfAKind, PrimaryValue = tripsRank, SecondaryValue = secondaryValue };

        if (IsTwoPair(cards, out var highPair, out var lowPair, out kicker))
            return new HandEvaluation { Rank = (int)HandRank.TwoPair, PrimaryValue = highPair, SecondaryValue = lowPair };

        if (IsOnePair(rankGroups, out var pairRank, out kicker))
            return new HandEvaluation { Rank = (int)HandRank.OnePair, PrimaryValue = pairRank };

        HighCard(cards, out int highCard, out secondaryValue, out kicker);
        return new HandEvaluation
        {
            Rank = (int)HandRank.HighCard,
            PrimaryValue = highCard,
            SecondaryValue = secondaryValue,
            Kicker = kicker
        };
    }
    public static List<PlayerResult> EvaluateWinner(List<PlayerResult> results)
    {
        var topRank = results.Max(r => r.Evaluation.Rank);

        // Filter players with the top rank
        var topPlayers = results
            .Where(r => r.Evaluation.Rank == topRank)
            .ToList();

        // If multiple, compare primary, secondary, kicker
        var maxPrimary = topPlayers.Max(r => r.Evaluation.PrimaryValue);
        topPlayers = topPlayers.Where(r => r.Evaluation.PrimaryValue == maxPrimary).ToList();

        var maxSecondary = topPlayers.Max(r => r.Evaluation.SecondaryValue);
        topPlayers = topPlayers.Where(r => r.Evaluation.SecondaryValue == maxSecondary).ToList();

        var kickerComparer = Comparer<int[]>.Create(CompareKickers);
        var maxKicker = topPlayers.MaxBy(p => p.Evaluation.Kicker, kickerComparer).Evaluation.Kicker;
        topPlayers = topPlayers.Where(p => kickerComparer.Compare(p.Evaluation.Kicker, maxKicker) == 0).ToList();

        return topPlayers;
    }
    private static int CompareKickers(int[] a, int[] b)
    {
        if (a == b) return 0;
        if (a is null) return -1;
        if (b is null) return 1;

        int minLen = Math.Min(a.Length, b.Length);

        for (int i = 0; i < minLen; i++)
        {
            int cmp = a[i].CompareTo(b[i]);
            if (cmp != 0)
                return cmp;
        }

        return a.Length.CompareTo(b.Length);
    }
    private static bool IsFourOfAKind(List<IGrouping<Rank, Card>> rankGroups, out int quadRank, out int[] kicker)
    {
        quadRank = 0;
        kicker = Array.Empty<int>();
        var quad = rankGroups.FirstOrDefault(g => g.Count() == 4);
        if (quad == null)
            return false;

        quadRank = (int)quad.Key;

        // @TODO inspect this for accuracy
        kicker = rankGroups
            .Where(g => g.Count() != 4)
            .SelectMany(g => g.Select(c => (int)c.Rank))
            .ToArray();

        return true;
    }
    private static bool IsStraightFlush(List<IGrouping<Suit, Card>> suitGroups, out int high, out int[] kicker)
    {
        high = 0;
        kicker = Array.Empty<int>();
        foreach (var group in suitGroups)
            if (IsStraight(group.ToList(), out high, out kicker))
                return true;
        return false;
    }
    private static bool IsStraight(List<Card> cards, out int high, out int[] kicker)
    {

        var ranks = cards.Select(c => (int)c.Rank).Distinct().OrderBy(r => r).ToList();
        kicker = Array.Empty<int>();
        high = 0;

        if (ranks.Contains(14))
            ranks.Insert(0, 1);// add a 1 if Ace 

        for (int i = 0; i <= ranks.Count - 5; i++)
        {
            if (ranks[i + 4] - ranks[i] == 4)
            {
                high = ranks[i + 4];
                var straightSet = ranks.GetRange(i, 5).ToHashSet();
                kicker = ranks.Where(r => !straightSet.Contains(r)).DefaultIfEmpty(0).ToArray();
                return true;
            }
        }
        return false;
    }
    private static bool IsFullHouse(List<IGrouping<Rank, Card>> rankGroups, out int trips, out int pair, out int[] kicker)
    {
        trips = 0;
        pair = 0;
        kicker = Array.Empty<int>();
        var tripRank = rankGroups.FirstOrDefault(g => g.Count() == 3);
        var pairRank = rankGroups.FirstOrDefault(g => g.Count() == 2);
        kicker = rankGroups
            .Where(g => g != tripRank && g != pairRank)
            .SelectMany(g => g.Select(c => (int)c.Rank))
            .ToArray();
        if (tripRank != null && pairRank != null)
        {
            trips = (int)tripRank.Key;
            pair = (int)pairRank.Key;
            return true;
        }
        return false;
    }
    private static bool IsFlush(List<IGrouping<Suit, Card>> suitGroups, out int high)
    {
        high = 0;
        var largestGroup = suitGroups.OrderByDescending(g => g.Count()).FirstOrDefault()!;
        if (largestGroup.Count() >= 5)
        {
            high = largestGroup.Max(c => (int)c.Rank);

            return true;
        }
        return false;
    }
    private static bool IsThreeOfAKind(List<Card> cards, out int tripRank, out int secondary, out int[] kicker)
    {
        tripRank = 0;
        secondary = 0;
        kicker = Array.Empty<int>();

        var rankGroups = cards.GroupBy(c => c.Rank)
                            .OrderByDescending(g => g.Count())
                            .ThenByDescending(g => g.Key)
                            .ToList();

        var trips = rankGroups.FirstOrDefault(g => g.Count() == 3);
        if (trips == null)
            return false;

        tripRank = (int)trips.Key;

        // Assign out variable to local to use in lambda
        var tripRankLocal = tripRank;

        // Remaining cards outside the trips
        var remainingRanks = cards
            .Where(c => (int)c.Rank != tripRankLocal)
            .Select(c => (int)c.Rank)
            .OrderByDescending(r => r)
            .ToList();

        if (remainingRanks.Count >= 1) secondary = remainingRanks[0];
        if (remainingRanks.Count >= 2) kicker = remainingRanks.Skip(1).ToArray();

        return true;
    }
    private static bool IsTwoPair(List<Card> cards, out int highPair, out int lowPair, out int[] kicker)
    {
        highPair = 0;
        lowPair = 0;
        kicker = Array.Empty<int>();

        var rankGroups = cards.GroupBy(c => c.Rank)
                            .OrderByDescending(g => g.Count())
                            .ThenByDescending(g => g.Key)
                            .ToList();

        var pairs = rankGroups.Where(g => g.Count() == 2)
                            .Select(g => (int)g.Key)
                            .OrderByDescending(r => r)
                            .ToList();

        if (pairs.Count >= 2)
        {
            highPair = pairs[0];
            lowPair = pairs[1];

            var usedRanks = new HashSet<int> { highPair, lowPair };

            // Kicker = highest card not in the two pairs
            kicker = cards
                .Select(c => (int)c.Rank)
                .Where(r => !usedRanks.Contains(r))
                .DefaultIfEmpty(0)
                .ToArray();

            return true;
        }

        return false;
    }
    private static bool IsOnePair(List<IGrouping<Rank, Card>> rankGroups, out int pairRank, out int[] kicker)
    {
        kicker = Array.Empty<int>();
        pairRank = 0;
        var pair = rankGroups.FirstOrDefault(g => g.Count() == 2);
        if (pair == null)
            return false;

        pairRank = (int)pair.Key;

        kicker = rankGroups
            .Where(g => g.Count() != 2)
            .SelectMany(g => g.Select(c => (int)c.Rank))
            .ToArray();
        return true;
    }
    private static bool HighCard(List<Card> cards, out int highCard, out int secondary, out int[] kicker)
    {
        highCard = (int)cards[0].Rank;
        secondary = (int)cards[1].Rank;
        kicker = cards.Count >= 3 ? cards.Skip(2).Select(c => (int)c.Rank).ToArray() : Array.Empty<int>();
        return true;
    }
}
