using System.Reflection;
using Microsoft.OpenApi.Models;
using PostService.Dtos;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Title = "PostService",
        Version = "v1",
        Description = "Навчальний API поштових відправлень на основі ПР-37."
    });

    string xml = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    options.IncludeXmlComments(xml);
    options.UseAllOfToExtendReferenceSchemas();
    options.OperationFilter<ParameterDescriptions>();
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins("http://localhost:3000", "http://localhost:60648")
    .AllowAnyHeader()
    .AllowAnyMethod()
));

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

object sync = new();
int next_id = 3;

var postings = new List<PostingGetDto> {
    new() {
        Id = 1,
        From = "Київ",
        To = "Львів",
        Content = "Книги",
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
        Weight = 1,
        Width = 20,
        Height = 5,
        Depth = 30,
        Value = 100,
        Price = 80,
        CreatedAt = DateTime.UtcNow
    }
};

app.MapGet("/", () => "Це API поштового клієнта.")
    .WithName("ApiInfo")
    .WithSummary("Інформація про API")
    .WithDescription("Повертає текстове призначення сервісу.")
    .Produces<string>();

app.MapGet("/postings", () => {
    lock (sync) return Results.Ok(postings.ToArray());
})
    .WithName("GetPostings")
    .WithSummary("Список відправлень")
    .WithDescription("Повертає всі записи зі сховища поточного процесу.")
    .Produces<PostingGetDto[]>();

app.MapGet("/postings/{id:int}", (int id) => {
    lock (sync) {
        var posting = postings.Find(item => item.Id == id);
        if (posting is null) return Results.NotFound();
        return Results.Ok(posting);
    }
})
    .WithName("FindPosting")
    .WithSummary("Пошук відправлення")
    .WithDescription("Повертає запис із заданим ідентифікатором або 404.")
    .Produces<PostingGetDto>()
    .Produces(404);

app.MapPost("/postings", (PostingPostDto dto) => {
    lock (sync) {
        var posting = map_posting(dto, next_id++, DateTime.UtcNow);
        postings.Add(posting);
        return Results.Created($"/postings/{posting.Id}", posting);
    }
})
    .WithName("CreatePosting")
    .WithSummary("Створення відправлення")
    .WithDescription("Приймає дані без id і CreatedAt. Сервер призначає унікальний id та дату створення.")
    .Accepts<PostingPostDto>("application/json")
    .Produces<PostingGetDto>(201)
    .Produces(400);

app.MapPut("/postings/{id:int}", (int id, PostingPutDto dto) => {
    if (id != dto.Id) return Results.BadRequest(new { message = "Ідентифікатори не збігаються." });

    lock (sync) {
        int index = postings.FindIndex(item => item.Id == id);
        if (index < 0) return Results.NotFound();

        var posting = map_posting(dto, id, postings[index].CreatedAt);
        postings[index] = posting;
        return Results.Ok(posting);
    }
})
    .WithName("UpdatePosting")
    .WithSummary("Оновлення відправлення")
    .WithDescription("Замінює всі змінювані поля; id тіла має збігатися з маршрутом, дата створення зберігається.")
    .Accepts<PostingPutDto>("application/json")
    .Produces<PostingGetDto>()
    .Produces(400)
    .Produces(404);

app.MapDelete("/postings/{id:int}", (int id) => {
    lock (sync) {
        if (postings.RemoveAll(item => item.Id == id) == 0) return Results.NotFound();
        return Results.NoContent();
    }
})
    .WithName("DeletePosting")
    .WithSummary("Видалення відправлення")
    .WithDescription("Видаляє запис за id; успішна відповідь 204 не містить тіла.")
    .Produces(204)
    .Produces(404);

app.Run();

static PostingGetDto map_posting(PostingPostDto dto, int id, DateTime created_at) {
    return new PostingGetDto {
        Id = id,
        CreatedAt = created_at,
        From = dto.From,
        To = dto.To,
        Content = dto.Content,
        DeliveryType = dto.DeliveryType,
        Weight = dto.Weight,
        Width = dto.Width,
        Height = dto.Height,
        Depth = dto.Depth,
        Value = dto.Value,
        Price = dto.Price
    };
}
