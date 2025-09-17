# دليل النشر المباشر على السيرفر باستخدام CI/CD أو Web Deploy

## الطريقة الأولى: النشر المستقل (Self-Contained) مع معمارية x64

### 1. النشر المستقل باستخدام Command Line:
```bash
# نشر مستقل مع معمارية x64
dotnet publish --configuration Release --self-contained true --runtime win-x64 --output ./publish

# نشر مستقل مع ملف تنفيذي واحد
dotnet publish --configuration Release --self-contained true --runtime win-x64 --output ./publish -p:PublishSingleFile=true

# نشر مستقل محسّن (مع تقليل الحجم)
dotnet publish --configuration Release --self-contained true --runtime win-x64 --output ./publish -p:PublishSingleFile=true -p:PublishTrimmed=true
```

### 2. مميزات النشر المستقل:
- لا يتطلب تثبيت .NET Runtime على السيرفر المستهدف
- يتضمن جميع المكتبات والتبعيات المطلوبة
- يدعم معمارية x64 بشكل مُحسّن
- إمكانية إنشاء ملف تنفيذي واحد (Single File)

### 3. تشغيل التطبيق:
```bash
# تشغيل مباشر بدون .NET Runtime
./Home.exe

# أو على Windows
Home.exe
```

## الطريقة الثانية: النشر باستخدام Web Deploy من Visual Studio

1. ضبط إعدادات النشر:
   - افتح المشروع في Visual Studio
   - انقر بزر الماوس الأيمن على المشروع
   - اختر "Publish..."
   - انقر على "New" لإنشاء ملف تعريف نشر جديد
   - اختر "IIS, FTP, etc."
   - حدد "Web Deploy" كطريقة النشر
   - أدخل عنوان السيرفر وبيانات الاعتماد والمسار

2. تكوين باراميترات النشر:
   - تأكد من إعداد متغيرات البيئة المناسبة (`Environment=Production`)
   - ضبط سلسلة الاتصال بقاعدة البيانات
   - تمكين الـ Precompilation إذا لزم الأمر
   - حدد خيار "Remove additional files at destination" إذا كنت ترغب في إزالة الملفات القديمة

3. النشر:
   - انقر على "Publish" لبدء عملية النشر
   - تحقق من نجاح النشر من خلال الوصول إلى التطبيق عبر المتصفح

## الطريقة الثالثة: استخدام GitHub Actions للنشر التلقائي (CI/CD)

1. إنشاء ملف GitHub Actions workflow:
   - أنشئ مجلد `.github/workflows` في مشروعك
   - أنشئ ملف `deploy.yml` بالمحتوى التالي:

```yaml
name: Deploy to Production

on:
  push:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '9.0.x'
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --configuration Release --no-restore
      
    - name: Publish (Self-Contained)
      run: dotnet publish --configuration Release --self-contained true --runtime win-x64 --output ./publish -p:PublishSingleFile=true
      
    - name: Deploy to IIS
      uses: ChristopheLav/iis-deploy@v1
      with:
        website-name: 'YourWebsiteName'
        msdeploy-service-url: ${{ secrets.IIS_SERVER_URL }}
        msdeploy-username: ${{ secrets.IIS_USERNAME }}
        msdeploy-password: ${{ secrets.IIS_PASSWORD }}
        source-path: './publish'
```

2. إعداد أسرار GitHub (Secrets):
   - في مستودع GitHub، انتقل إلى Settings > Secrets
   - أضف الأسرار التالية:
     - `IIS_SERVER_URL`: عنوان URL للخادم (مثال: `https://yourserver.com:8172/msdeploy.axd`)
     - `IIS_USERNAME`: اسم المستخدم لحساب Web Deploy
     - `IIS_PASSWORD`: كلمة المرور لحساب Web Deploy

