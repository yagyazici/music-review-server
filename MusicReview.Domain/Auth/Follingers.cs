namespace MusicReview.Domain.Auth;

public class Follingers
{
    public Follingers(int followers, int followings)
    {
        this.followers = followers;
        this.followings = followings;
    }

    public int followers { get; set; }
    public int followings { get; set; }
}