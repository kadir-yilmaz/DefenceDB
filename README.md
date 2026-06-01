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

