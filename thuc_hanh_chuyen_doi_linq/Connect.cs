using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace thuc_hanh_chuyen_doi_linq
{
    internal class Connect
    {
        SqlConnection conn= new SqlConnection("Data Source=DESKTOP-4TDH76B\\DELL3501;Initial Catalog=thuc_hanh_chuyen_doi_xml;Integrated Security=True");
        public DataTable Load(string sql)
        {
            conn.Open();
            SqlDataAdapter ad= new SqlDataAdapter(sql,conn);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            conn.Close();
            return dt;
        }
        public void Excecute(string sql)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public DataTable Execute1(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-4TDH76B\\DELL3501;Initial Catalog=thuc_hanh_chuyen_doi_xml;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

    }
}
