namespace ProjectManagement.Api.Models;

public class PortalNavigationItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortalDesignId { get; set; }
    public PortalDesign? PortalDesign { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Href { get; set; } = "#";
    public int SortOrder { get; set; }
    public bool OpenInNewTab { get; set; }
}
