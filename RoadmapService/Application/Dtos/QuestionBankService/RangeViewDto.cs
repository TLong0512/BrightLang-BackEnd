namespace Application.Dtos.QuestionBankService;

public class RangeViewDto
{
    // DO NOT TOUCH
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StartQuestionNumber { get; set; }
    public int EndQuestionNumber { get; set; }
}
