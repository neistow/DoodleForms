using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace DoodleForms.Identity.Common;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new(Constants.ApiScopes.DoodleForms, "Provides Access To Doodle Forms")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new()
            {
                ClientId = "ui",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    Constants.ApiScopes.DoodleForms
                },
                RequireClientSecret = false,
                AllowOfflineAccess = true
            }
        };
}