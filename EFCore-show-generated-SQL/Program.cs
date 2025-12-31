using EFCore_show_generated_SQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    using var db = new BlogDbContext();
    db.Blogs.Add(new Blog { Url = "https://example.com" });
    db.SaveChanges();
    var query = db.Blogs.Where(q => q.BlogId > 2); //1
    return db.Blogs.ToList();


});

app.Run();
