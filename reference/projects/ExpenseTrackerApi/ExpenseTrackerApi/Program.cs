var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ExpenseTrackerApi.Repository.IExpenseRepository, ExpenseTrackerApi.Repository.InMemoryExpenseRepository>();
builder.Services.AddScoped<ExpenseTrackerApi.Services.IExpenseService, ExpenseTrackerApi.Services.ExpenseService>();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
