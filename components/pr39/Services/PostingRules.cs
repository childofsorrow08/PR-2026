using PostService.CommonTypes;
using PostService.Models;

namespace PostService.Services;

public static class PostingRules {
    public static void Validate(Posting posting) {
        Dictionary<string, string[]> errors = [];

        if (string.IsNullOrWhiteSpace(posting.From) || posting.From.Trim().Length < 2)
            errors["from"] = ["Відправник: щонайменше 2 символи."];
        if (string.IsNullOrWhiteSpace(posting.To) || posting.To.Trim().Length < 2)
            errors["to"] = ["Одержувач: щонайменше 2 символи."];
        if (string.IsNullOrWhiteSpace(posting.Content)) errors["content"] = ["Укажіть вміст відправлення."];
        if (!Enum.IsDefined(posting.DeliveryType)) errors["deliveryType"] = ["Оберіть спосіб доставки."];
        if (!float.IsFinite(posting.Weight) || posting.Weight <= 0)
            errors["weight"] = ["Вага має бути додатним числом."];
        else if (posting.DeliveryType == DeliveryType.Department && posting.Weight > 20)
            errors["weight"] = ["Для відділення максимальна вага — 20 кг."];

        if (posting.Width < 0) errors["width"] = ["Ширина не може бути від'ємною."];
        if (posting.Height < 0) errors["height"] = ["Висота не може бути від'ємною."];
        if (posting.Depth < 0) errors["depth"] = ["Довжина не може бути від'ємною."];
        if (posting.Value is float value && (!float.IsFinite(value) || value < 0))
            errors["value"] = ["Оголошена вартість має бути невід'ємним числом."];

        if (errors.Count > 0) throw new PostingValidationException(errors);
    }

    public static float CalculatePrice(Posting posting) {
        int rate = posting.DeliveryType switch {
            DeliveryType.Department => 15,
            DeliveryType.Courier => 30,
            DeliveryType.ExpressCourier => 55,
            _ => throw new ArgumentOutOfRangeException(nameof(posting.DeliveryType))
        };

        double price = Math.Round((double)posting.Weight * rate, 2, MidpointRounding.AwayFromZero);
        if (price > float.MaxValue) {
            throw new PostingValidationException(new() { ["weight"] = ["Вага завелика для розрахунку вартості."] });
        }
        return (float)price;
    }
}
