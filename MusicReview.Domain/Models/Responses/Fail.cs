using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Models.Responses;

public class Fail<TResponse> : Response
{
    public Fail(TResponse response, string responseText, bool status = false) : base(responseText, status)
    {
        this.response = response;
    }

    public TResponse response { get; set; }
}