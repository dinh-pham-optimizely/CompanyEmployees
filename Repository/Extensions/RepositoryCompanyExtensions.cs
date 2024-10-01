namespace Repository.Extensions;

public static class RepositoryCompanyExtensions
{
    public static IQueryable<Company> Search(this IQueryable<Company> companies, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return companies;
        }

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return companies.Where(c => c.Name.ToLower().Contains(lowerCaseTerm));
    }
}
