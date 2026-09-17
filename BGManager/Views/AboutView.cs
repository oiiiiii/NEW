using System.Reflection;
using System.IO;

namespace BGManager.Views;

public class AboutView : UserControl
{
    public AboutView()
    {
        Dock = DockStyle.Fill;
        BackColor = Color.FromArgb(249, 250, 251);
        BuildUI();
    }

    private void BuildUI()
    {
        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(249, 250, 251)
        };
        Controls.Add(mainPanel);

        var card = new Panel
        {
            Width = 720,
            Height = 620,
            BackColor = Color.White,
            Padding = new Padding(50),
            BorderStyle = BorderStyle.FixedSingle
        };
        card.Left = (mainPanel.Width - card.Width) / 2;
        card.Top = (mainPanel.Height - card.Height) / 2;
        mainPanel.Controls.Add(card);

        var headerPanel = new Panel { Dock = DockStyle.Top, Height = 100 };
        card.Controls.Add(headerPanel);

        var logoPanel = new Panel
        {
            Width = 64,
            Height = 64,
            Left = 0,
            Top = 18,
            BackColor = Color.FromArgb(59, 130, 246)
        };
        headerPanel.Controls.Add(logoPanel);

        var iconLabel = new Label
        {
            Text = "BGS",
            Font = new Font("微软雅黑", 20f, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point(14, 15),
            AutoSize = true
        };
        logoPanel.Controls.Add(iconLabel);

        var infoPanel = new Panel { Left = 80, Top = 10, Width = 400 };
        headerPanel.Controls.Add(infoPanel);

        var lblAppName = new Label
        {
            Text = "血气分析管理系统",
            Font = new Font("微软雅黑", 20f, FontStyle.Bold),
            AutoSize = true,
            ForeColor = Color.FromArgb(15, 23, 42)
        };
        infoPanel.Controls.Add(lblAppName);

        var version = GetVersionInfo();
        var lblVersion = new Label
        {
            Text = $"版本 {version.Version}",
            Font = new Font("微软雅黑", 12f),
            AutoSize = true,
            Top = 35,
            ForeColor = Color.FromArgb(59, 130, 246)
        };
        infoPanel.Controls.Add(lblVersion);

        var lblBuild = new Label
        {
            Text = $"编译日期: {version.BuildDate}",
            Font = new Font("微软雅黑", 9f),
            AutoSize = true,
            Top = 60,
            ForeColor = Color.FromArgb(100, 116, 139)
        };
        infoPanel.Controls.Add(lblBuild);

        var line = new Panel
        {
            Dock = DockStyle.Top,
            Height = 1,
            BackColor = Color.FromArgb(226, 232, 240)
        };
        card.Controls.Add(line);

        var featuresGroup = new GroupBox
        {
            Text = "系统特性",
            Dock = DockStyle.Top,
            Height = 220,
            Font = new Font("微软雅黑", 12f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 41, 59)
        };
        card.Controls.Add(featuresGroup);

        var featuresLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 5,
            Padding = new Padding(15)
        };
        featuresLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        featuresLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        featuresLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
        featuresLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
        featuresLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
        featuresLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
        featuresLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
        featuresGroup.Controls.Add(featuresLayout);

        AddFeatureItem(featuresLayout, "📡", "ASTM LIS2-A2 协议支持", 0, 0);
        AddFeatureItem(featuresLayout, "🖥️", "服务端/客户端分离架构", 0, 1);
        AddFeatureItem(featuresLayout, "💾", "SQLite 本地数据库", 1, 0);
        AddFeatureItem(featuresLayout, "🔑", "API Key 安全认证", 1, 1);
        AddFeatureItem(featuresLayout, "⚡", "实时数据接收", 2, 0);
        AddFeatureItem(featuresLayout, "📊", "历史数据查询统计", 2, 1);
        AddFeatureItem(featuresLayout, "🖨️", "报告打印预览", 3, 0);
        AddFeatureItem(featuresLayout, "⌨️", "自定义快捷键", 3, 1);
        AddFeatureItem(featuresLayout, "🔄", "数据备份恢复", 4, 0);
        AddFeatureItem(featuresLayout, "📱", "支持局域网访问", 4, 1);

        var updateGroup = new GroupBox
        {
            Text = "更新日志",
            Dock = DockStyle.Top,
            Height = 160,
            Font = new Font("微软雅黑", 12f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 41, 59)
        };
        card.Controls.Add(updateGroup);

        var updateList = new ListBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("微软雅黑", 9f),
            BorderStyle = BorderStyle.None,
            Padding = new Padding(10)
        };
        updateList.Items.Add("• 修复患者匹配逻辑，确保精确匹配病案号");
        updateList.Items.Add("• 添加全局快捷键支持（F5刷新、F6审核、Del删除等）");
        updateList.Items.Add("• 优化API请求重试机制，提升网络稳定性");
        updateList.Items.Add("• 新增数据库索引，提升查询性能");
        updateList.Items.Add("• 支持旧数据库自动迁移升级");
        updateGroup.Controls.Add(updateList);

        var footerPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
        card.Controls.Add(footerPanel);

        var copyright = new Label
        {
            Text = "© 2024 血气分析管理系统. All rights reserved.",
            Font = new Font("微软雅黑", 9f),
            AutoSize = true,
            ForeColor = Color.FromArgb(148, 163, 184),
            Location = new Point(0, 12)
        };
        footerPanel.Controls.Add(copyright);

        mainPanel.SizeChanged += (s, e) =>
        {
            card.Left = (mainPanel.Width - card.Width) / 2;
            card.Top = (mainPanel.Height - card.Height) / 2;
        };
    }

    private void AddFeatureItem(TableLayoutPanel layout, string icon, string text, int row, int col)
    {
        var lbl = new Label
        {
            Text = $"{icon} {text}",
            Font = new Font("微软雅黑", 10f),
            ForeColor = Color.FromArgb(51, 65, 85),
            Padding = new Padding(5)
        };
        layout.Controls.Add(lbl, col, row);
    }

    private (string Version, string BuildDate) GetVersionInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        var versionStr = version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "2.0.0";

        string buildDate = "未知";
        try
        {
            var fileInfo = new FileInfo(assembly.Location);
            buildDate = fileInfo.LastWriteTime.ToString("yyyy-MM-dd");
        }
        catch { }

        return (versionStr, buildDate);
    }
}