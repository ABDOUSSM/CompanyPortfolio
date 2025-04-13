using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// تكوين Kestrel للاستماع على المنفذ 80 من أي عنوان
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 80); // اجعل التطبيق يستمع على المنفذ 80
});

// إضافة الخدمات
builder.Services.AddControllers();

// بناء التطبيق
var app = builder.Build();

// تكوين مسار Swagger (إذا كنت تستخدمه)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}

// استخدام HTTPS، CORS، والـ Authorization حسب الحاجة
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// تشغيل التطبيق
app.Run();
