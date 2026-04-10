using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using npost.Core;
using npost.Data;
using npost.Infraestrutura;
using npost.Middlewares;
using Constants = npost.Core.Constants;
using Secret = npost.Core.Secret;

namespace npost;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        
        ProvideService.AddDAO(builder);
        ProvideService.AddService(builder);
        ProvideService.OtherServices(builder);
        
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(Secret.GetConnectionStringDb(Constants.DbName), 
        o=> o.CommandTimeout(60)));
        
        //Configura o swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Npost",
                Description = "NPost-it",
                TermsOfService = new Uri("https://github.com/MauricioSiqueira"),
                Contact = new OpenApiContact
                {
                    Name = "Mauricio Siqueira",
                    Url = new Uri("https://github.com/MauricioSiqueira"),
                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. <br>
                      Entre com 'Bearer + [Espaço] + Token' no input abaixo.<br>
                      Exemplo: 'Bearer 12345abcdef12345abcdef12345abcdef12345abcdef12345abcde'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
                {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
        
            },
            new List<string>()
            }
            });
        });
        
        
        //Define o método de autenticação
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
        
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secret.GetJWTEncodedSecretKeyToken()),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
        
            });
        
        
        //Define as permissões de CORS
        builder.Services.AddCors(options =>
         {
             options.AddPolicy("AllowAll", builder =>
                 builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("*")
                        );
         });
        
        builder.Services.AddCors(options =>
         {
             options.AddPolicy("AllowProduction", builder =>
                 builder
                        .AllowAnyMethod()
                        .AllowAnyHeader());
         });
        
        var app = builder.Build();
        
        // AzureBlobFile.CreateContainerIfNotExists(Constants.BlobContainer);
        
        if (app.Environment.IsDevelopment())
        {
            //Print para utilizar no Swagger
            // var token = TokenService.Generation("14492082409", Constants.CodSistema, 1, 1, 1,1, 600);
            // Debug.Print(token);
            // Console.WriteLine(token);
        
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseCors("AllowAll");
            //TODO: VERIFICAR PQ NÃO FUNCIONA O AllowProduction
            //app.UseCors("AllowProduction");
            app.UseHttpsRedirection();
        }
        
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        // app.UseAuthMiddleware();
        app.MapControllers().RequireAuthorization();
        app.MapControllers();
        app.Run();
    }
}