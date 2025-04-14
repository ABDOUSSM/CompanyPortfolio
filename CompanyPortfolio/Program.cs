using System.Net;
using CompanyPortfolio.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ إعداد Kestrel للاستماع على 0.0.0.0:8080 (لتوافق مع Fly.io)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080); // ✅ طريقة أكثر وضوحًا
});

// إضافة الخدمات إلى الحاوية
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CompanyPortfolio API", Version = "v1" });
});

// إعداد CORS لتحديد الأصول المسموحة
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins(
            builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
            ?? new[] { "http://localhost:3000", "https://riseley.vercel.app" }  // أضف رابط الـ frontend الخاص بك على Vercel
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// تسجيل خدمة البريد الإلكتروني
builder.Services.AddScoped<IEmailService, MailjetEmailService>();

var app = builder.Build();

// تفعيل Swagger في وضع التطوير
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompanyPortfolio API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowedOrigins");
app.UseAuthorization();
app.MapControllers();

// بدء التطبيق على المنفذ 8080
app.Run();
