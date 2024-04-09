namespace CodeBE_LEM.Common
{
    public class FilterDTO
    {
        public List<long>? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public FilterOrderBy? OrderBy { get; set; }
        public FilterOrderType? OrderType { get; set; }
        public FilterSearch? Search { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? Pinned { get; set; }
        public bool? IsNotification { get; set; }
    }

    public enum FilterOrderBy
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }

    public enum FilterOrderType
    {
        ASC = 0,
        DESC = 1,
    }

    public enum FilterSearch
    {
        ALL = 0,
        Code = 1,
        Name = 2,
    }
}
