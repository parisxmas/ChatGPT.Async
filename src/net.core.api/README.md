
Clone the project by running the following command:
```bash
$ git clone https://github.com/parisxmas/ChatGPT.Async.git
```
Navigate to the main directory of the project and run the seed.sql file in the SQL folder on your database.

If you have a local or remote ElasticSearch and Kibana running, you can skip this part. If ElasticSearch is running on a different server, you need to modify the Uri value under ElasticConfiguration in the net.core.api\net.core.api\appsettings.json file. To start the ElasticSearch and Kibana applications on Docker, run the following command:
```bash
$ docker-compose up -d
```

If you have a local or remote Redis running, you can skip this part. If Redis is running on a different server, you need to modify the Uri value under RedisConfiguration in the net.core.api\net.core.api\appsettings.json file. If you do not have a Redis server and Docker is installed on your machine, you can start Redis with Docker by running the following command:
```bash
$ docker run --name my-redis -p 6379:6379 -d redis
```
To run the project:
```bash
$ dotnet run
```
