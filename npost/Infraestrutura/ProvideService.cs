using npost.Core;
using npost.Core.Auth.DAO;
using npost.Core.Auth.Service;
using npost.DAO;
using npost.Service;

namespace npost.Infraestrutura;

public class ProvideService
{
     public static void AddDAO(WebApplicationBuilder builder)
    {
        //DAO
        builder.Services.AddTransient<UserDAO>();
        builder.Services.AddTransient<NotationDAO>();
    }

    public static void AddService(WebApplicationBuilder builder)
    {
        //Services
        builder.Services.AddTransient<UserService>();
        builder.Services.AddTransient<NotationService>();
    }
    
    public static void OtherServices(WebApplicationBuilder builder)
    {
        //Services
        builder.Services.AddTransient<UnitOfWork>();
        builder.Services.AddTransient<TokenService>();
    }
}
