using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Lab07
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HienThiDanhSach();
            btnSua.Enabled= false;
            btnXoa.Enabled= false;
            gbTTSV.Enabled= false;
        }

        string strCon = @"Data Source=puoqnt;Initial Catalog=QLSV;Integrated Security=True";
        SqlConnection sqlCon= null;

        DataSet ds = null;
        SqlDataAdapter adapter= null;

        int chucNang = 0;
        int vt = -1;

        private void MoKetNoi()
        {
            if(sqlCon==null) sqlCon = new SqlConnection(strCon);
            if(sqlCon.State==ConnectionState.Closed) sqlCon.Open();
        }

        private void HienThiDanhSach()
        {
            MoKetNoi();

            string sql = "select * from SV";
            adapter = new SqlDataAdapter(sql,sqlCon);

            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

            ds = new DataSet();
            adapter.Fill(ds,"tblSinhVien");
            dgvSinhVien.DataSource = ds.Tables["tblSinhVien"];
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có thực sự muốn thoát?","Thoát ứng dụng",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == DialogResult.Yes) { Close(); }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maSV = txtTimKiemMaSV.Text.Trim();
            string tenSV = txtTimKiemTenSV.Text.Trim();

            if (maSV != "" && tenSV == "") TimKiemTheoMa(maSV);
            else if (maSV == "" && tenSV != "") TimKiemTheoTen(tenSV);
            else if (maSV != "" && tenSV != "") TimKiemTheoMa(maSV);
            else
            {
                MessageBox.Show("Nhập dữ liệu cần tìm!");
                HienThiDanhSach();
                txtTimKiemMaSV.Focus();
            }
        }

        private void TimKiemTheoMa(string maSV)
        {
            MoKetNoi();

            string sql = "select * from SV where MaSV='"+maSV+"'";
            adapter = new SqlDataAdapter(sql, sqlCon);

            ds = new DataSet();
            adapter.Fill(ds, "tblMaSV");
            dgvSinhVien.DataSource = ds.Tables["tblMaSV"];
        }   
        
        private void TimKiemTheoTen(string tenSV)
        {
            MoKetNoi();

            string sql = "select * from SV where TenSV like '%"+tenSV+"%'";
            adapter = new SqlDataAdapter(sql, sqlCon);

            ds = new DataSet();
            adapter.Fill(ds, "tblTenSV");
            dgvSinhVien.DataSource = ds.Tables["tblTenSV"];
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            chucNang = 1;
            gbTTSV.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            chucNang = 2;
            gbTTSV.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (chucNang == 1) ThemSV();
            if (chucNang == 2) SuaSV();
        }

        private void XoaDuLieu()
        {
            txtMaSV.Clear();
            txtTenSV.Clear();
            txtQueQuan.Clear();
            txtMaLop.Clear();
        }

        private void ThemSV()
        {
            MoKetNoi();

            DataRow row = ds.Tables["tblSinhVien"].NewRow();

            row["MaSV"] = txtMaSV.Text.Trim();
            row["TenSV"] = txtTenSV.Text.Trim();
            if (gtNam.Checked == true) row["GioiTinh"] = "Nam";
            else if (gtNu.Checked == true) row["GioiTinh"] = "Nu";
            row["NgaySinh"] = dtpNgaySinh.Value.Year + "/" + dtpNgaySinh.Value.Month + "/" + dtpNgaySinh.Value.Day;
            row["QueQuan"] = txtQueQuan.Text.Trim();
            row["MaLop"] = txtMaLop.Text.Trim();

            ds.Tables["tblSinhVien"].Rows.Add(row);

            int kq = adapter.Update(ds.Tables["tblSinhVien"]);
            if (kq>0)
            {
                HienThiDanhSach();
                MessageBox.Show("Thêm sinh viên thành công!");
                XoaDuLieu();

                gbTTSV.Enabled= false;
            }
            else
            {
                MessageBox.Show("Thêm sinh viên không thành công!");
                XoaDuLieu();
                gbTTSV.Enabled = false;
            }
    
        }

        private void SuaSV() //trong hàm sửa phải có row.beginEdit và row.EndEdit
        {
            if (vt == -1) return;
            MoKetNoi();

            DataRow row = ds.Tables["tblSinhVien"].Rows[vt];

            row.BeginEdit();

            row["MaSV"] = txtMaSV.Text.Trim();
            row["TenSV"] = txtTenSV.Text.Trim();
            if (gtNam.Checked == true) row["GioiTinh"] = "Nam";
            else if (gtNu.Checked == true) row["GioiTinh"] = "Nu";
            row["NgaySinh"] = dtpNgaySinh.Value.Year + "/" + dtpNgaySinh.Value.Month + "/" + dtpNgaySinh.Value.Day;
            row["QueQuan"] = txtQueQuan.Text.Trim();
            row["MaLop"] = txtMaLop.Text.Trim();

            row.EndEdit();


            int kq = adapter.Update(ds.Tables["tblSinhVien"]);
            if (kq > 0)
            {
                HienThiDanhSach();
                MessageBox.Show("CHỉnh sửa sinh viên thành công!");
                XoaDuLieu();

                gbTTSV.Enabled = false;
            }
            else
            {
                MessageBox.Show("Chỉnh sửa sinh viên không thành công!");
                XoaDuLieu();
                gbTTSV.Enabled = false;
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (vt == -1) return;
            DialogResult result = MessageBox.Show("Bạn có thực sự muốn thoát?","Thoát ứng dụng!",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == DialogResult.Yes) XoaSinhVien();
            else btnXoa.Enabled = false;
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void XoaSinhVien()
        {
            if (vt == -1) return ;
            MoKetNoi();
            DataRow row = ds.Tables["tblSinhVien"].Rows[vt] ;
            row.Delete();
            int kq = adapter.Update(ds.Tables["tblSinhVien"]);
            if (kq > 0)
            {
                MessageBox.Show("Xóa sinh viên thành công!");
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Xóa sinh viên không thành công!");
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            vt = e.RowIndex;
            if (vt == -1) return;

            DataRow row = ds.Tables["tblSinhVien"].Rows[vt];
            txtMaSV.Text = row["MaSV"].ToString().Trim();
            txtTenSV.Text = row["TenSV"].ToString().Trim();
            if (row["GioiTinh"].ToString().Trim() == "Nam") gtNam.Checked = true;
            else if (row["GioiTinh"].ToString().Trim() == "Nữ" || row["GioiTinh"].ToString().Trim() == "Nu") gtNu.Checked = true;
            string[] a = row["NgaySinh"].ToString().Trim().Split(' ');
            string[] b = a[0].Split('/');
            dtpNgaySinh.Value = new DateTime(int.Parse(b[2]), int.Parse(b[1]), int.Parse(b[0]));
            txtQueQuan.Text = row["QueQuan"].ToString().Trim();
            txtMaLop.Text = row["MaLop"].ToString().Trim();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // Tắt chế độ chỉnh sửa thông tin
            gbTTSV.Enabled = false;

            // Xóa dữ liệu đang hiển thị ở các TextBox nếu bạn muốn reset hoàn toàn
            XoaDuLieu();

            // Vô hiệu hóa nút Sửa và Xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Reset biến chức năng và vị trí
            chucNang = 0;
            vt = -1;
        }
    }
}
