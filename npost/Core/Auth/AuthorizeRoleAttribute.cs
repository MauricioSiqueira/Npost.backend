using npost.Data;

namespace npost.Core.Auth;

public sealed class AuthorizeRoleAttribute : Attribute
{
    public readonly int Rule;

    public AuthorizeRoleAttribute(Roles Role)
    {
        Rule = (int)Role;
    }
}