using Airline_Reservation_System.Models;
using Microsoft.ML;
using static MyMLApp.SentimentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<DB>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
// Add this to your service configuration
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>();


//// Load the ML model
//MLContext mlContext = new MLContext();
//ITransformer model = mlContext.Model.Load("MLModel/SentimentModel.zip", out var inputSchema);
//PredictionEngine<ModelInput, ModelOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);

//// Register Prediction Engine
//builder.Services.AddSingleton(predictionEngine);

var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.Run();
