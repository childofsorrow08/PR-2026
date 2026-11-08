namespace PostService.Dtos;

/// <summary>Відправлення у відповіді сервера.</summary>
public class PostingGetDto : PostingPutDto {
    /// <summary>Дата та час створення в UTC; встановлюється сервером.</summary>
    public DateTime CreatedAt { get; set; }
}
