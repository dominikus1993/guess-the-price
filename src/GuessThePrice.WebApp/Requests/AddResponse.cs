using GuessThePrice.Core.Model;

namespace GuessThePrice.WebApp.Requests;

public record PromotionalPriceResponseRequest(decimal Value);

public record AddResponseRequest(int ProductId, PromotionalPriceResponseRequest PromotionalPriceResponse);