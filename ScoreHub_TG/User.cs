using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreHub_TG;

public class User
{
    public string Token {get; set;}
    public long UserId {get; set;}
}