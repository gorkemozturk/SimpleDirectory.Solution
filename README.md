## SimpleDirectory
Çözüm bünyesinde çok katmanlı mimari yapısı tercih edilmiştir. Her katmanın ayrı prensip ve görevleri vardır. Bu katmanlar:
  - SimpleDirectory.Data
  - SimpleDirectory.Domain
  - SimpleDirectory.Extension
  - SimpleDirectory.Web
  - SimpleDirectory.Test
  - SimpleDirectory.Consumer

olarak listelenmektedir.


### Veritabanı ve Migration
Çözüm bünyesinde PostgreSQL kullanılmış ve tabloların droplanması için ilgili migrationlar ".Data" katmanında hazırlanmıştır.


### Kuyruklama Sistemi (RabbitMQ)
Kuyruklama sistemi olarak çözüm bünyesinde RabbitMQ kullanışmıştır. Çözümü debug etmek için lokal cihaza herhangi bir kurulum yapılmasına gerek olmayıp, ilgili kuyruklama sistemi bulut ortamında barınmaktadır (İlgili servis sağlayıcısı: https://www.cloudamqp.com/).

Kuyruklama sistemi daha sonra belirtilecek olan ilgili rapor endpointi üzerinden devreye girmektedir. Bu endpoint rapor taleplerini sırası ile kuyruğa almakta, kesinlikle veritabanında ekleme işlemi **yapmamamktadır**. Kuruğa alınan raporların son aşama olarak kayıt işleminin tamamlanması için ".Consumer" katmanındaki konsol uygulamasında consume edilmesi gerekir. Bu konsol uygulaması kısaca kuyruktaki rapor taleplerini sırasıyla, lokasyon kriterlerine bağlı kalarak kayıtlarını tamamlamaktadır. Bu sayede, ilgili rapor oluşturma endpointinin trafiği ne kadar yoğun olursa olsun, her işlem kuyruğa alınacak daha sonra kayıtları sağlıklı bir şekilde yapılacaktır.

Bu projenin canlı ortamda hizmet verdiği düşünüldüğünde, ".Consumer" ayrı bir proje olarak değerlendirilip web servis sağlıyıcılarında (AWS, Azure Webjob vs.) yayınlanabilir. Bu sayede mevcut endpointle consumer paralel bir şekilde hizmet verebilir.

Proje debug edilip sonuçları görülmek istendiğinde sadece ".Web" katmanının çalıştığı gözükecektir. Bu durumda rapor veya raporlar talep edildiğinde kayıt talebi alınıp kuyruğa aktarılacak, fakat kayıt sonlanmayacaktır. Ne zaman ."Consumer" katmanındaki konsol uygulaması debug edildiğinde kuyruktaki kayıt talepleri sırası ile ele alınacaktır. Bu işlemin paralel bir şekilde yürütülmesi isteniyorsa, çözüme sağ tıklanıp özelliklerinden her iki projeninde startının verilmesi gerekmektedir.


### Endpointler ve Örnek Payloadlar
Tasarlanan tüm endpointler ve varsa örnek payloadlar listelenmiştir.


### DirectoriesController
- [GET] https://localhost:44379/directories
  - Veritabanındaki tüm kişilerin kayıtlarını liste halinde döndürür.
  
- [GET] https://localhost:44379/directories/person/{id}
  - Veritabanına kaydı gerçekleştirilen kişinin verisini döndürür.

- [POST] https://localhost:44379/directories/person
  - Veritabanına kişi kaydı gerçekleştirir.
  - ```javascript
    {
      "firstName": "İsim",
      "lastName": "Soyisim",
      "CompanyName": "Şirket Adı"
      "contacts": [
        {
          "type": 0,
          "body" "+905558884411",
        },
        {
          "type": 2,
          "body" "Turkey, Izmir",
        }
      ]
    }
    ```
  - Payload içeresinde gönderilen "contacts" arrayi opsiyoneldir.
  - "type" bir enum veri tipine sahiptir. İlgili karşılıklar:
    ```javascript
      "Telefon": 0
      "E-Posta": 1
      "Lokasyon": 2
    ```
    olarak listelenmektedir.

- [DELETE] https://localhost:44379/directories/person/{id}
  - Kişinin kaydını siler.
  
- [POST] https://localhost:44379/directories/person/{id}/contact
  - Kişiye iletişim bilgisi ekler.
  - ```javascript
    {
      "type": 0,
      "body": "+9055588899966",
      "personId": "eaec0f02-35be-401f-a24a-30151f5f1c3e"
    }
    ```
  - Payload içeresinde gönderilen "personId" üyesi önemlidir. Routeda gönderilen "id" ile kordineli bir şekilde çalışır.
  
- [DELETE] https://localhost:44379/directories/contact/{id}
  - Kişiye ait iletişim bilgisini siler.


### ReportsController
- [GET] https://localhost:44379/reports
  - Veritabanındaki tüm raporların kayıtlarını liste halinde döndürür.
  
- [GET] https://localhost:44379/reports/{id}
  - Veritabanındaki ilgili raporun verisini döndürür.
  
- [POST] https://localhost:44379/reports
  - Kuyruğa rapor talebi gönderir. Kayıt ancak consume edildikten sonra tamamlanır.
  - ```javascript
    {
      "location": "Turkey, Izmir"
    }
    ```
