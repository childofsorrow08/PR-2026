namespace PostService.CommonTypes;

public class PostingValidationException(Dictionary<string, string[]> errors) : Exception("Дані відправлення некоректні.") {
    public Dictionary<string, string[]> Errors { get; } = errors;
}
