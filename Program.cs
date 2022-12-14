using ManualProfiler;
using ServiceProfiler.EventPipe.UserApp30;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(opt =>
{
    opt.SingleLine = true;
});

// Add services to the container.
builder.Services.AddHostedService<HeartBeatService>();
builder.Services.AddHostedService<SocketUsage>();
builder.Services.AddHostedService<ProfilerService>();
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
