namespace FluentArgs.Description
{
    internal class Flag
    {
        public Flag(Name name)
        {
            Name = name;
        }

        public string? Description { get; set; }

        public Name Name { get; }
    }
}
