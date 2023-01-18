namespace MusicReview.Domain.Models.Base;

public class Response
{
    public Response(string responseText)
    {
        this.responseText = responseText;
    }

    public string responseText { get; set; }
}