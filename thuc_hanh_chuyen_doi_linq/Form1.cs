using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;

namespace thuc_hanh_chuyen_doi_linq
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataSet ds= new DataSet();
        Connect ob = new Connect();

        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = new string[100];
            if (System.IO.File.Exists("D:\\Bai thuc hanh XML\\thuc_hanh_chuyen_doi_linq\\thuc_hanh_chuyen_doi_linq\\Brand.xml"))
            {
                lines = System.IO.File.ReadAllLines("D:\\Bai thuc hanh XML\\thuc_hanh_chuyen_doi_linq\\thuc_hanh_chuyen_doi_linq\\Brand.xml");
            }
            int i = 0;
            while (i < lines.Length)
            {
                if (lines[i].Length > 0)
                {
                    listBox1.Items.Add(lines[i]);
                }
                i++;
            }
        }
      /*  static bool IsTableExists(SqlConnection conn, string tableName)
        {
            string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            SqlCommand command = new SqlCommand(query, conn);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return (count > 0);
        }
        static void CreateBrandTable(SqlConnection connection)
        {
            string query = "CREATE TABLE Brand (ID INT PRIMARY KEY, Name NVARCHAR(50))";
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Brand table created successfully.");
        }*/
       /* SqlConnection conn = new SqlConnection("Data Source=DESKTOP-4TDH76B\\DELL3501;Initial Catalog=thuc_hanh_chuyen_doi_xml;Integrated Security=True");*/
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string select = "SELECT * FROM Brand";
                DataTable table = ob.Execute1(select);

                if (table == null || table.Rows.Count == 0)
                {
                    string createTable = "CREATE TABLE Brand(ID int not null, Name nvarchar(50))";
                    ob.Execute1(createTable);
                    MessageBox.Show("Tạo thành công bảng Brand");
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load("D:\\Bai thuc hanh XML\\thuc_hanh_chuyen_doi_linq\\thuc_hanh_chuyen_doi_linq\\Brand.xml");
                    XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName("Brand");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        int id = int.Parse(xmlNode.ChildNodes[0].InnerText);
                        string name = xmlNode.ChildNodes[1].InnerText;

                        // Kiểm tra xem ID đã tồn tại trong bảng Brand chưa
                        string checkIdQuery = $"SELECT COUNT(*) FROM Brand WHERE ID = {id}";
                        DataTable resultTable = ob.Execute1(checkIdQuery);
                        int count = Convert.ToInt32(resultTable.Rows[0][0]);

                        if (count == 0)
                        {
                            // Nếu ID không tồn tại trong bảng Brand, thực hiện thêm bản ghi mới
                            string insertQuery = $"INSERT INTO Brand VALUES({id}, '{name}')";
                            ob.Execute1(insertQuery);
                        }
                        else
                        {

                             MessageBox.Show($"ID {id} đã tồn tại trong bảng Brand");
                        }
                    }
                    dataGridView1.DataSource = ob.Load("select * from Brand");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*string ID = textBox1.Text; // Chuyển đổi giá trị từ textbox sang kiểu int
            string Name = textBox2.Text;
            XDocument root = XDocument.Load("..\\..\\Brand.xml");
            
            XmlDocument root1 = new XmlDocument();
            root1.Load("..\\..\\Brand.xml");
            XmlNodeList ls = root1.GetElementsByTagName("Brand");
            bool kt = true;
            foreach (XmlNode n in ls)
            {
                if (n.El == ID)
                {
                    kt = false;
                    break;
                }
                if (kt)
                {
                    XElement brand = new XElement("Brand",
                           new XElement("ID", int.Parse(ID)),
                           new XElement("Name", Name)
                           );
                    root.Element("NewDataSet").Add(brand);
                    root.Save("..\\..\\Brand.xml");
                    
                    string sqlinsert = "insert into Brand(ID,Name) values ("+ID+",'"+Name+"')";
                    ob.Excecute(sqlinsert);
                }
                if(!kt)
                {
                    MessageBox.Show("ID đã tồn tại kiểm tra lại");
                    textBox1.Focus();
                }*/

            XDocument root = XDocument.Load("..\\..\\Brand.xml");
            XElement br = new XElement("Brand",
                           new XElement("ID",int.Parse( textBox1.Text)),
                           new XElement("Name", textBox2.Text)
                           );
            XmlDocument root1 = new XmlDocument();
            root1.Load("..\\..\\Brand.xml");
            XmlNodeList ls = root1.GetElementsByTagName("Brand");
            bool kt = true;
            foreach (XmlNode n in ls)
            {
                if (n.ChildNodes[0].InnerText == textBox1.Text.ToString())
                {
                    kt = false;
                    break;
                }
            }
            if (kt)
                {
                    root.Root.Add(br);
                    root.Save("..\\..\\Brand.xml");
                    MessageBox.Show("DL đã được thêm");
                }
                else
                {
                    MessageBox.Show("ID đã tồn tại kiểm tra lại");
                    textBox1.Focus();
                }
          

        }

        private void button4_Click(object sender, EventArgs e)
        {  
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("..\\..\\Brand.xml");
            XmlNodeList lopNodes = xmlDoc.SelectNodes("//Brand");
            bool found = false;
            foreach (XmlNode lopNode in lopNodes)
            {
                if (lopNode.SelectSingleNode("ID").InnerText == textBox1.Text.ToString())
                {
                    lopNode.SelectSingleNode("Name").InnerText = textBox2.Text;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                MessageBox.Show("Không tìm thấy brand cần sửa!");
            }
            else
            {
                xmlDoc.Save("..\\..\\Brand.xml");
                string Name = textBox2.Text;
                string sqlup = "update Brand set Name = '" + Name + "' where ID = " +int.Parse( textBox1.Text )+ "";
                ob.Excecute(sqlup);
                MessageBox.Show("Đã cập nhật cơ sở dữ liệu thành công!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("..\\..\\Brand.xml");
            XmlNodeList lopNodes = xmlDocument.SelectNodes("//Brand");
            bool found = false;
            foreach (XmlNode lopNode in lopNodes)
            {
                if (lopNode.SelectSingleNode("ID").InnerText == textBox1.Text)
                {
                    lopNode.ParentNode.RemoveChild(lopNode);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                MessageBox.Show("Không tìm thấy lớp cần xóa!");
            }
            else
            {
                xmlDocument.Save("..\\..\\Brand.xml");
                string IDToDelete = textBox1.Text;
                string sqlDelete = "DELETE FROM Brand WHERE ID = '" + IDToDelete + "'";
                ob.Excecute(sqlDelete);
                MessageBox.Show("Đã xóa lớp và cập nhật cơ sở dữ liệu thành công!");
            }
        }
    }
    
}
