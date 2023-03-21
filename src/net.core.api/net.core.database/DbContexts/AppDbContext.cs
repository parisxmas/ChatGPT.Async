using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using net.core.data.POCOs;

namespace net.core.database.DbContexts;

public class AppDbContext:DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    
    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("MSSQL");
    }
    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}