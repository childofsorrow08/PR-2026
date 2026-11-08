namespace PostService.Dtos;

/// <summary>Дані для створення відправлення. Ціну задає клієнт у версії API з ПР-37.</summary>
public class PostingPostDto {
    /// <summary>Відправник або адреса відправлення.</summary>
    public string From { get; set; } = "";
    /// <summary>Одержувач або адреса призначення.</summary>
    public string To { get; set; } = "";
    /// <summary>Опис вмісту посилки.</summary>
    public string Content { get; set; } = "";
    /// <summary>Тип доставки: 0 — відділення, 1 — кур'єр, 2 — експрес.</summary>
    public DeliveryType DeliveryType { get; set; }
    /// <summary>Фактична вага в кілограмах.</summary>
    public float Weight { get; set; }
    /// <summary>Ширина в сантиметрах.</summary>
    public float Width { get; set; }
    /// <summary>Висота в сантиметрах.</summary>
    public float Height { get; set; }
    /// <summary>Довжина в сантиметрах.</summary>
    public float Depth { get; set; }
    /// <summary>Оголошена цінність у гривнях.</summary>
    public float Value { get; set; }
    /// <summary>Вартість пересилання у гривнях.</summary>
    public float Price { get; set; }
}
