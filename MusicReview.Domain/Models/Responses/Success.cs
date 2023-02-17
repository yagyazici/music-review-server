using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Models.Responses;

public class Success<TResponse>: Response
{
    public Success(TResponse response, string responseText, bool status = true) : base(responseText, status)
    {
        this.response = response;
    }

    public TResponse response { get; set; }
}