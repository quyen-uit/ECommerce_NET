using Ardalis.Specification;
namespace Core.Specifications.Accounts
{
    public class RefreshTokenSpecification : Specification<RefreshToken>
    {
        public RefreshTokenSpecification(string userId, DateTime? revokedAt = null)
        {
            Query.Where(x => (!string.IsNullOrEmpty(userId) && x.UserId == userId)
                              && x.RevokedAt == revokedAt);
        }
    }
}
