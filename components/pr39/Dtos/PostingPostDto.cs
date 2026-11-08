using PostService.CommonTypes;

namespace PostService.Dtos;

public class PostingPostDto {
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public string Content { get; set; } = "";
    public DeliveryType DeliveryType { get; set; }
    public float Weight { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }
    public float? Value { get; set; }
}
