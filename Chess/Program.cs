

publicabstract class Player
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

public class Match : MatchData
{
    private int player1;
    private int player2;

    public Match(int player1, int player2, List<string> moves, List<int> results, int winner): base(moves, results, winner);
    {
        this.player1 = player1;
        this.player2 = player2;
    }

}

public class MatchData
{
    private List<string> moves;
    private List<int> results;

    private int winner;

    public MatchData(List<string> moves, List<int> results, int winner)
    {
        this.moves = moves;
        this.results = results;
        this.winner = winner;
    }
}

