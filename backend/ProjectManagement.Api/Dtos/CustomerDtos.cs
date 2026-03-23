namespace ProjectManagement.Api.Dtos;

public record CustomerSummaryResponse(string CustomerCode, int TotalUsers, int PortalEditors, int ActiveUsers, DateTime? LastPortalUpdateUtc);
