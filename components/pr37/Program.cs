using PostService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins("http://localhost:3000", "http://localhost:60648")
    .AllowAnyHeader()
    .AllowAnyMethod()
));

var app = builder.Build();
app.UseCors();

object sync = new();
int next_id = 3;

var postings = new List<Posting> {
    new() {
        Id = 1,
        From = "Київ",
        To = "Львів",
        Content = "Книги",
        DeliveryType = DeliveryType.Department,
        Weight = 2,
        Width = 20,
        Height = 10,
        Depth = 30,
        Value = 500,
        Price = 100,
        CreatedAt = DateTime.UtcNow
    },
    new() {
        Id = 2,
        From = "Рівне",
        To = "Луцьк",
        Content = "Документи",
        DeliveryType = DeliveryType.Courier,
        Weight = 1,
        Width = 20,
        Height = 5,
        Depth = 30,
        Value = 100,
        Price = 80,
        CreatedAt = DateTime.UtcNow
    }
};

app.MapGet("/", () => "Це API поштового клієнта.");

app.MapGet("/postings", () => {
    lock (sync) return Results.Ok(postings.ToArray());
});

app.MapGet("/postings/{id:int}", (int id) => {
    lock (sync) {
        Posting? posting = postings.Find(item => item.Id == id);
        if (posting is null) return Results.NotFound();
        return Results.Ok(posting);
    }
});

app.MapPost("/postings", (Posting posting) => {
    lock (sync) {
        posting.Id = next_id++;
        posting.CreatedAt = DateTime.UtcNow;
        postings.Add(posting);
        return Results.Created($"/postings/{posting.Id}", posting);
    }
});

app.MapPut("/postings/{id:int}", (int id, Posting updated) => {
    if (id != updated.Id) return Results.BadRequest(new { message = "Ідентифікатори не збігаються." });

    lock (sync) {
        int index = postings.FindIndex(item => item.Id == id);
        if (index < 0) return Results.NotFound();

        updated.CreatedAt = postings[index].CreatedAt;
        postings[index] = updated;
        return Results.Ok(updated);
    }
});

app.MapDelete("/postings/{id:int}", (int id) => {
    lock (sync) {
        if (postings.RemoveAll(item => item.Id == id) == 0) return Results.NotFound();
        return Results.NoContent();
    }
});

app.Run();
