using static DatabaseMapper.Core.Parser.Enums.FilterTypeEnum;

namespace DatabaseMapper.Core.Parser.Models
{
    public class Filter
    {
        public string Name { get; set; }

        public FilterType Type { get; set; }

        public string Alias { get; set; }

        public FilterValue DefaultValue { get; set; }

        public string Text { get; set; }

        public bool IsDetail { get; set; }

        public bool HasAlias => Alias != default;

        public bool HasDefaultValue => DefaultValue != null;

    }
}
