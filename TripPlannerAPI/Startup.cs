using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using System.Net.Http;
using System.Text;
using TripPlannerAPI.Data;
using TripPlannerAPI.Models;
using TripPlannerAPI.Services;
using TripPlannerAPI.Repositories;
using System.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.XPath;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
 );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "TripPlanner API", Version = "v1" });
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer n on textbox below!",
       
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});


//builder.Services.Configure(options =>
//    options.DescribeAllEnumsAsStrings();
//    var xmlDocFile = Path.Combine(AppContext.BaseDirectory, $"{_hostingEnv.ApplicationName}.xml");
//    if (File.Exists(xmlDocFile))
//    {
//        var comments = new XPathDocument(xmlDocFile);
//        options.OperationFilter<XmlCommentsOperationFilter>(comments);
//        options.ModelFilter<XmlCommentsModelFilter>(comments);
//    }
//);

bool isDbSqlite = true;
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("SQLITE_DB_CONNECTIONSTRING"))
        //options.UseSqlServer(builder.Configuration.GetConnectionString("DOCKER_MSSQLSERVER_LOCAL_DB"))
        //options.UseNpgsql(builder.Configuration.GetConnectionString("DOCKER_POSTGRES_LOCAL_DB"))
        );//("AZURE_SQL_CONNECTIONSTRING"))); //("LOCAL_DB")));

//builder.Services.AddDbContext<AppDbContext>(opt =>
//{
//    //System.Diagnostics.Debug.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});
//builder.Services.AddCors();
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration["JWTSettings:TokenKey"]))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<ITokenService,TokenService>();
//builder.Services.AddScoped<TripManager>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITripTypePreferenceRepository, TripTypePreferenceRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<IUserRatingRepository, UserRatingRepository>();
builder.Services.AddCors(options => options.AddPolicy("Cors",
            builder =>
            {
                builder.
                AllowAnyOrigin().
                AllowAnyMethod().
                AllowAnyHeader();
            }));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (true) //(app.Environment.IsDevelopment())
{
    app.UseSwagger(c=>
    {
        c.RouteTemplate = "api/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api/v1/swagger.json", "TripPlanner API Spec (Swagger)");

        c.RoutePrefix = "api";
    });
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //db.Database.EnsureDeleted();
    //db.Database.EnsureCreated();
    await db.Database.MigrateAsync();

}
//app.UseHttpsRedirection();



//app.UseCors(x => x
//.AllowAnyOrigin()
//.AllowAnyHeader()
//.AllowAnyMethod()
//.SetIsOriginAllowed(origin=>true));
app.UseRouting();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles(); //for serving wwwroot react static files.

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
