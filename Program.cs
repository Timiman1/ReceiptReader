using Microsoft.EntityFrameworkCore;
using ReceiptReader.Application.Analyzers;
using ReceiptReader.Application.FileStorage;
using ReceiptReader.Application.Repositories;
using ReceiptReader.Application.Services;
using ReceiptReader.Application.Utility;
using ReceiptReader.Application.Validation;
using ReceiptReader.Infrastructure.Analyzers;
using ReceiptReader.Infrastructure.Data;
using ReceiptReader.Infrastructure.FileStorage;
using ReceiptReader.Infrastructure.Repositories;
using ReceiptReader.Infrastructure.Configurations;
using ReceiptReader.Infrastructure.Utility;
using ReceiptReader.Infrastructure.Validation;
using ReceiptReader.Application.RawTextExtractors;
using ReceiptReader.Infrastructure.RawTextExtractors;
using ReceiptReader.Application.ReceiptDataExtractors;
using ReceiptReader.Infrastructure.ReceiptDataExtractors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurations
builder.Services
    .Configure<FileSettings>(
        builder.Configuration.GetSection(FileSettings.SectionName))
    .Configure<TesseractOptions>(
        builder.Configuration.GetSection(TesseractOptions.SectionName))
    .Configure<AzureAIDocumentIntelligenceOptions>(
        builder.Configuration.GetSection(AzureAIDocumentIntelligenceOptions.SectionName))
    .Configure<ReceiptDataExtractionOptions>(
        builder.Configuration.GetSection(ReceiptDataExtractionOptions.SectionName)
);

// Controllers
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application Services
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IFileRetrievalService, FileRetrievalService>();

// File Validator
builder.Services.AddSingleton<IFileValidator, FileValidator>();

// Content Type Resolver
builder.Services.AddSingleton<IContentTypeResolver, ContentTypeResolver>();

// File Storage
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();

// Receipt Analyzer
builder.Services
    .AddScoped<IReceiptAnalyzer, DefaultReceiptAnalyzer>()
    .AddScoped<IReceiptInfoMapper, DefaultReceiptInfoMapper>()
    .AddScoped<IReceiptKeywordClassifier, DefaultReceiptKeywordClassifier>();

// Hash Calculator (for file caching needs).
builder.Services.AddSingleton<IHashCalculator, HashCalculator>();

// Raw Text Extractors
builder.Services
    .AddScoped<ILowCostRawTextExtractor, TesseractRawTextExtractor>()
    .AddScoped<IHighCostRawTextExtractor, AzureAIDocumentIntelligenceRawTextExtractor>();

// Receipt Data Extraction Services
builder.Services
    .AddScoped<IReceiptDataExtractor, DefaultReceiptDataExtractor>()
    .AddScoped<ISimpleFieldExtractor, RegexSimpleFieldExtractor>()
    .AddScoped<ILineItemExtractor, RegexLineItemExtractor>()
    .AddScoped<ITaxLineExtractor, RegexTaxLineExtractor>()
    .AddScoped<ITotalValidator, RegexTotalValidator>()
    .AddScoped<IExtractionGatekeeper, DefaultExtractionGateKeeper>();

// Repositories
builder.Services.AddScoped<IReceiptRepository, EfReceiptRepository>();
builder.Services.AddScoped<IAnalysisLogRepository, EfAnalysisLogRepository>();

// DB Context
var connectionString = builder.Configuration.GetConnectionString("ReceiptDatabase");
builder.Services.AddDbContext<ReceiptDbContext>(options =>
    options.UseSqlite(connectionString));

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
