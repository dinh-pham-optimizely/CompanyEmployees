namespace Shared.DataTransferObjects;

[Serializable]
public record EmployeeDto(Guid id, string Name, int Age, string Position);
