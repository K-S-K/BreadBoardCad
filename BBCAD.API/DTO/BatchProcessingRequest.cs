namespace BBCAD.API.DTO
{
    public class BatchProcessingRequest
    {
        public Guid? Id { get; set; }
        public string Script { get; set; } = null!;
    }
}
