using Domain.Entities;

namespace GameServerAPI;

public static class GameHelper
{
    /// <summary>
    /// Generates a random round with a random expression
    /// </summary>
    /// <returns>instance of GameRound model with an expression and an answer</returns>
    public static GameRound CreateNewRound()
    {
        string expression = string.Empty;
        string answer = string.Empty;

        //Create two random numbers from 1 to 10 and create random expression
        var randomGenerator = new Random();
        var a = randomGenerator.Next(1,11);
        var b = randomGenerator.Next(1,11);
        var operation = randomGenerator.Next(0,4);

        switch(operation)
        {
            case 0: // addition
                expression = string.Format("{0} + {1} = ?", a , b);
                answer = (a + b).ToString();
                break;
            case 1: // subtraction
                expression = string.Format("{0} - {1} = ?", a , b);
                answer = (a - b).ToString();
                break;
            case 2: //  multiply
                expression = string.Format("{0} * {1} = ?", a , b);
                answer = (a * b).ToString();
                break;
            case 3: // divide
                expression = string.Format("{0} / {1} = ?", a , b);
                answer = (a / b).ToString();
                break;
            default:
                break;
        }
        return new GameRound
        {
            Expression = expression,
            CorrectAnswer = answer
        };
        
    }
}
