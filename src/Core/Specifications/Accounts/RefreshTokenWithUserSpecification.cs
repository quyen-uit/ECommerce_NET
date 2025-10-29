using Ardalis.Specification;
namespace Core.Specifications.Accounts
{
    public class RefreshTokenWithUserSpecification : Specification<RefreshToken>
    {
        public RefreshTokenWithUserSpecification(string? token)
        {
            Query.Where(x => (!string.IsNullOrEmpty(token) && x.Token == token) && x.IsActive == true)
                 .Include(x => x.User);
        }
    }
}
