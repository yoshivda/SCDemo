using Microsoft.EntityFrameworkCore;
using SCDemo;
using DbContext = SCDemo.DbContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddTransient<Service>();
builder.Services.AddScoped<QueryStats>();
builder.Services.AddDbContext<DbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"))
        .UseLazyLoadingProxies()
        .AddInterceptors(new QueryInterceptor())
        .EnableSensitiveDataLogging();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<HttpLogger>();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<DbContext>();
    dbContext!.Database.Migrate();
}

app.Run();
