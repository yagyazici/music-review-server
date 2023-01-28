namespace MusicReview.Domain.Models.Base;

public class Response
{
    public Response(string responseText, bool status)
    {
        this.responseText = responseText;
        this.status = status;
    }

    public string responseText { get; set; }
    public bool status { get; set; }
}