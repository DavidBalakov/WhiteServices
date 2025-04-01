namespace Diploma.DTO
{
    public class SearchResult<T>
    {
        public int RecordsTotal {  get; set; }
        public int RecordsFiltered { get; set; }
        public int Start { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; }
    }
}
