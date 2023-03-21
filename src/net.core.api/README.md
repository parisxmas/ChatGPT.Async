# Çalıştırma

- Projeyi klonlayın

```bash
  $ git clone https://github.com/parisxmas/ChatGPT.Async.git
```

- Projenin ana dizinine gidin

- Eğer lokalinizde çalışan bir ElasticSearch ve Kibana varsa bu kısmı atlayabilirsiniz. Elastic Search farklı bir sunucuda çalışıyorsa ```net.core.api\net.core.api\appsettings.json``` dosyası altındaki ```ElasticConfiguration``` altındaki ```Uri``` değerini değiştirmelisiniz. ElasticSearch ve Kibana uygulamalarını Docker'da ayağa kaldırmak için aşağıdaki komutu çalıştırın:
```bash
  $ docker-compose up -d
```

- Eğer lokalinizde çalışan bir redis varsa bu kısmı atlayabilirsiniz. Redis farklı bir sunucuda çalışıyorsa ```net.core.api\net.core.api\appsettings.json``` dosyası altındaki ```RedisConfiguration``` altındaki ```Uri``` değerini değiştirmelisiniz. Redis sunucunuz yoksa ve Docker'ınız kuruluysa aşağıdaki komutu çalıştırarak Redis'i Docker yardımıyla ayağa kaldırabilirsiniz:
```bash
  $ docker run --name my-redis -p 6379:6379 -d redis
```

- Projeyi  çalıştırın:

```bash
  $ dotnet run
```
