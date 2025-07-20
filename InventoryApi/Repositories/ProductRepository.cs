using Dapper;
using InventoryApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryApi.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "SELECT Id, Name, CategoryId, SupplierId, Price, Quantity FROM Product";

            var products = await connection.QueryAsync<Product>(sql);

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "SELECT Id, Name, CategoryId, SupplierId, Price, Quantity FROM Product WHERE Id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"INSERT INTO Product (Name, CategoryId, SupplierId, Price, Quantity)
                        VALUES (@Name, @CategoryId, @SupplierId, @Price, @Quantity);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await connection.QuerySingleAsync<int>(sql, product);
            return newId;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"UPDATE Product SET
                            Name = @Name,
                            CategoryId = @CategoryId,
                            SupplierId = @SupplierId,
                            Price = @Price,
                            Quantity = @Quantity
                        WHERE Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, product);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "DELETE FROM Product WHERE Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}
