# DefenceDB - Modern Savunma Sanayii Veritabanı

DefenceDB, dünya çapındaki askeri platformları, silah sistemlerini ve savunma sanayii ürünlerini (Uçaklar, Tanklar, Gemiler, Füzeler, Radarlar vb.) kategorize ederek sunan kapsamlı, modern ve yüksek performanslı bir web uygulamasıdır.

## Öne Çıkan Özellikler

### 1. TPT (Table-Per-Type) Veritabanı Mimarisi
Entity Framework Core üzerinde kompleks bir kalıtım (Inheritance) yapısı kurulmuştur.
* `DefenseProduct` adında temel bir sınıf bulunur.
* Savaş Uçakları (`FighterAircraft`), Tanklar (`Tank`), Denizaltılar (`Submarine`) gibi onlarca farklı araç bu temel sınıftan türer.
* Her aracın sadece kendine has özellikleri (Tankın top çapı, Uçağın aesa radarı vb.) veritabanında ayrı tablolarda profesyonelce tutulur.

### 2. SEO Uyumlu URL Mimarisi (Slug System)
Ürün ve kategori linkleri arama motorları için özel olarak optimize edilmiştir. Ürün isimleri özel bir algoritma (Extension Method) ile temizlenip URL'ye uygun hale getirilir.
* **Kötü:** `/Product/Detail?id=103`
* **İyi (DefenceDB):** `/Product/Detail/103-altay`

### 3. Gelişmiş Görsel Optimizasyonu & Lazy Loading
Modern web standartlarına uygun yüksek performanslı medya yönetimi:
* **Otomatik Thumbnail Üretimi:** Büyük boyutlu görseller yüklendiğinde, sistem otomatik olarak (ImageSharp kullanarak) listeleme sayfaları için düşük boyutlu küçük resimler (thumbnail) oluşturur. 
* **Lazy Loading:** Resimler sadece kullanıcının ekranında görünür olduğunda yüklenir (`loading="lazy"`). Sayfa ilk açılış hızını devasa oranda artırır.

### 4. Sıfır Etki (Zero-Impact) YouTube İframe Sistemi
Ürün detay sayfalarındaki YouTube videoları sayfa açılışını asla yavaşlatmaz. Klasik iframe yerine **`srcdoc` lazy load hack** tekniği kullanılmıştır. Sayfa yüklenirken sadece hafif bir kapak resmi gelir, kullanıcı "Play" butonuna basana kadar YouTube'un ağır script'leri sisteme sızmaz.

## Kullanılan Teknolojiler

* **Backend:** C#, ASP.NET Core 8 MVC
* **Veritabanı:** Microsoft SQL Server & Entity Framework Core (Code First)
* **Mimari:** N-Tier Architecture (Katmanlı Mimari - BLL, DAL, EL, WebUI)
* **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5
* **Görsel İşleme:** SixLabors.ImageSharp

## Kurulum ve Çalıştırma

1. Projeyi klonlayın.
2. `DefenceDB.WebUI/appsettings.template.json` dosyasını kopyalayarak `appsettings.Development.json` oluşturun ve SQL Server bağlantı dizenizi (Connection String) girin.
3. Package Manager Console'da `Update-Database` komutunu çalıştırarak veritabanını oluşturun (Sistem otomatik olarak yüzlerce test verisini/seed data ekleyecektir).
4. Projeyi Visual Studio ile çalıştırın.

---

## Mimari Değerlendirmeler & Trade-Offs (Ödünleşimler)

Bu projede tercih edilen **TPT (Table-Per-Type)** kalıtım modeli ile alternatif modern yaklaşımlar (örneğin **TPH + JSONB Hibrit Yaklaşım**) arasındaki farklar ve tasarım kararları aşağıda teknik detaylarıyla açıklanmıştır.

### 1. TPT (Table-Per-Type) Neden Tercih Edildi?
Savunma sanayii alanı (domain), askeri standartlar (STANAG vb.) ve regülasyonlar nedeniyle **katı ve kararlı (stable)** bir taksonomiye sahiptir. Bir muharebe tankının namlu çapı, bir uçağın radar tipi veya bir denizaltının dalış derinliği gibi teknik parametreler uzun yıllar boyunca değişmez.

