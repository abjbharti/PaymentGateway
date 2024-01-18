namespace PaymentGate.Models
{
    public class SessionCreateOptions
    {
        public List<string>? PaymentMethodTypes { get; set; }
        public List<SessionLineItemOptions>? LineItems { get; set; }
        public string? Mode { get; set; }
        public string? SuccessUrl { get; set; }
        public string? CancelUrl { get; set; }
    }

    public class SessionLineItemOptions
    {
        public SessionLineItemPriceDataOptions? PriceData { get; set; }
        public int Quantity { get; set; }
    }

    public class SessionLineItemPriceDataOptions
    {
        public long? UnitAmount { get; set; }
        public string? Currency { get; set; }
        public SessionLineItemPriceDataProductDataOptions? ProductData { get; set; }
    }

    public class SessionLineItemPriceDataProductDataOptions
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
