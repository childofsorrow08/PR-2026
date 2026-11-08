using Microsoft.Data.Sqlite;
using PostService.DataAccess;
using PostService.CommonTypes;
using PostService.Dtos;
using PostService.Mappings;
using PostService.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);
const string database_name = "postings.db";
string database = builder.Configuration["DatabasePath"] ?? Path.Combine(builder.Environment.ContentRootPath, database_name);
string connection_string = new SqliteConnectionStringBuilder { DataSource = database, DefaultTimeout = 30 }.ToString();
builder.Services.AddScoped<IPostingRepository>(_ => new PostingRepository(connection_string));
builder.Services.AddScoped<IPostingService, PostingService>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins("http://localhost:3000", "http://localhost:60648")
    .AllowAnyHeader()
    .AllowAnyMethod()
));

var app = builder.Build();
using (var scope = app.Services.CreateScope()) {
    ((PostingRepository)scope.ServiceProvider.GetRequiredService<IPostingRepository>()).CreateDb();
}
app.UseCors();

app.Use(async (context, next) => {
    try {
        await next(context);
    } catch (PostingValidationException error) {
        await Results.ValidationProblem(error.Errors).ExecuteAsync(context);
    }
});

app.MapGet("/", () => "Це API поштового клієнта.");

app.MapGet("/postings", (IPostingService service) =>
    Results.Ok(service.GetAll().Select(PostingMapper.ToPostingGetDto)));

app.MapGet("/postings/{id:int}", (int id, IPostingService service) => {
    var posting = service.Find(id);
    if (posting is null) return Results.NotFound();
    return Results.Ok(PostingMapper.ToPostingGetDto(posting));
});

app.MapPost("/postings", (PostingPostDto dto, IPostingService service) => {
    var posting = service.Create(PostingMapper.ToPosting(dto));
    return Results.Created($"/postings/{posting.Id}", PostingMapper.ToPostingGetDto(posting));
});

app.MapPut("/postings/{id:int}", (int id, PostingPutDto dto, IPostingService service) => {
    if (id != dto.Id) return Results.BadRequest(new { message = "Ідентифікатори не збігаються." });
    var posting = service.Update(PostingMapper.ToPosting(dto));
    if (posting is null) return Results.NotFound();
    return Results.Ok(PostingMapper.ToPostingGetDto(posting));
});

app.MapDelete("/postings/{id:int}", (int id, IPostingService service) =>
    service.Delete(id) == 0 ? Results.NotFound() : Results.NoContent());

app.Run();
