
using System.Security.Claims;

namespace C.Services;

public class GetUserId
{
    private readonly HttpContextAccessor context = new HttpContextAccessor();

    // public GetUserId(HttpContextAccessor context)
    // {
        // this.context = context;
    // }

    public Guid UserId
    {
        get
        {
            var userid = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userid);
        }
    }
}