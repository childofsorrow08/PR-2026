namespace PostService.Dtos;

/// <summary>Спосіб доставки поштового відправлення.</summary>
public enum DeliveryType {
    /// <summary>Доставка у відділення.</summary>
    Department,
    /// <summary>Кур'єрська доставка.</summary>
    Courier,
    /// <summary>Експрес-доставка кур'єром.</summary>
    ExpressCourier
}
