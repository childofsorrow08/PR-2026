namespace PostService.Dtos;

/// <summary>Повний новий стан відправлення для оновлення.</summary>
public class PostingPutDto : PostingPostDto {
    /// <summary>Ідентифікатор запису; має збігатися з id у маршруті.</summary>
    public int Id { get; set; }
}
