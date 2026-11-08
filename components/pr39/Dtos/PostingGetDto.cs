namespace PostService.Dtos;

public class PostingGetDto : PostingPutDto {
    public float Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
