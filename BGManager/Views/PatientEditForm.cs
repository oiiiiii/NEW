using BGShared.Models;

namespace BGManager.Views;

public class PatientEditForm : Form
{
    public Patient Patient { get; private set; }

    private TextBox txtPatientId;
    private TextBox txtPatientName;
    private TextBox txtNameAbbr;
    private ComboBox cboGender;
    private TextBox txtBedNo;
    private TextBox txtDepartment;
    private Button btnOk;
    private Button btnCancel;

    private bool _isNew;
    private readonly List<Patient> _allPatients;

    public PatientEditForm(Patient patient, List<Patient> allPatients)
    {
        Patient = patient;
        _allPatients = allPatients;
        _isNew = string.IsNullOrEmpty(patient.PatientId);
        Text = _isNew ? "新增患者" : "编辑患者";
        Width = 480;
        Height = 500;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("微软雅黑", 9f);

        BuildUI();
        LoadData();
        AttachValidationEvents();
        ValidateInputs();
    }

    private void BuildUI()
    {
        int y = 25;
        int labelX = 25;
        int inputX = 110;
        int inputWidth = 320;

        var lblId = new Label { Text = "病案号:", AutoSize = true, Location = new Point(labelX, y + 3) };
        Controls.Add(lblId);
        txtPatientId = new TextBox { Location = new Point(inputX, y), Width = inputWidth };
        Controls.Add(txtPatientId);
        y += 35;

        var lblName = new Label { Text = "姓名:", AutoSize = true, Location = new Point(labelX, y + 3) };
        Controls.Add(lblName);
        txtPatientName = new TextBox { Location = new Point(inputX, y), Width = inputWidth };
        Controls.Add(txtPatientName);
        y += 35;

        var lblAbbr = new Label { Text = "姓名缩写:", AutoSize = true, Location = new Point(labelX, y + 3) };
        Controls.Add(lblAbbr);
        txtNameAbbr = new TextBox { Location = new Point(inputX, y), Width = inputWidth };
        Controls.Add(txtNameAbbr);
        y += 35;

        var lblBed = new Label { Text = "床号:", AutoSize = true, Location = new Point(labelX, y + 3) };
        Controls.Add(lblBed);
        txtBedNo = new TextBox { Location = new Point(inputX, y), Width = inputWidth };
        Controls.Add(txtBedNo);
        y += 35;

        var lblDept = new Label { Text = "科室:", AutoSize = true, Location = new Point(labelX, y + 3) };
        Controls.Add(lblDept);
        txtDepartment = new TextBox { Location = new Point(inputX, y), Width = inputWidth, Text = "重症医学科" };
        Controls.Add(txtDepartment);
        y += 35;

        var lblGender = new Label { Text = "性别:", AutoSize = true, Location = new Point(labelX, y + 3) };
        Controls.Add(lblGender);
        cboGender = new ComboBox { Location = new Point(inputX, y), Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
        cboGender.Items.AddRange(new object[] { "男", "女" });
        Controls.Add(cboGender);
        y += 35;

        btnOk = new Button
        {
            Text = "确定",
            Width = 80,
            Height = 30,
            Location = new Point(280, y + 10),
            DialogResult = DialogResult.OK
        };
        btnOk.Click += (s, e) => SaveData();
        Controls.Add(btnOk);

        btnCancel = new Button
        {
            Text = "取消",
            Width = 80,
            Height = 30,
            Location = new Point(370, y + 10),
            DialogResult = DialogResult.Cancel
        };
        Controls.Add(btnCancel);

        AcceptButton = btnOk;
        CancelButton = btnCancel;
    }

    private void LoadData()
    {
        txtPatientId.Text = Patient.PatientId;
        txtPatientName.Text = Patient.PatientName;
        txtNameAbbr.Text = Patient.NameAbbreviation;
        txtBedNo.Text = Patient.BedNo;
        txtDepartment.Text = string.IsNullOrEmpty(Patient.Department) ? "重症医学科" : Patient.Department;
        cboGender.SelectedItem = Patient.Gender;
    }

    private void AttachValidationEvents()
    {
        txtPatientId.TextChanged += (s, e) => ValidateInputs();
        txtPatientName.TextChanged += (s, e) => ValidateInputs();
        txtBedNo.TextChanged += (s, e) => ValidateInputs();
    }

    private void ValidateInputs()
    {
        var patientId = txtPatientId.Text.Trim();
        var patientName = txtPatientName.Text.Trim();
        var bedNo = txtBedNo.Text.Trim();

        var patientIdDuplicate = !string.IsNullOrEmpty(patientId) && CheckDuplicate(p => p.PatientId == patientId);
        var bedNoDuplicate = !string.IsNullOrEmpty(bedNo) && CheckDuplicate(p => p.BedNo == bedNo);

        txtPatientId.BackColor = patientIdDuplicate ? Color.LightCoral : Color.White;
        txtBedNo.BackColor = bedNoDuplicate ? Color.LightCoral : Color.White;

        btnOk.Enabled = !patientIdDuplicate && !bedNoDuplicate && 
                        !string.IsNullOrEmpty(patientId) && 
                        !string.IsNullOrEmpty(patientName) && 
                        !string.IsNullOrEmpty(bedNo);
    }

    private bool CheckDuplicate(Func<Patient, bool> predicate)
    {
        return _allPatients.Any(p => p.IsActive && predicate(p) && p.PatientId != Patient.PatientId);
    }

    private void SaveData()
    {
        var patientId = txtPatientId.Text.Trim();
        var patientName = txtPatientName.Text.Trim();
        var bedNo = txtBedNo.Text.Trim();

        if (string.IsNullOrWhiteSpace(patientName))
        {
            MessageBox.Show("请输入姓名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            txtPatientName.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(patientId))
        {
            MessageBox.Show("请输入病案号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            txtPatientId.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(bedNo))
        {
            MessageBox.Show("请输入床号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            txtBedNo.Focus();
            return;
        }

        var patientIdDuplicate = CheckDuplicate(p => p.PatientId == patientId);
        if (patientIdDuplicate)
        {
            MessageBox.Show("病案号已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            txtPatientId.Focus();
            return;
        }

        var bedNoDuplicate = CheckDuplicate(p => p.BedNo == bedNo);
        if (bedNoDuplicate)
        {
            MessageBox.Show("床号已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            txtBedNo.Focus();
            return;
        }

        Patient.PatientId = patientId;
        Patient.PatientName = patientName;
        Patient.NameAbbreviation = txtNameAbbr.Text.Trim();
        Patient.BedNo = bedNo;
        Patient.Department = txtDepartment.Text.Trim();
        Patient.Gender = cboGender.SelectedItem?.ToString() ?? "";
        Patient.IsActive = true;
    }
}
