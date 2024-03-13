using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class GameRound
{
    [Key]
    public int Id { get; set; }
    public string Expression { get; set; } = null!;
    public string CorrectAnswer { get; set; } = null!;
    public Guid? AnsweredByPlayerId { get; set; }
    
    [ForeignKey("AnsweredByPlayerId")]
    public Player Player { get; set; } = null!;
}