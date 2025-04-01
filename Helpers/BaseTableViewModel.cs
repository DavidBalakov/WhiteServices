namespace Diploma.Helpers;

public class BaseTableViewModel
{
    public int Start {  get; set; }
    public int Length { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public int RecordsTotal { get; set; }
    public int RecordsFiltered {  get; set; }
    public int TotalPages {  get; set; }
    public string? SortOrder {  get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
