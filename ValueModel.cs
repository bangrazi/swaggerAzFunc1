using System;

namespace swaggerAzFunc1
{
    public class ValueModel
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Value { get; set; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}