using System;

namespace API.RequestHelpers;

// The Pagination<T> class is a generic class that helps in returning paginated results from the server to the client.
// It is designed to handle paging for any kind of data (T represents the data type).
public class Pagination<T>(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
{
    // This class encapsulates the logic needed to implement server-side pagination,
    // making it easier for the client (UI) to handle paging when displaying large data sets.
    public int PageIndex { get; set; } = pageIndex;         // Current page index
    public int PageSize { get; set; } = pageSize;           // Number of items per page
    public int Count { get; set; } = count;                 // Total number of items
    public IReadOnlyList<T> Data { get; set; } = data;      // Paginated data of type T
}
