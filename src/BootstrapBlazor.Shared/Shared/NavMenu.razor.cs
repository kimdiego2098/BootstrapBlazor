﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.Extensions.Options;
using System;

namespace BootstrapBlazor.Shared.Shared;

/// <summary>
///
/// </summary>
[JSModuleAutoLoader("menu", ModuleName = "Menu")]
public partial class NavMenu
{
    private bool IsAccordion { get; set; }

    private bool IsExpandAll { get; set; }

    [NotNull]
    private string? AccordionText { get; set; }

    [NotNull]
    private string? ExpandAllText { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<App>? AppLocalizer { get; set; }

    [Inject]
    [NotNull]
    private TitleService? TitleService { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<NavMenu>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private IOptionsMonitor<WebsiteOptions>? WebsiteOption { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        AccordionText ??= Localizer["MenuAccordion"];
        ExpandAllText ??= Localizer["MenuExpandAll"];
    }

    private Task OnValueChanged(bool accordion)
    {
        if (accordion)
        {
            IsExpandAll = false;
        }
        return Task.CompletedTask;
    }

    private async Task OnClickMenu(MenuItem item)
    {
        if (!item.Items.Any() && !string.IsNullOrEmpty(item.Text))
        {
            await TitleService.SetTitle($"{item.Text} - {AppLocalizer["Title"]}");
        }
    }
}