* **Tip Güvenliği (Compile-time Type Safety):** C# sınıfları ve veritabanı şeması arasında birebir strongly-typed eşleşme sağlanmıştır. Bu durum veritabanında tutarsız veri oluşmasını engeller.
* **Veritabanı Seviyesinde Kısıtlar (Database Constraints):** Her alt tabloya özel Foreign Key, Not Null ve Check kısıtları doğrudan veritabanı motoru (SQL Server/PostgreSQL) tarafından denetlenir.
* **Metadata Tabanlı Dinamik UI (Reflection):** Projenin admin paneli, alt sınıflardaki (örn: `LandVehicle`) strongly-typed property'leri C# Reflection (Yansıma) kullanarak okur ve UI formlarını sıfır kod yazımı ile dinamik olarak üretir.

### 2. TPT'nin Performans Maliyeti (JOIN Patlaması)
TPT modelinde her alt tip için ayrı bir fiziksel tablo oluşturulduğundan, tüm ürünleri alt tipleriyle birlikte çekmek istediğimizde (`_context.DefenseProducts.ToList()`) EF Core arkada tüm alt tabloları (20+ tablo) ana tabloya `LEFT JOIN` ile bağlayan devasa bir SQL sorgusu üretir:

```sql
SELECT p.Id, p.Name, t.EngineHorsePower, s.MaxDepthMeters, a.Generation ...
FROM DefenseProducts p
LEFT JOIN LandVehicles t ON p.Id = t.Id
LEFT JOIN Submarines s ON p.Id = s.Id
LEFT JOIN FighterAircrafts a ON p.Id = a.Id
-- (20+ LEFT JOIN...)
```

Bu durum yüksek ölçekli sistemlerde veritabanı CPU, I/O ve RAM kullanımını artırır.

### 3. Alternatif: TPH + JSONB (Hibrit) Yaklaşımı ve Karşılaştırma
Eğer bu proje dinamik, sürekli değişen ve sınırsız varyasyona sahip bir yapı gerektirseydi (örneğin E-Ticaret ürün özellikleri gibi), TPH (Table-Per-Hierarchy) mimarisi üzerine tek bir **JSON/JSONB** kolonu eklenerek hibrit bir model kurulabilirdi:

* **Sıfır JOIN Performansı:** Veritabanında sadece `DefenseProducts` adında tek bir tablo olur ve alt sınıflara ait özel nitelikler bu tablodaki tek bir `SpecJson` kolonunda JSON formatında tutulurdu. Bu sayede tüm ürünleri çekme işlemi sıfır JOIN ile inanılmaz hızlı gerçekleşirdi.
* **Ağ (Network) Hafifliği:** Sadece o ürüne ait dolu veriler JSON olarak taşınacağı için, TPT'deki gibi yüzlerce `NULL` kolon içeren şişmiş veri setleri oluşmazdı.

#### TPT vs TPH+JSONB Karşılaştırma Tablosu

| Kriter | TPT (Projede Seçilen) | TPH + JSONB (Alternatif) |
| :--- | :--- | :--- |
| **Derleme Zamanı Güvenliği** | En Yüksek (Strongly-typed) | Düşük (JSON string parsing) |
| **Sorgu Performansı (Milyon Satır)** | Düşük (20+ JOIN) | En Yüksek (Tek Tablo, 0 JOIN) |
| **Veritabanı Kısıtları (FK, Not Null)** | Var (Veritabanı seviyesinde) | Yok (Uygulama katmanında yönetilmeli) |
| **Şema Esnekliği (Yeni Özellik Ekleme)** | Düşük (Yeni kolon / Migration gerekir) | En Yüksek (Migration gerektirmez) |
| **Kullanım Alanı** | Regüle, Katı ve Sabit Domainler | Dinamik, Sürekli Değişen Domainler (E-Ticaret vb.) |

**Tasarım Kararı Özeti:** Projede, veri tutarlılığını garanti altına almak, güçlü tipli (strongly-typed) yapıyı korumak ve Reflection tabanlı dinamik UI motorunu en temiz şekilde beslemek amacıyla **TPT** modeli bilinçli olarak tercih edilmiştir. Ölçeklenebilirlik gereksinimlerine göre JSONB hibrit modeline geçiş planı mimari yol haritamızda yer almaktadır.

