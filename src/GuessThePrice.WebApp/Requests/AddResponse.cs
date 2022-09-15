using GuessThePrice.Core.Model;

namespace GuessThePrice.WebApp.Requests;

public record PromotionalPriceResponseRequest(double Value);

public record AddResponseRequest(int ProductId, PromotionalPriceResponseRequest PromotionalPriceResponse);