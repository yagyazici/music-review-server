using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Models.Responses;

public class Fail : Response
{
    public Fail(object response, string responseText, bool status = false) : base(responseText, status)
    {
        this.response = response;
    }

    public object response { get; set; }
}