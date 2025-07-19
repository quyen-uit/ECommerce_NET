using Core.Common.Specifications;
namespace Core.Specifications.Accounts
{
    public class RefreshTokenSpecification : BaseSpecification<RefreshToken>
    {
        public RefreshTokenSpecification(string userId, DateTime? revokedAt = null)
            : base(x =>
                (string.IsNullOrEmpty(userId) || x.UserId.ToLower().Contains(userId.ToLower()))
                && x.RevokedAt == revokedAt)
        {
        }
    }
}