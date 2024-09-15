namespace Core.Specifications;

// The ProductSpecParams class holds query parameters for filtering, sorting,
// and paginating products in a specification-based approach. This class helps control how a user
// can search, filter, and paginate through products in the database.
public class ProductSpacParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;     // Limit page size to MaxPageSize
    }


    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands;             // types=boards,gloves => if we select many brands // Comma-separated brand values
        set
        {
            _brands = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList();       // Converts comma-separated string into a list of product types
        }
    }

    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;              // types=abd,xyz => if we select many types
        set
        {
            _types = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList();        // Converts comma-separated string into a list of product types
        }
    }

    public string? Sort { get; set; }

    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();       // Convert search query to lowercase for case-insensitive searching
    }
}
