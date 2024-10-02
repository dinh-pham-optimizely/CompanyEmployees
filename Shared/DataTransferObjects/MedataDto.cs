namespace Shared.DataTransferObjects;

public record MedataDataDto(int CurrentPage, int TotalPages, int PageSize, int TotalCount, bool HasPrevious, bool HasNext);
