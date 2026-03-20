using NoteService.Infrastructure.Settings;
using NoteService.Infrastructure.Services;
using NoteService.Application.Interfaces;
using NoteService.Application.Services;
using NoteService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<NoteMongoService>();

builder.Services.AddScoped<IPatientNoteRepository, PatientNoteRepository>();
builder.Services.AddScoped<IPatientNoteService, PatientNoteAppService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

