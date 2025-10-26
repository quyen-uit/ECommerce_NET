using API.Commons.Response;
using API.Helpers;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    public class PaymentController : ApiControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration _config;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger, IConfiguration config)
        {
            _paymentService = paymentService;
            _logger = logger;
            _config = config;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<ApiSuccessResponse<CustomerBasket>>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            return Ok(ResponseFactory.Ok(basket));
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"].ToString();
            var whSecret = _config["StripeSettings:WebhookSecret"];
            if (string.IsNullOrWhiteSpace(whSecret))
            {
                _logger.LogError("Stripe webhook secret missing from configuration.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var stripeEvent = EventUtility.ConstructEvent(json, signature, whSecret);

            PaymentIntent intent;
            Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment successfully: {id}", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to received payment: {id}", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment successfully: {id}", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Order updated to failed payment: {id}", order.Id);
                    break;
            }

            return new EmptyResult();
        }
    }
}
