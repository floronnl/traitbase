// 
// Runtime bouwen in Visual Studio en online publiceren
// 0. appSettings.Development.json moet gelijk zijn aan appSettings.json
// 1. Build -> Build Biobase.API   (CTRL+B)
// 2. Build -> Publish Biobase.API
// 3. Bestandsmap bin/release/net8.0/publish kopieren naar server root folder
// 

using biobase.API.Data;
using biobase.API.Mappings;
using biobase.API.Middleware;
using biobase.API.Repositories;
using biobase.API.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Registering MVC Controllers
builder.Services.AddControllers();

// Add Endpoints API Explorer for discovering and exposing APIs
builder.Services.AddEndpointsApiExplorer();

// Swagger setup for API documentation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Traitbase API",
        Version = "v1",
        Description = "**About Traitbase:**\n\n" +
        "This API supports various filters and output formats to retrieve data on habitat types, taxa and species traits and protection status in The Netherlands.\n\n" +
        "For some endpoints an API key is required. An API key can be requested by creating an account at [NDFF Verspreidingsatlas](https://www.verspreidingsatlas.nl). " +
        "This API was developed by FLORON Plant Conservation Netherlands with support of Wageningen University and Research (WUR), " +
        "Statistics Netherlands (CBS), Dutch Bryological and Lichenological Society (BLWG), Dutch Mycological Society (NMV).",
    });

    // Add XML comments to Swagger for better documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Enable annotations in Swagger for additional metadata
    options.EnableAnnotations();

    // Tags Selector: Allows dynamic tag filtering in Swagger
    options.SwaggerGeneratorOptions.TagsSelector = apiDesc =>
        apiDesc.ActionDescriptor.EndpointMetadata.OfType<SwaggerOperationAttribute>().FirstOrDefault()?.Tags;

    // Global API Key Security Setup for Swagger
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key required.",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                Scheme = "ApiKeyScheme",
                Name = "X-API-KEY",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Fetch the connection string from the configuration
var connectionString = builder.Configuration.GetConnectionString("biobaseConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'biobaseConnection' not found.");
}

// Register DbContext with MySQL connection string
builder.Services.AddDbContext<BiobaseDbContext>(options =>
    options.UseMySQL(connectionString));

// Dependency Injection for Repositories and Services
builder.Services.AddScoped<ITaxaRepository, SQLTaxaRepository>();
builder.Services.AddScoped<ITraitsRepository>(sp => new SQLTraitsRepository(connectionString));
builder.Services.AddScoped<ITraitsCategoriesRepository, SQLTraitsCategoriesRepository>();
builder.Services.AddScoped<ICsvExportService, CsvExportService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHabitatClassesTaxaRepository, SQLHabitatClassesTaxaRepository>();

// Registering AutoMapper profiles for object mapping
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Register console logging for diagnostics
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline

// Developer exception page only in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable static files like CSS and JS
app.UseStaticFiles();

// Enable Swagger UI for API documentation
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.InjectStylesheet("/swagger-ui/custom.css");
    options.DocumentTitle = "Traitbase";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Traitbase API v1");
});

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Use custom API Key Middleware to secure API
app.UseMiddleware<ApiKeyMiddleware>();

// Map the controllers to their routes
app.MapControllers();

// Run the application
app.Run();
