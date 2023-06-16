using Dapper;
using SimApi.Base;
using SimApi.Data.Context;

namespace SimApi.Data.Repository;

public class DapperRepository<Entity> : IDapperRepository<Entity> where Entity : BaseModel
{
    protected readonly SimDapperDbContext context;
    private bool disposed;

    public DapperRepository(SimDapperDbContext context)
    {
        this.context = context;
    }
    public void DeleteById(int id)
    {
        var sql = $"DELETE FROM dbo.\"{typeof(Entity).Name}\" WHERE Id = @Id";
        using (var connection = context.CreateConnection())
        {
            connection.Open();
            connection.Execute(sql, new { Id = id });
            connection.Close();
        }
    }

    public List<Entity> Filter(string sql)
    {
        throw new NotImplementedException();
    }

    //public List<Entity> Filter(string sql)
    //{
    //    throw new NotImplementedException();
    //}

    public List<Entity> GetAll()
    {
        var sql = $"SELECT * FROM dbo.\"{typeof(Entity).Name}\"";
        using (var connection = context.CreateConnection())
        {
            connection.Open();
            var result = connection.Query<Entity>(sql);
            connection.Close();
            return result.ToList();
        }
    }

    public Entity GetById(int id)
    {
        var sql = $"SELECT * FROM dbo.\"{typeof(Entity).Name}\" WHERE \"Id\"=@Id";
        using (var connection = context.CreateConnection())
        {
            connection.Open();
            var result = connection.QueryFirst<Entity>(sql, new { id });
            connection.Close();
            return result;
        }
    }

    public void Insert(Entity entity)
    {
        var entityType = typeof(Entity);
        var properties = entityType.GetProperties();
        var columnNames = properties.Select(property => property.Name);
        var parameterNames = properties.Select(property => $"@{property.Name}");
        var sql = $"INSERT INTO dbo.\"{entityType.Name}\" ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", parameterNames)})";
        using (var connection = context.CreateConnection())
        {
            connection.Open();
            connection.Execute(sql, entity);
            connection.Close();
        }
    }

    public void Update(Entity entity)
    {
        var entityType = typeof(Entity);
        var properties = entityType.GetProperties();
        var columns = properties.Select(property => $"{property.Name} = @{property.Name}");
        var sql = $"UPDATE dbo.\"{entityType.Name}\" SET {string.Join(", ", columns)} WHERE Id = @Id";
        using (var connection = context.CreateConnection())
        {
            connection.Open();
            connection.Execute(sql, entity);
            connection.Close();
        }
    }
}
