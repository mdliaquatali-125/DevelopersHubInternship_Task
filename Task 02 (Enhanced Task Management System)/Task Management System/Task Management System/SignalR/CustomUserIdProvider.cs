using Microsoft.AspNet.SignalR;

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(IRequest request)
    {
        return request.QueryString["userId"];
    }
}