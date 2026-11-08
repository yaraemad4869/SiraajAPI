using Microsoft.Data.SqlClient;
using System;

var connectionString = "Server=localhost;Database=SiraajDb;Trusted_Connection=True;TrustServerCertificate=True;";

try
{
    using var connection = new SqlConnection(connectionString);
    connection.Open();
    
    var query = @"
        SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
        FROM INFORMATION_SCHEMA.COLUMNS 
        WHERE TABLE_NAME = 'Packages' 
        AND COLUMN_NAME IN ('Price', 'PriceUsd', 'OriginalPrice', 'OriginalPriceUsd', 'DefaultCurrency')
        ORDER BY COLUMN_NAME";
    
    using var command = new SqlCommand(query, connection);
    using var reader = command.ExecuteReader();
    
    Console.WriteLine("\n=== Packages Table Columns ===");
    Console.WriteLine($"{"Column",-25} {"Type",-15} {"Nullable",-10}");
    Console.WriteLine(new string('-', 55));
    
    while (reader.Read())
    {
        var column = reader.GetString(0);
        var type = reader.GetString(1);
        var nullable = reader.GetString(2);
        Console.WriteLine($"{column,-25} {type,-15} {nullable,-10}");
    }
    
    connection.Close();
    Console.WriteLine("\n✅ Database connection successful");
}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ Error: {ex.Message}");
}
