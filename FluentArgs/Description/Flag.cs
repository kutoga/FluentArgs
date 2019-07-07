namespace FluentArgs.Description
{
    internal class Flag
    {
        public Flag(Name name, string? description = null)
        {
            Description = description;
            Name = name;
        }

        public string? Description { get; set; }

        public Name Name { get; }
    }
}
