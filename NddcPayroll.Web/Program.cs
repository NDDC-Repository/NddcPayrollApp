using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NddcPayroll.Web.Renderers;
using NddcPayrollLibrary.Data;
using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Data.Calculations.Deductions;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.DataManagement;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Helper;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.PayrollJournal;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Validators;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddTransient<IValidator<EmployeeModel>, EmployeeValidator>();
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
builder.Services.AddSyncfusionBlazor();
builder.Services.AddTransient<IPayrollJournalData, SQLPayrollJournal>();
builder.Services.AddScoped<IRazorTemplateRenderer, RazorTemplateRenderer>();

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
    options.Conventions.AllowAnonymousToPage("/Reports/PayrollSummaryByDept");
    options.Conventions.AllowAnonymousToPage("/PdfPages/EmployeePayslip/");
    options.Conventions.AllowAnonymousToPage("/VerifyStaff");
})
.AddMvcOptions(options => { })
.AddMicrosoftIdentityUI();

var app = builder.Build();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhkVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jS39WdkJgXHtfdnZRQQ==;Mgo+DSMBPh8sVXJ0S0J+XE9AflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TdERmWXhddXBUQGRcVg==;ORg4AjUWIQA/Gnt2VVhkQlFacldJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkdiXH9Zc3FRR2dfVkM=;MTEzNTM3NkAzMjMwMmUzNDJlMzBqYlZSNjRzK09VWGpDbXYzdFh6RnlHTiszL3U0cXhlTTFCNzVJWXdwVzBjPQ==;MTEzNTM3N0AzMjMwMmUzNDJlMzBFNk9lZWc4ckdDbFlNOVFvWFpscGpRNE8vNUJoKzNjSG85R2tXZnY1S0RRPQ==;NRAiBiAaIQQuGjN/V0Z+WE9EaFtKVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdUVhWHped3dQQmFUUEB0;MTEzNTM3OUAzMjMwMmUzNDJlMzBBSmdKc01lcnpnUVBrNndRNlpiWkFOQlVieUZyTm83VTl5Zjd3eWlHMnZBPQ==;MTEzNTM4MEAzMjMwMmUzNDJlMzBneEViVis0Y3JIaitpR3RjZ0hkdGV3RGRRakVsTmlHdjhUeTNZelpqd3drPQ==;Mgo+DSMBMAY9C3t2VVhkQlFacldJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkdiXH9Zc3FRR2hUVEM=;MTEzNTM4MkAzMjMwMmUzNDJlMzBnYlJDMDk2VXZpa3JJdk5EVEszTWxwcHZWbTFlNjFSbHE3SitxNDdsLzM0PQ==;MTEzNTM4M0AzMjMwMmUzNDJlMzBIZ09NQ2s2QkIzYU1WUGlxdHpMNnlVVUdEQnpRZ0JSb0ErUlVybzJBRnNnPQ==;MTEzNTM4NEAzMjMwMmUzNDJlMzBBSmdKc01lcnpnUVBrNndRNlpiWkFOQlVieUZyTm83VTl5Zjd3eWlHMnZBPQ==");

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
