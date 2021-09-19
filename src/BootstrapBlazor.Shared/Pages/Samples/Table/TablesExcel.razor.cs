﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BootstrapBlazor.Shared.Pages.Table
{
    /// <summary>
    /// 
    /// </summary>
    partial class TablesExcel
    {
        [NotNull]
        private DataTableDynamicContext? DataTableDynamicContext { get; set; }

        private DataTable UserData { get; } = new DataTable();

        [Inject]
        [NotNull]
        private IStringLocalizer<Foo>? Localizer { get; set; }

        [Inject]
        [NotNull]
        private IStringLocalizer<Tables>? TablesLocalizer { get; set; }

        private string? ButtonAddColumnText { get; set; }

        private string? ButtonRemoveColumnText { get; set; }

        [NotNull]
        private BlockLogger? Trace { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            ButtonAddColumnText ??= TablesLocalizer[nameof(ButtonAddColumnText)];
            ButtonRemoveColumnText ??= TablesLocalizer[nameof(ButtonRemoveColumnText)];

            // 初始化 DataTable
            InitDataTable();

            // 初始化 DataTableContext 绑定 Table 组件
            InitDataTableContext();
        }

        private void InitDataTable()
        {
            UserData.Columns.Add(new DataColumn(nameof(Foo.DateTime), typeof(DateTime)) { DefaultValue = DateTime.Now });
            UserData.Columns.Add(nameof(Foo.Name), typeof(string));
            UserData.Columns.Add(nameof(Foo.Complete), typeof(bool));
            UserData.Columns.Add(nameof(Foo.Education), typeof(string));
            UserData.Columns.Add(nameof(Foo.Count), typeof(int));

            Foo.GenerateFoo(Localizer, 10).ForEach(f =>
            {
                UserData.Rows.Add(f.DateTime, f.Name, f.Complete, f.Education, f.Count);
            });
        }

        private void InitDataTableContext()
        {
            DataTableDynamicContext = new(UserData);

            var method = DataTableDynamicContext.OnValueChanged;
            DataTableDynamicContext.OnValueChanged = async (model, col, val) =>
            {
                // 调用内部提供的方法
                if (method != null)
                {
                    // 内部方法会更新原始数据源 DataTable
                    await method(model, col, val);
                }

                // 输出日志信息
                Trace.Log($"FieldName: {col.GetFieldName()} - Value: {val?.ToString()}");
            };
        }
    }
}
