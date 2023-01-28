using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Models.Responses;

public class Success: Response
{
    public Success(object response, string responseText, bool status = true) : base(responseText, status)
    {
        this.response = response;
    }

    public object response { get; set; }
}