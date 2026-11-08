public class Posting : Model {
    public string TrackingNumber { get; set; } = "";
    public double Weight { get; set; }
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public string Status { get; set; } = "";

    public override string ToString() {
        return $"{Id}: {TrackingNumber}, {Weight} кг, {From} → {To}, {Status}";
    }
}
