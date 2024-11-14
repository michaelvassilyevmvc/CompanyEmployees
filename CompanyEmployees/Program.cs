using NLog;
using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Contracts;
using CompanyEmployees;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddControllers()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


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
