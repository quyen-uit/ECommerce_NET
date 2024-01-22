using Core.Common;
using Core.Entities.OrderAggregate;


namespace Core.Specifications.Orders
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentId) : base(x => x.PaymentIntentId == paymentId)
        {
        }
    }
}
