﻿namespace Presentation.WebApp.Models.Components;

public class ButtonModel
{
    public string? IconUri { get; set; }

    public string Title { get; set; } = null!;

    public bool DataModal { get; set; }
    public bool DataModalClose { get; set; }

    public string? DataTarget { get; set; }

    public string? Type { get; set; } = "button";

    public string? Name { get; set; }

    public string? Value { get; set; }
}
