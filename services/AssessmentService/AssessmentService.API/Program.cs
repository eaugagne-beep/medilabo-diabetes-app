using AssessmentService.Application.Interfaces;
using AssessmentService.Infrastructure.Clients;





var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddHttpClient<IPatientServiceClient, PatientServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5261");
});

builder.Services.AddHttpClient<INoteServiceClient, NoteServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5148");
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();