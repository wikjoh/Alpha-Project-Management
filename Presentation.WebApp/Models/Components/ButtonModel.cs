namespace Presentation.WebApp.Models.Components;

public class ButtonModel
{
    public string? IconUri { get; set; }

    public string Title { get; set; } = null!;

    public bool DataModal { get; set; }

    public string? DataTarget { get; set; }

    public string? Type { get; set; } = "button";
}
