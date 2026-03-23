namespace ProjectManagement.Api.Models;

public class PortalContentSection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortalDesignId { get; set; }
    public PortalDesign? PortalDesign { get; set; }
    public string SectionKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
