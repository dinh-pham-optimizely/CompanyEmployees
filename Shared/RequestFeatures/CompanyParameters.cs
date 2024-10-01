namespace Shared.RequestFeatures;

public class CompanyParameters : RequestParameters
{
    public CompanyParameters() => OrderBy = "name";

    public string? SearchTerm { get; set; }
}
