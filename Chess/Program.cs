namespace data;

public abstract class Player
{
    private string firstname;
    private string lastname;
    private int number;

    public Player(string firstname, string lastname, int number)
    {
        this.firstname = firstname;
        this.lastname = lastname;
        this.number = number;
    }

    public string override string ToString()
    {
        return $"{firstname} {lastname} (#{number})";
    }

}

public abstract class Activity
{
    private string name;
    private Match match; // in minutes

    public Activity(string name, Match Match)
    {
        this.name = name;
        match = Match;
    }

}

public class Match : MatchScore
{
    private int player1;
    private int player2;

    public Match(int player1, int player2, List<string> moves, List<int> results, int winner): base(moves, results, winner);
    {
        this.player1 = player1;
        this.player2 = player2;
    }

}

public class MatchScore : GradingMethod
{
    private List<string> moves;

    private int winner;

    private int grading;

    public MatchScore(List<string> moves, int winner, int grading) : base(grading)
    {
        this.moves = moves;
        this.winner = winner;
    }
}

public static class GradingMethod
{

    private int grading;

    public GradingMethod
    {
        //grading method here, ELO in this case
    }
}   
