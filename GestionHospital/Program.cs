using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CapaDatos;
using CapaNegocio;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("cn");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});


builder.Services.AddControllersWithViews(); 
builder.Services.AddRazorPages();

// Register DAL and BL classes
builder.Services.AddScoped<PacienteDAL>();
builder.Services.AddScoped<PacienteBL>();
builder.Services.AddScoped<AccountBL>();
builder.Services.AddScoped<MedicoDAL>();
builder.Services.AddScoped<MedicoBL>();
builder.Services.AddScoped<CitaDAL>();
builder.Services.AddScoped<CitaBL>();
builder.Services.AddScoped<TratamientoDAL>();
builder.Services.AddScoped<TratamientoBL>();
builder.Services.AddScoped<FacturacionDAL>();
builder.Services.AddScoped<FacturacionBL>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Doctor", "Patient", "Staff"};
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string email = "admin@admin.com";
    string password = "Admin1234,";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");

    }
}
app.Run();
