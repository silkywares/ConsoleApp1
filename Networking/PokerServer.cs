namespace PokerGame;

public class GameStateDTO
{
    public List<PlayerDTO> Players { get; set; }
    public List<CardDTO> Board { get; set; }
    public int Pot { get; set; }
    public string CurrentPlayerId { get; set; }
}
public class PlayerDTO
    {
      public string Id { get; set; }
    public string Name { get; set; }
    public int ChipCount { get; set; }
    public int CurrentBet { get; set; }
    public List<CardDTO>? Hand { get; set; } // ONLY for that player
    }
public class CardDTO
{
    public int Rank { get; set; }
    public int Suit { get; set; }
}

public GameStateDTO BuildGameStateForPlayer(Player requestingPlayer)
{
    return new GameStateDTO
    {
        Pot = Pot,
        Board = Board.Select(c => new CardDTO(c)).ToList(),

        Players = Players.Select(p => new PlayerDTO
        {
            Id = p.Id,
            Name = p.Name,
            ChipCount = p.ChipCount,
            CurrentBet = p.CurrentBet,

            // ONLY show hand to owner
            Hand = p == requestingPlayer
                ? p.Hand.Select(c => new CardDTO(c)).ToList()
                : null

        }).ToList()
    };
}
