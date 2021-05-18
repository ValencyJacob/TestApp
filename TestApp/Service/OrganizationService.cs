using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestApp.Models;

namespace TestApp.Services
{
    public class OrganizationService
    {
        IGridFSBucket gridFS;   // файловое хранилище
        IMongoCollection<Organization> Organizations; // коллекция в базе данных

        public OrganizationService(IDatabaseSettings settings)
        {
            // строка подключения
            //var connection = new MongoUrlBuilder(settings.ConnectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(settings.ConnectionString);
            // получаем доступ к самой базе данных
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            // получаем доступ к файловому хранилищу
            gridFS = new GridFSBucket(database);
            // обращаемся к коллекции Products
            Organizations = database.GetCollection<Organization>("Organizations");
        }

        // получаем все документы
        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            return await Organizations.Find(items => true).ToListAsync();
        }

        // получаем один документ по id
        public async Task<Organization> GetOrganization(string id)
        {
            return await Organizations.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        // добавление документа
        public async Task Create(Organization p)
        {
            await Organizations.InsertOneAsync(p);
        }

        // обновление документа
        public async Task Update(Organization p)
        {
            await Organizations.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(p.Id)), p);
        }

        // удаление документа
        public async Task Remove(string id)
        {
            await Organizations.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }

        // получение изображения
        public async Task<byte[]> GetImage(string id)
        {
            return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
        }

        // сохранение изображения
        public async Task StoreImage(string id, Stream imageStream, string imageName)
        {
            Organization p = await GetOrganization(id);
            if (p.HasImage())
            {
                // если ранее уже была прикреплена картинка, удаляем ее
                await gridFS.DeleteAsync(new ObjectId(p.PhotoId));
            }
            // сохраняем изображение
            ObjectId imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
            // обновляем данные по документу
            p.PhotoId = imageId.ToString();
            var filter = Builders<Organization>.Filter.Eq("_id", new ObjectId(p.Id));
            var update = Builders<Organization>.Update.Set("PhotoId", p.PhotoId);
            await Organizations.UpdateOneAsync(filter, update);
        }
    }
}
