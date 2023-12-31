namespace DatabaseMapper.Core.Parser.Models
{
    public class EcalcFilter
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Alias { get; set; }

        public string DefaultValue { get; set; }

        public string Text { get; set; }

        public bool HasAlias => Alias != default;

        public bool HasDefaultValue => DefaultValue != default;
    }
}
