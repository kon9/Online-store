using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.ApiService.Authorization;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Clients;
using OnlineStore.Library.Clients.ArticlesService;
using OnlineStore.Library.Clients.AspIdentity;
using OnlineStore.Library.Clients.IdentityServer;
using OnlineStore.Library.Clients.OrdersService;
using OnlineStore.Library.Clients.UserManagementService;
using OnlineStore.Library.Constants;
using OnlineStore.Library.Options;
using OnlineStore.Library.OrdersService.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<UsersClient>();
builder.Services.AddHttpClient<RolesClient>();
builder.Services.AddHttpClient<IdentityServerClient>();
builder.Services.AddHttpClient<ArticlesClient>();
builder.Services.AddHttpClient<PriceListsClient>();
builder.Services.AddHttpClient<OrderedArticle>();
builder.Services.AddHttpClient<Order>();
builder.Services.AddHttpClient<IAspIdentityClient, AspIdentityClient>();

builder.Services.AddTransient<IRolesClient, RolesClient>();
builder.Services.AddTransient<IUserClient, UsersClient>();
builder.Services.AddTransient<IIdentityServerClient, IdentityServerClient>();
builder.Services.AddTransient<IClientAuthorization, HttpClientAuthorization>();
builder.Services.AddTransient<IRepoClient<Article>, ArticlesClient>();
builder.Services.AddTransient<IRepoClient<PriceList>, PriceListsClient>();
builder.Services.AddTransient<IRepoClient<OrderedArticle>, OrderedArticlesClient>();
builder.Services.AddTransient<IRepoClient<Order>, OrdersClient>();

builder.Services.Configure<AspIdentityApiOptions>(builder.Configuration.GetSection(ServiceAddressOptions.SectionName));
builder.Services.Configure<ServiceAddressOptions>(builder.Configuration.GetSection(ServiceAddressOptions.SectionName));

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        options.RequireHttpsMetadata = false;
    });

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", IdConstants.ApiScope);
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
