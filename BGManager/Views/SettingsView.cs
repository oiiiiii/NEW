using BGManager.Services;
using BGManager.Tools;

namespace BGManager.Views;

public class SettingsView : UserControl
{
    private readonly ApiClient _api;

    private TextBox txtServerUrl;
    private NumericUpDown nudAutoRefresh;
    private Button btnTest;
    private Button btnSave;
    private Label lblTestResult;
    private Label lblStatus;
    private Button btnImportLegacy;
    private Label lblImportStatus;
    private Button btnDisplaySettings;
    private Button btnPrintTemplateSettings;
    private Button btnShortcutSettings;

    public event EventHandler? SettingsChanged;

    public SettingsView(ApiClient api)
    {
        _api = api;
        Dock = DockStyle.Fill;
        BuildUI();
        LoadSettings();
    }

    private void BuildUI()
    {
        var container = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.FromArgb(249, 250, 251) };
        Controls.Add(container);

        var card = new Panel
        {
            Width = 500,
            Height = 720,
            BackColor = Color.White,
            Left = 30,
            Top = 30,
            Padding = new Padding(25)
        };
        container.Controls.Add(card);

        var lblTitle = new Label
        {
            Text = "系统设置",
            Font = new Font("微软雅黑", 14f, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(25, 20),
            ForeColor = Color.FromArgb(30, 41, 59)
        };
        card.Controls.Add(lblTitle);

        var line = new Panel
        {
            Location = new Point(25, 55),
            Size = new Size(450, 1),
            BackColor = Color.FromArgb(226, 232, 240)
        };
        card.Controls.Add(line);

        int y = 75;

        var lblServer = new Label
        {
            Text = "服务端地址",
            AutoSize = true,
            Location = new Point(25, y),
            Font = new Font("微软雅黑", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85)
        };
        card.Controls.Add(lblServer);
        y += 25;

        txtServerUrl = new TextBox { Location = new Point(25, y), Width = 350, Height = 28 };
        card.Controls.Add(txtServerUrl);

        btnTest = new Button { Text = "测试连接", Width = 80, Height = 28, Location = new Point(395, y - 1) };
        btnTest.Click += async (s, e) => await TestConnectionAsync();
        card.Controls.Add(btnTest);
        y += 35;

        lblTestResult = new Label { Text = "", AutoSize = true, Location = new Point(25, y), ForeColor = Color.Gray };
        card.Controls.Add(lblTestResult);
        y += 40;

        var lblRefresh = new Label
        {
            Text = "自动刷新间隔（秒）",
            AutoSize = true,
            Location = new Point(25, y),
            Font = new Font("微软雅黑", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85)
        };
        card.Controls.Add(lblRefresh);
        y += 25;

        nudAutoRefresh = new NumericUpDown { Location = new Point(25, y), Width = 100, Minimum = 1, Maximum = 600, Value = 5 };
        card.Controls.Add(nudAutoRefresh);
        y += 40;

        btnSave = new Button
        {
            Text = "保存设置",
            Width = 100,
            Height = 32,
            Location = new Point(25, y),
            BackColor = Color.FromArgb(59, 130, 246),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.Click += (s, e) => SaveSettings();
        card.Controls.Add(btnSave);
        y += 35;

        lblStatus = new Label { Text = "", AutoSize = true, Location = new Point(140, y + 8), ForeColor = Color.FromArgb(34, 197, 94) };
        card.Controls.Add(lblStatus);

        y += 50;
        var line2 = new Panel
        {
            Location = new Point(25, y),
            Size = new Size(450, 1),
            BackColor = Color.FromArgb(226, 232, 240)
        };
        card.Controls.Add(line2);

        y += 15;
        var lblImport = new Label
        {
            Text = "数据迁移",
            AutoSize = true,
            Location = new Point(25, y),
            Font = new Font("微软雅黑", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85)
        };
        card.Controls.Add(lblImport);

        y += 30;
        btnImportLegacy = new Button
        {
            Text = "导入旧版数据库",
            Width = 120,
            Height = 32,
            Location = new Point(25, y),
            BackColor = Color.FromArgb(245, 158, 11),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnImportLegacy.FlatAppearance.BorderSize = 0;
        btnImportLegacy.Click += BtnImportLegacy_Click;
        card.Controls.Add(btnImportLegacy);

        lblImportStatus = new Label { Text = "选择旧版 bloodgas.db 文件进行数据迁移", AutoSize = true, Location = new Point(160, y + 8), ForeColor = Color.Gray, Font = new Font("微软雅黑", 9f) };
        card.Controls.Add(lblImportStatus);

        y += 60;
        var line3 = new Panel
        {
            Location = new Point(25, y),
            Size = new Size(450, 1),
            BackColor = Color.FromArgb(226, 232, 240)
        };
        card.Controls.Add(line3);

        y += 15;
        var lblDisplay = new Label
        {
            Text = "显示配置",
            AutoSize = true,
            Location = new Point(25, y),
            Font = new Font("微软雅黑", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85)
        };
        card.Controls.Add(lblDisplay);

        y += 30;
        btnDisplaySettings = new Button
        {
            Text = "显示设置",
            Width = 120,
            Height = 32,
            Location = new Point(25, y),
            BackColor = Color.FromArgb(34, 197, 94),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnDisplaySettings.FlatAppearance.BorderSize = 0;
        btnDisplaySettings.Click += BtnDisplaySettings_Click;
        card.Controls.Add(btnDisplaySettings);

        var lblDisplayDesc = new Label { Text = "配置检验结果的显示顺序、上下限和参考值", AutoSize = true, Location = new Point(160, y + 8), ForeColor = Color.Gray, Font = new Font("微软雅黑", 9f) };
        card.Controls.Add(lblDisplayDesc);

        y += 60;
        var line4 = new Panel
        {
            Location = new Point(25, y),
            Size = new Size(450, 1),
            BackColor = Color.FromArgb(226, 232, 240)
        };
        card.Controls.Add(line4);

        y += 15;
        var lblPrint = new Label
        {
            Text = "打印模板",
            AutoSize = true,
            Location = new Point(25, y),
            Font = new Font("微软雅黑", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85)
        };
        card.Controls.Add(lblPrint);

        y += 30;
        btnPrintTemplateSettings = new Button
        {
            Text = "打印模板设置",
            Width = 120,
            Height = 32,
            Location = new Point(25, y),
            BackColor = Color.FromArgb(59, 130, 246),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnPrintTemplateSettings.FlatAppearance.BorderSize = 0;
        btnPrintTemplateSettings.Click += BtnPrintTemplateSettings_Click;
        card.Controls.Add(btnPrintTemplateSettings);

        var lblPrintDesc = new Label { Text = "配置打印报告的页面、页眉、页脚等模板", AutoSize = true, Location = new Point(160, y + 8), ForeColor = Color.Gray, Font = new Font("微软雅黑", 9f) };
        card.Controls.Add(lblPrintDesc);

        y += 60;
        var line5 = new Panel
        {
            Location = new Point(25, y),
            Size = new Size(450, 1),
            BackColor = Color.FromArgb(226, 232, 240)
        };
        card.Controls.Add(line5);

        y += 15;
        var lblShortcut = new Label
        {
            Text = "快捷键设置",
            AutoSize = true,
            Location = new Point(25, y),
            Font = new Font("微软雅黑", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85)
        };
        card.Controls.Add(lblShortcut);

        y += 30;
        btnShortcutSettings = new Button
        {
            Text = "快捷键配置",
            Width = 120,
            Height = 32,
            Location = new Point(25, y),
            BackColor = Color.FromArgb(168, 85, 247),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnShortcutSettings.FlatAppearance.BorderSize = 0;
        btnShortcutSettings.Click += BtnShortcutSettings_Click;
        card.Controls.Add(btnShortcutSettings);

        var lblShortcutDesc = new Label { Text = "配置常用操作的快捷键", AutoSize = true, Location = new Point(160, y + 8), ForeColor = Color.Gray, Font = new Font("微软雅黑", 9f) };
        card.Controls.Add(lblShortcutDesc);

        card.Height = 880;
    }

    private void BtnDisplaySettings_Click(object? sender, EventArgs e)
    {
        using var form = new DisplaySettingsForm();
        form.ShowDialog();
    }

    private void BtnPrintTemplateSettings_Click(object? sender, EventArgs e)
    {
        using var form = new PrintTemplateSettingsForm();
        form.ShowDialog();
    }

    private void BtnShortcutSettings_Click(object? sender, EventArgs e)
    {
        var config = AppConfig.Load();
        using var form = new ShortcutSettingsForm(config.ShortcutKeys);
        if (form.ShowDialog() == DialogResult.OK)
        {
            config.Save();
        }
    }

    private void LoadSettings()
    {
        txtServerUrl.Text = _api.BaseUrl;
    }

    private async Task TestConnectionAsync()
    {
        lblTestResult.Text = "测试中...";
        lblTestResult.ForeColor = Color.Gray;

        try
        {
            var url = txtServerUrl.Text.Trim();
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            var resp = await client.GetAsync($"{url.TrimEnd('/')}/api/status");
            if (resp.IsSuccessStatusCode)
            {
                lblTestResult.Text = "✓ 连接成功";
                lblTestResult.ForeColor = Color.FromArgb(34, 197, 94);
            }
            else
            {
                lblTestResult.Text = $"✗ 连接失败 ({(int)resp.StatusCode})";
                lblTestResult.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblTestResult.Text = $"✗ {ex.Message}";
            lblTestResult.ForeColor = Color.Red;
        }
    }

    private void SaveSettings()
    {
        var url = txtServerUrl.Text.Trim();
        if (string.IsNullOrWhiteSpace(url))
        {
            MessageBox.Show("请输入服务端地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _api.BaseUrl = url;
        
        var config = AppConfig.Load();
        config.ServerUrl = url;
        config.Save();
        
        lblStatus.Text = "设置已保存";
        SettingsChanged?.Invoke(this, EventArgs.Empty);

        var timer = new System.Windows.Forms.Timer { Interval = 2000 };
        timer.Tick += (s, e) => { lblStatus.Text = ""; timer.Stop(); };
        timer.Start();
    }

    private void BtnImportLegacy_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "SQLite 数据库 (*.db)|*.db|所有文件 (*.*)|*.*",
            Title = "选择旧版数据库文件",
            FileName = "bloodgas.db"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        var result = MessageBox.Show(
            "确定要导入旧版数据库吗？\n\n导入后会生成新格式的数据库文件（bloodgas_new.db），不会覆盖当前数据库。",
            "确认导入",
            MessageBoxButtons.OKCancel,
            MessageBoxIcon.Question);

        if (result != DialogResult.OK)
            return;

        try
        {
            lblImportStatus.Text = "正在导入...";
            lblImportStatus.ForeColor = Color.FromArgb(59, 130, 246);
            btnImportLegacy.Enabled = false;

            string oldDbPath = dialog.FileName;
            string? dir = Path.GetDirectoryName(oldDbPath);
            string newDbPath = Path.Combine(dir ?? ".", "bloodgas_new.db");

            if (File.Exists(newDbPath))
                File.Delete(newDbPath);

            LegacyDbMigrator.Migrate(oldDbPath, newDbPath);

            lblImportStatus.Text = $"导入完成！新数据库: {Path.GetFileName(newDbPath)}";
            lblImportStatus.ForeColor = Color.FromArgb(34, 197, 94);

            MessageBox.Show(
                $"数据导入完成！\n\n新数据库位置:\n{newDbPath}\n\n请将新数据库文件复制到 BGService 运行目录，替换原有的 bloodgas.db",
                "导入成功",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            lblImportStatus.Text = "导入失败";
            lblImportStatus.ForeColor = Color.Red;
            MessageBox.Show($"导入失败: {ex.Message}\n\n{ex.StackTrace}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnImportLegacy.Enabled = true;
        }
    }
}
