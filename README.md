
# Identity Project

This project uses Identity to manage users following instructions from this youtube course:
[Máster en .NET 6 Identity, Autenticación y Autorización](https://www.udemy.com/course/identity-autenticacion-autorizacion/)



## Technologies used
The technologies used in these project are:
- .Net 6
- Identity 6.0.15
- Entity framework core 6.0.15
- [MailJet](https://www.mailjet.com/) API: 3.0.0
## Initial configuration
Install packages using nuget package manager 
- AutoMapper.Extensions.Microsoft.DependencyInjection version 12.0.1
- Mailjet.Api version 3.0.0 
- Microsoft.AspNetCore.Identity.UI 6.0.16
- Microsoft.AspNetCore.Identity.EntityFrameworkCore version 6.0.15
- Microsoft.EntityFrameworkCore.Tools 6.0.15
- Microsoft.EntityFrameworkCore 6.0.15
- Microsoft.EntityFrameworkCore.SqlServer 6.0.15

Create user in [Mailjet](https://www.mailjet.com/) and ApiKey and Secretkey and add it to appsettings
### Configure MailJet
In Appsettings:
````json
"MailJet": {
    "Apikey": "####",
    "Secretkey": "####"
  }
````
Create options class to map appsettings:

````
namespace ProyectoIdentity.Services
{
    public class OptionsMailJet
    {
        public string Apikey { get; set; }
        public string Secretkey { get; set; }
    }
}
````
Create email sender class:

````
public class MailJetEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    public OptionsMailJet _optionsMailJet;
    public MailJetEmailSender(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _optionsMailJet = _configuration.GetSection("MailJet").Get<OptionsMailJet>();

        MailjetClient client = new MailjetClient(_optionsMailJet.Apikey, _optionsMailJet.Secretkey);

        var myEmail = new TransactionalEmailBuilder()
            .WithFrom(new SendContact("julianlondono@outlook.com"))
            .WithSubject(subject)
            .WithHtmlPart(htmlMessage)
            .WithTo(new SendContact(email))
            .Build();
        var response = await client.SendTransactionalEmailAsync(myEmail); ;
    }
}
````
Configure in Program class
````
builder.Services.AddTransient<IEmailSender, MailJetEmailSender>();
````
### Configure Automapper
Create class with mappings
````
public class AutomapperProfiles: Profile
{
    public AutomapperProfiles()
    {
        CreateMap<AppUser, RegisterViewModel>();
        CreateMap<RegisterViewModel, AppUser>().ForMember(x => x.UserName, p => p.MapFrom(a => a.Email));
    }
}
````
Configure Program class
````
builder.Services.AddAutoMapper(typeof(AutomapperProfiles));
```` 
### Configure Entity framework core as code first
Create connection string in Appsettings
````
"ConnectionStrings": {
    "SQLConnection": "Server=DESKTOP-QQJ2RCB\\SQLEXPRESS;Database=IdentityBD;Integrated Security=True"
  },
````
Create user class, must inhet from **IdentityUser**
````
public class AppUser: IdentityUser
{
    public string Name { get; set; }
    public string Url { get; set; }
    public int CountryCode { get; set; }
    public string Telephone { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public DateTime BirthDate { get; set; }
    public bool State { get; set; }
}
````
Create DbContext
````
public class ApplicationDbContext: IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options): base(options){}
    public DbSet<AppUser>AppUsers { get; set; }
}
````
Configure Program class
````
builder.Services.AddDbContext <ApplicationDbContext>(options 
    => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));
````
### Configure Identity
Configure in Program class
````
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Accounts/Access");
    options.AccessDeniedPath = new PathString("/Accounts/Bloquead");
});
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.SignIn.RequireConfirmedEmail = true;
});
````
### Create Database
Run commands to create database as usual Entity framework procedures in cmd.  
`dotnet ef migrations add InitialCreate`   
`dotnet ef database update`

## Features
This project includes this features
1. Register
2. Email to Confirm Account
3. Access
4. Reset password functionality
5. Email to reset password
6. Delete account view


## Authors
- Student:  Julian Gallo Londoño
- Profesor: [render2web](https://www.udemy.com/user/render2web/)
