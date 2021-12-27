using System.Security.Claims;
using DoodleForms.Shared.Abstractions;
using IdentityModel;
using Microsoft.AspNetCore.Http;

namespace DoodleForms.Api.Common;

public class CurrentUser : ICurrentUser
{
    public CurrentUser(IHttpContextAccessor contextAccessor)
    {
        var user = contextAccessor.HttpContext?.User;
        if (user == null)
        {
            return;
        }

        Id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue(JwtClaimTypes.Subject);
    }

    public string? Id { get; set; }
}