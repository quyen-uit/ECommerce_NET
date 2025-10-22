using Core.Common.Specifications;
namespace Core.Specifications.Accounts
{
    public class RefreshTokenWithUserSpecification : BaseSpecification<RefreshToken>
    {
        public RefreshTokenWithUserSpecification(string? token)
            : base(x =>
                (!string.IsNullOrEmpty(token) && x.Token == token)
                && x.IsActive == true)
        {
            AddInclude(x => x.User);
        }
    }
}
