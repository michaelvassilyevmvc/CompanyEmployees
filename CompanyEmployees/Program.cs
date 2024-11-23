using CompanyEmployees;
using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// ��������� Cors
builder.Services.ConfigureCors();

builder.Services.ConfigureIISIntegration();
// Logger
builder.Services.ConfigureLoggerService();
// Repo
builder.Services.ConfigureRepositoryManager();
//Services
builder.Services.ConfigureServiceManager();
// Database
builder.Services.ConfigureSqlContext(builder.Configuration);
// ����������� ���������� ������� � �������������
builder.Services.AddControllers()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);
// Automapper
builder.Services.AddAutoMapper(typeof(Program));
// ��������� ����������
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// ���� ����������� ������ ������ � json �� xml
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters()
.AddCustomCSVFormatter()
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);



var app = builder.Build();

// �������� LogerService �� ����� ��������
//var logger = app.Services.GetRequiredService<ILoggerManager>();
//���������� ��������� ���������� � ��������, ������� ����� ����� ������ ������������
//��� production ����� ������ ����� �� ������������ ����������
//app.ConfigureExceptionHandler(logger);
app.UseExceptionHandler(opt => { });

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");

app.UseAuthorization();

#region Middleware experiments
//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Logic before  executing the next delegate in the Use method");
//    await next.Invoke();
//    Console.WriteLine($"Logic after executing the next delegate in the Use method");
//});

//app.Map("/usingmapbranch", builder =>
//{
//    builder.Use(async (context, next) =>
//    {
//        Console.WriteLine("Map branch logic in the Use method before the next delegate");
//        await next.Invoke();
//        Console.WriteLine("Map branch logic in the Use method after the next delegate");
//    });
//    builder.Run(async context =>
//    {
//        Console.WriteLine($"Map branch response to the client in the Run method");
//        await context.Response.WriteAsync("Hello from the map branch.");
//    });
//});

//app.MapWhen(context =>
//context.Request.Query.ContainsKey("testquerystring"), builder =>
//{
//    builder.Run(async context =>
//    {
//        await context.Response.WriteAsync("Hello from the MapWhen branch.");
//    });
//});

//app.Run(async context =>
//{
//    Console.WriteLine($"Writting the response to the client in the Run method");
//    context.Response.StatusCode = 200;
//    await context.Response.WriteAsync("Hello from the middleware component.");
//});
#endregion



app.MapControllers();

app.Run();
