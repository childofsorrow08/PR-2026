using System.Globalization;
using Microsoft.Data.Sqlite;
using PostService.Models;

namespace PostService.DataAccess;

public class PostingRepository(string connection_string) : IPostingRepository {
    public void CreateDb() {
        using var connection = open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Postings (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                [From] TEXT NOT NULL, [To] TEXT NOT NULL, Content TEXT NOT NULL,
                DeliveryType INTEGER NOT NULL, Weight REAL NOT NULL,
                Width INTEGER NOT NULL, Height INTEGER NOT NULL, Depth INTEGER NOT NULL,
                Value REAL NULL, Price REAL NOT NULL, CreatedAt TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }

    public List<Posting> GetList() {
        using var connection = open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Postings ORDER BY Id";
        using var reader = command.ExecuteReader();
        List<Posting> postings = [];
        while (reader.Read()) postings.Add(read_posting(reader));
        return postings;
    }

    public Posting? GetById(int id) {
        using var connection = open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Postings WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        using var reader = command.ExecuteReader();
        return reader.Read() ? read_posting(reader) : null;
    }

    public int Create(Posting posting) {
        using var connection = open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Postings ([From], [To], Content, DeliveryType, Weight,
                Width, Height, Depth, Value, Price, CreatedAt)
            VALUES (@From, @To, @Content, @DeliveryType, @Weight,
                @Width, @Height, @Depth, @Value, @Price, @CreatedAt);
            SELECT last_insert_rowid();
            """;
        parameters(command, posting);
        return checked((int)(long)command.ExecuteScalar()!);
    }

    public int Update(Posting posting) {
        using var connection = open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Postings SET [From] = @From, [To] = @To, Content = @Content,
                DeliveryType = @DeliveryType, Weight = @Weight, Width = @Width,
                Height = @Height, Depth = @Depth, Value = @Value, Price = @Price
            WHERE Id = @Id;
            """;
        parameters(command, posting);
        return command.ExecuteNonQuery();
    }

    public int Delete(int id) {
        using var connection = open();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Postings WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        return command.ExecuteNonQuery();
    }

    private SqliteConnection open() {
        var connection = new SqliteConnection(connection_string);
        connection.Open();
        return connection;
    }

    private static void parameters(SqliteCommand command, Posting posting) {
        command.Parameters.AddWithValue("@Id", posting.Id);
        command.Parameters.AddWithValue("@From", posting.From);
        command.Parameters.AddWithValue("@To", posting.To);
        command.Parameters.AddWithValue("@Content", posting.Content);
        command.Parameters.AddWithValue("@DeliveryType", (int)posting.DeliveryType);
        command.Parameters.AddWithValue("@Weight", posting.Weight);
        command.Parameters.AddWithValue("@Width", posting.Width);
        command.Parameters.AddWithValue("@Height", posting.Height);
        command.Parameters.AddWithValue("@Depth", posting.Depth);
        command.Parameters.AddWithValue("@Value", (object?)posting.Value ?? DBNull.Value);
        command.Parameters.AddWithValue("@Price", posting.Price);
        command.Parameters.AddWithValue("@CreatedAt", posting.CreatedAt.ToString("O", CultureInfo.InvariantCulture));
    }

    private static Posting read_posting(SqliteDataReader reader) {
        int value = reader.GetOrdinal("Value");
        return new Posting {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            From = reader.GetString(reader.GetOrdinal("From")),
            To = reader.GetString(reader.GetOrdinal("To")),
            Content = reader.GetString(reader.GetOrdinal("Content")),
            DeliveryType = (PostService.CommonTypes.DeliveryType)reader.GetInt32(reader.GetOrdinal("DeliveryType")),
            Weight = reader.GetFloat(reader.GetOrdinal("Weight")),
            Width = reader.GetInt32(reader.GetOrdinal("Width")),
            Height = reader.GetInt32(reader.GetOrdinal("Height")),
            Depth = reader.GetInt32(reader.GetOrdinal("Depth")),
            Value = reader.IsDBNull(value) ? null : reader.GetFloat(value),
            Price = reader.GetFloat(reader.GetOrdinal("Price")),
            CreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedAt")), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };
    }
}
