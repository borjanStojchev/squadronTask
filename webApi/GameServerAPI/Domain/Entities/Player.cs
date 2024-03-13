using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Player 
{
    [Key]
    public Guid Id { get; set; }
    public int Points { get; set; }
    public string? ConnectionId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DateCreated { get; set; }
    public virtual ICollection<GameRound> GameRounds { get; set; } = new List<GameRound>();
    public virtual ICollection<PlayerRound> PlayerRounds { get; set; } = new List<PlayerRound>();
}
