using BGShared.Models;

namespace BGManager.Views;

public class TestResultsPanel : Panel
{
    public DataGridView DgvResults { get; private set; }
    public Button BtnAbnormalOnly { get; private set; }
    public Button BtnCopy { get; private set; }
    public Button BtnPreview { get; private set; }

    public event EventHandler? AbnormalOnlyClicked;
    public event EventHandler? CopyClicked;
    public event EventHandler? PreviewClicked;

    public TestResultsPanel()
    {
        Dock = DockStyle.Fill;
        BuildUI();
    }

    private void BuildUI()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55f));
        Controls.Add(layout);

        var group = new GroupBox { Text = "检查结果", Dock = DockStyle.Fill, Font = new Font("微软雅黑", 10f) };
        var resultPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };

        DgvResults = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AllowUserToResizeColumns = false,
            ReadOnly = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.CellSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            GridColor = Color.FromArgb(226, 232, 240),
            Font = new Font("微软雅黑", 10f),
            ColumnHeadersHeight = 35,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = new Font("微软雅黑", 10f, FontStyle.Bold), SelectionBackColor = Color.FromArgb(241, 245, 249), SelectionForeColor = Color.FromArgb(71, 85, 105) },
            RowTemplate = new DataGridViewRow { Height = 28 },
            EnableHeadersVisualStyles = false,
            DefaultCellStyle = new DataGridViewCellStyle { SelectionBackColor = Color.Transparent, SelectionForeColor = Color.Black }
        };
        DgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
        DgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105);
        DgvResults.CurrentCellChanged += (s, e) => DgvResults.ClearSelection();

        DgvResults.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "Check",
            HeaderText = "选中",
            Width = 50,
            ReadOnly = false,
            FillWeight = 50
        });
        DgvResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ChineseName",
            HeaderText = "中文名称",
            ReadOnly = true,
            FillWeight = 150
        });
        DgvResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "TestName",
            HeaderText = "项目名称",
            ReadOnly = true,
            FillWeight = 130
        });
        DgvResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Value",
            HeaderText = "当前值",
            ReadOnly = true,
            FillWeight = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("微软雅黑", 10f, FontStyle.Bold) }
        });
        DgvResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "History1",
            HeaderText = "历史1",
            ReadOnly = true,
            FillWeight = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        DgvResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "History2",
            HeaderText = "历史2",
            ReadOnly = true,
            FillWeight = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        DgvResults.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "History3",
            HeaderText = "历史3",
            ReadOnly = true,
            FillWeight = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        var unitCol = new DataGridViewTextBoxColumn
        {
            Name = "Unit",
            HeaderText = "单位",
            ReadOnly = true,
            FillWeight = 70,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            HeaderCell = { Style = new DataGridViewCellStyle { ForeColor = Color.Black } }
        };
        DgvResults.Columns.Add(unitCol);
        var refCol = new DataGridViewTextBoxColumn
        {
            Name = "Reference",
            HeaderText = "参考范围",
            ReadOnly = true,
            FillWeight = 120,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            HeaderCell = { Style = new DataGridViewCellStyle { ForeColor = Color.Black } }
        };
        DgvResults.Columns.Add(refCol);

        resultPanel.Controls.Add(DgvResults);
        group.Controls.Add(resultPanel);
        layout.Controls.Add(group, 0, 0);

        var bottomPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
            Padding = new Padding(5)
        };
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));

        BtnAbnormalOnly = new Button { Text = "仅勾选异常", Dock = DockStyle.Fill, Margin = new Padding(2), Font = new Font("微软雅黑", 11f) };
        BtnAbnormalOnly.Click += (s, e) => AbnormalOnlyClicked?.Invoke(this, EventArgs.Empty);

        BtnCopy = new Button { Text = "复制", Dock = DockStyle.Fill, Margin = new Padding(2), Font = new Font("微软雅黑", 11f), Enabled = false };
        BtnCopy.Click += (s, e) => CopyClicked?.Invoke(this, EventArgs.Empty);

        BtnPreview = new Button { Text = "预览", Dock = DockStyle.Fill, Margin = new Padding(2), Font = new Font("微软雅黑", 11f) };
        BtnPreview.Click += (s, e) => PreviewClicked?.Invoke(this, EventArgs.Empty);

        bottomPanel.Controls.Add(BtnAbnormalOnly, 0, 0);
        bottomPanel.Controls.Add(BtnCopy, 1, 0);
        bottomPanel.Controls.Add(BtnPreview, 2, 0);

        layout.Controls.Add(bottomPanel, 0, 1);
    }

    public void Clear()
    {
        DgvResults.Rows.Clear();
        BtnCopy.Enabled = false;
    }

    public void BindData(Specimen specimen, List<Specimen> historicalSpecimens)
    {
        DgvResults.Rows.Clear();

        if (specimen.Results == null || specimen.Results.Count == 0)
        {
            BtnCopy.Enabled = false;
            return;
        }

        var testCodes = specimen.Results.Select(r => r.TestName).Distinct().ToList();

        foreach (var code in testCodes)
        {
            var result = specimen.Results.First(r => r.TestName == code);
            var historyValues = GetHistoricalValues(code, historicalSpecimens);

            bool isAbnormal = result.Flag == ResultFlag.High || result.Flag == ResultFlag.CriticalHigh ||
                              result.Flag == ResultFlag.Low || result.Flag == ResultFlag.CriticalLow;

            string displayValue = result.Value.HasValue ? result.Value.Value.ToString("G6") : (result.RawValue ?? "-");
            string arrow = "";
            
            if (isAbnormal)
            {
                if (result.Flag == ResultFlag.High || result.Flag == ResultFlag.CriticalHigh)
                    arrow = " ↑";
                else if (result.Flag == ResultFlag.Low || result.Flag == ResultFlag.CriticalLow)
                    arrow = " ↓";
            }

            string reference = "";
            if (result.MinRange.HasValue && result.MaxRange.HasValue)
                reference = $"{result.MinRange.Value}-{result.MaxRange.Value}";
            else if (result.MinRange.HasValue)
                reference = $">={result.MinRange.Value}";
            else if (result.MaxRange.HasValue)
                reference = $"<={result.MaxRange.Value}";

            DgvResults.Rows.Add(false, result.TestName, result.TestName, displayValue + arrow,
                historyValues[0], historyValues[1], historyValues[2], result.Unit, reference);

            var row = DgvResults.Rows[DgvResults.Rows.Count - 1];
            if (isAbnormal)
            {
                row.Cells["Value"].Style.ForeColor = Color.Red;
                row.Cells["ChineseName"].Style.ForeColor = Color.Red;
            }
        }

        BtnCopy.Enabled = true;
    }

    private List<string> GetHistoricalValues(string testName, List<Specimen> historicalSpecimens)
    {
        var values = new List<string>();
        foreach (var hist in historicalSpecimens)
        {
            var result = hist.Results?.FirstOrDefault(r => r.TestName == testName);
            if (result != null && result.Value.HasValue)
            {
                values.Add(result.Value.Value.ToString("G6"));
            }
        }
        while (values.Count < 3)
            values.Add("");
        return values.Take(3).ToList();
    }
}