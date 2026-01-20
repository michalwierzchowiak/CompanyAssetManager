namespace Core.Events
{
    public record ResourceCreated
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }

    public record ResourceUpdated
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public byte[] RowVersion { get; init; }
    }
}