var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/ApiTest", async (CancellationToken token) =>
{

    var result1 = await DoWorkAsync(token);
    var result2 = await DoWorkLoop(token);

    return new { result1 = result1, result2 = result2 };
})
.WithName("ApiTest")
.WithOpenApi();

static async Task<string> DoWorkAsync(CancellationToken cancellationToken)
{
    try
    {
        // A long-running operation like database, heavy business logics
        await Task.Delay(5000, cancellationToken);

        return "Operation completed.";
    }
    catch (OperationCanceledException ex)
    {
        return "Operation canceled";
    }
}

static async Task<string> DoWorkLoop(CancellationToken cancellationToken)
{
    try
    {
        var result = "";
        for (int i = 0; i < 1000; i++)
        {
            if (cancellationToken.IsCancellationRequested)
                return $"Operation cancelled: result = {result}";
            await Task.Delay(1000);
            result += i;
        }
        // A long-running operation like database, heavy business logics

        return $"Operation completed. result = {result}";
    }
    catch (OperationCanceledException ex)
    {
        return "Operation canceled";
    }
}

app.Run();

