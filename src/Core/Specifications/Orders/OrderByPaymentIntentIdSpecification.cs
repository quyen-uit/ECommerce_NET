using Ardalis.Specification;
using Core.Entities.OrderAggregate;


namespace Core.Specifications.Orders
{
    public class OrderByPaymentIntentIdSpecification : Specification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentId)
        {
            Query.Where(x => x.PaymentIntentId == paymentId);
        }
    }
}
