
namespace CodeBE_LEM.Entities
{
    public class Answer
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public long QuestionId { get; set; }

        public Question Question { get; set; } = null!;
    }
}
