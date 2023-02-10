using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NddcPayrollLibrary.Data;
using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Data.Calculations.Deductions;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.DataManagement;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Helper;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Databases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<ICompanyData, SQLCompany>();
builder.Services.AddTransient<IPayrollData, SQLPayroll>();
builder.Services.AddTransient<IEmployeeData, SQLEmployee>();
builder.Services.AddTransient<IHelperData, SQLHelper>();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IAllowanceData, SQLAllowance>();
builder.Services.AddTransient<IDeductionData, SQLDeduction>();
builder.Services.AddTransient<IDataMigration, SQLMigration>();
builder.Services.AddTransient<IReportsData, SQLReports>();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to 
    // the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages(options => {
    options.Conventions.AllowAnonymousToPage("/Index");
})
.AddMvcOptions(options => { })
.AddMicrosoftIdentityUI();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapBlazorHub();

app.Run();
