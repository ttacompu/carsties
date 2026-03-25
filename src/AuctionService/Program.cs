using AuctionService;
using AuctionService.Consumers;
using AuctionService.Data;
using AuctionService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>( opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(cfg => {}, typeof(Program));
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AuctionDbContext>(opt =>
    {
        opt.QueryDelay = TimeSpan.FromSeconds(10);
        opt.UsePostgres();
        opt.UseBusOutbox();
    });

    x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction",false));
    x.UsingRabbitMq((context, cfg) =>
    {
            cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
            {
                
                h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
                h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
            });
        cfg.ConfigureEndpoints(context);
    });
});
 builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.Authority = builder.Configuration["IdentityServiceUrl"];
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters.ValidateAudience = false;
         options.TokenValidationParameters.NameClaimType = "username";
     });
    builder.Services.AddGrpc();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<GrpcAuctionService>();
var retryPolicy = Policy
    .Handle<NpgsqlException>()
    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(5));

retryPolicy.ExecuteAndCapture( () => DbInitializer.InitDb(app));

app.Run();

