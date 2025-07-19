using Core.Common.Specifications;
using Core.Enums;
namespace Core.Specifications.Accounts
{
    public class RefreshTokenWithUserSpecification : BaseSpecification<RefreshToken>
    {
        public RefreshTokenWithUserSpecification(string? token)
            : base(x =>
                (string.IsNullOrEmpty(token) || x.Token.ToLower().Contains(token.ToLower()))
                && x.IsActive == true)
        {
            AddInclude(x => x.User);
        }
    }
}