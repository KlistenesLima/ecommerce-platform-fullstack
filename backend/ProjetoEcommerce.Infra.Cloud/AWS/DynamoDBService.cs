using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.Cloud.AWS
{
    public interface IDynamoDBService
    {
        Task SaveAsync<T>(string tableName, string id, T data);
        Task<T> GetAsync<T>(string tableName, string id);
        Task<bool> DeleteAsync(string tableName, string id);
    }

    public class DynamoDBService : IDynamoDBService
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public DynamoDBService(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task SaveAsync<T>(string tableName, string id, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var item = new Dictionary<string, AttributeValue>
            {
                { "id", new AttributeValue { S = id } },
                { "data", new AttributeValue { S = json } },
                { "createdAt", new AttributeValue { S = DateTime.UtcNow.ToString("O") } }
            };

            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = item
            };

            await _dynamoDbClient.PutItemAsync(request);
        }

        public async Task<T> GetAsync<T>(string tableName, string id)
        {
            var request = new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "id", new AttributeValue { S = id } }
                }
            };

            var response = await _dynamoDbClient.GetItemAsync(request);

            if (!response.Item.ContainsKey("data"))
                return default;

            var json = response.Item["data"].S;
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task<bool> DeleteAsync(string tableName, string id)
        {
            var request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "id", new AttributeValue { S = id } }
                }
            };

            await _dynamoDbClient.DeleteItemAsync(request);
            return true;
        }
    }
}
