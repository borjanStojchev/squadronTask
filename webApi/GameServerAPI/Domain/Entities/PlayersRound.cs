using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class PlayerRound
{
    [Key]
    public int Id { get; set; }
    public Guid PlayerId { get; set; }
    public int RoundId { get; set; }
    public string? Answer { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateExpressionAnswered { get; set; }

    [ForeignKey("RoundId")]
    public GameRound Round { get; set; } = null!;
    [ForeignKey("PlayerId")]
    public Player Player { get; set; } = null!;
}
