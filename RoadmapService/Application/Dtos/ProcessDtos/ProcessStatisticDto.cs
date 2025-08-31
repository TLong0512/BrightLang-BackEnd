namespace Application.Dtos.ProcessDtos;

public class ProcessStatisticDto
{
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public int FinishedProcessCount { get; set; }
    public int OpenedProcessCount { get; set; }
    public int AllProcessCount { get; set; }
}
