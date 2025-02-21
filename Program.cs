using CuestionarioInteractivo.Components;
using CuestionarioInteractivo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<CuestionarioService>();
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

var cuestionarioService = app.Services.GetRequiredService<CuestionarioService>();
cuestionarioService.CargarCuestionarioDesdeJson();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();