3. دفع التغييرات إلى GitHub:
   - عند دفع تغييرات إلى الفرع الرئيسي، سيتم تشغيل workflow تلقائيًا
   - تحقق من نتائج النشر في تبويب "Actions" على GitHub

## الطريقة الرابعة: تشغيل كخدمة Windows (Windows Service)

1. إنشاء خدمة Windows:
   - قم بتثبيت حزمة `Microsoft.Extensions.Hosting.WindowsServices`:
   ```bash
   dotnet add package Microsoft.Extensions.Hosting.WindowsServices
   ```

2. تعديل ملف Program.cs:
   - أضف `.UseWindowsService()` إلى Host Builder:
   ```csharp
   var builder = WebApplication.CreateBuilder(new WebApplicationOptions
   {
       Args = args,
       ContentRootPath = AppContext.BaseDirectory
   });
   
   // أضف هذا السطر لتشغيل التطبيق كخدمة Windows
   builder.Host.UseWindowsService();
   ```

3. نشر التطبيق (مستقل):
   ```bash
   dotnet publish --configuration Release --self-contained true --runtime win-x64 --output C:\path\to\publish -p:PublishSingleFile=true
   ```

4. تثبيت الخدمة باستخدام SC:
   ```powershell
   sc.exe create "HomeRealEstate" binPath="C:\path\to\publish\Home.exe" DisplayName="Home Real Estate Service"
   sc.exe start "HomeRealEstate"
   ```

## الطريقة الخامسة: استخدام Docker

1. إنشاء Dockerfile (مع النشر المستقل):
   ```dockerfile
   FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
   WORKDIR /app
   EXPOSE 80
   EXPOSE 443

   FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
   WORKDIR /src
   COPY ["Home.csproj", "./"]
   RUN dotnet restore "./Home.csproj"
   COPY . .
   WORKDIR "/src/."
   RUN dotnet build "Home.csproj" -c Release -o /app/build

   FROM build AS publish
   RUN dotnet publish "Home.csproj" -c Release -o /app/publish --self-contained true --runtime linux-x64 -p:PublishSingleFile=true

   FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final
   WORKDIR /app
   COPY --from=publish /app/publish .
   ENTRYPOINT ["./Home"]
   ```

2. بناء ونشر الصورة:
   ```bash
   docker build -t home-realstate:latest .
   docker run -d -p 80:80 -p 443:443 --name home-realstate home-realstate:latest
   ```

3. للنشر على سيرفر بعيد، ادفع الصورة إلى سجل Docker (مثل Docker Hub) ثم اسحبها على السيرفر:
   ```bash
   docker push yourusername/home-realstate:latest
   
   # على السيرفر البعيد
   docker pull yourusername/home-realstate:latest
   docker run -d -p 80:80 -p 443:443 --name home-realstate yourusername/home-realstate:latest
   ```

## ملاحظات هامة

### حول النشر المستقل (Self-Contained):
- **المميزات:**
  - لا يتطلب تثبيت .NET Runtime على السيرفر
  - توافق أفضل مع البيئات المختلفة
  - أداء محسّن للتطبيقات الكبيرة
  - استقلالية كاملة عن إصدارات .NET المثبتة على النظام

- **الاعتبارات:**
  - حجم أكبر للملفات (حوالي 130-140 MB)
  - وقت build أطول قليلاً
  - يجب تحديد معمارية النظام المستهدف

### نصائح عامة:
- تأكد من نسخ ملفات appsettings.json المناسبة للبيئة المستهدفة.
- تأكد من إعداد قاعدة البيانات بشكل صحيح وإجراء الهجرات اللازمة.
- تأكد من صلاحيات الوصول للمجلدات المطلوبة، خاصة مجلدات تحميل الصور.
- احرص على أمان كلمات المرور وسلاسل الاتصال باستخدام متغيرات البيئة أو أسرار Azure KeyVault.
- للبيئات الإنتاجية، فعّل HTTPS وقم بتكوين شهادات SSL المناسبة.