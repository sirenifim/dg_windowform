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
using System.Configuration;

namespace Db87CopyToMyLocal
{
    public partial class Form1 : Form
    {
        private string sqlsc = "";
        private List<string> dgtable = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            foreach(var stable in dgtable){
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["SmoothEnterpriseSqlConnStr"].ConnectionString;
            string sqlstr = @"
SELECT
    a.TABLE_NAME                as 表格名稱,
    b.COLUMN_NAME               as 欄位名稱,
    b.DATA_TYPE                 as 資料型別,
    b.CHARACTER_MAXIMUM_LENGTH  as 最大長度,
    b.COLUMN_DEFAULT            as 預設值,
    b.IS_NULLABLE               as 允許空值,
    (
        SELECT
            value
        FROM
            fn_listextendedproperty (NULL, 'schema', 'dbo', 'table', a.TABLE_NAME, 'column', default)
        WHERE
            name='MS_Description' 
            and objtype='COLUMN' 
            and objname Collate Chinese_Taiwan_Stroke_CI_AS = b.COLUMN_NAME
    ) as 欄位備註
FROM
    INFORMATION_SCHEMA.TABLES  a
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS b ON ( a.TABLE_NAME=b.TABLE_NAME )
WHERE
    TABLE_TYPE='BASE TABLE'
AND
	a.TABLE_NAME = '" + stable + "' ORDER BY a.TABLE_NAME, ordinal_position";

            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            try
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                sqlsc = "CREATE TABLE " + stable + "(";

                while (dr.Read())
                {
                    if (dr["資料型別"].ToString().Equals("uniqueidentifier"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [uniqueidentifier] ";
                            if (dr["允許空值"].ToString().Equals("YES"))
                            {
                                sqlsc += "NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    if (dr["欄位名稱"].ToString().Equals("id"))
                                    {
                                        sqlsc += "PRIMARY KEY " + "ROWGUIDCOL,";
                                    }
                                    else 
                                    {
                                        sqlsc += ",";
                                    }
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() +" ";
                                    if (dr["欄位名稱"].ToString().Equals("id"))
                                    {
                                        sqlsc += "PRIMARY KEY " + "ROWGUIDCOL,";
                                    }
                                    else
                                    {
                                        sqlsc += ",";
                                    }
                                }
                            }
                            else
                            {
                                sqlsc += "NOT NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    if (dr["欄位名稱"].ToString().Equals("id"))
                                    {
                                        sqlsc += "PRIMARY KEY " + "ROWGUIDCOL,";
                                    }
                                    else
                                    {
                                        sqlsc += ",";
                                    }
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + " ";
                                    if (dr["欄位名稱"].ToString().Equals("id"))
                                    {
                                        sqlsc += "PRIMARY KEY " + "ROWGUIDCOL,";
                                    }
                                    else
                                    {
                                        sqlsc += ",";
                                    }
                                }
                            }
                    }
                    else if (dr["資料型別"].ToString().Equals("varchar"))
                    {
                        if (!dr["最大長度"].ToString().Equals("-1"))
                        {
                            sqlsc += dr["欄位名稱"].ToString() + " [varchar](" + dr["最大長度"].ToString() + ") ";
                            if (dr["允許空值"].ToString().Equals("YES"))
                            {
                                sqlsc += "NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() +",";
                                }
                            }
                            else
                            {
                                sqlsc += "NOT NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }

                            }
                        }
                        else
                        {
                            sqlsc += dr["欄位名稱"].ToString() + " [varchar](" + "max" + ") ";
                            if (dr["允許空值"].ToString().Equals("YES"))
                            {
                                sqlsc += "NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }
                            }
                            else
                            {
                                sqlsc += "NOT NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }

                            }
                        }
                    }
                    else if (dr["資料型別"].ToString().Equals("nvarchar"))
                    {
                        if (!dr["最大長度"].ToString().Equals("-1"))
                        {
                            sqlsc += dr["欄位名稱"].ToString() + " [nvarchar](" + dr["最大長度"].ToString() + ") ";
                            if (dr["允許空值"].ToString().Equals("YES"))
                            {
                                sqlsc += "NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }
                            }
                            else
                            {
                                sqlsc += "NOT NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }

                            }
                        }
                        else
                        {
                            sqlsc += dr["欄位名稱"].ToString() + " [nvarchar](" + "max" + ") ";
                            if (dr["允許空值"].ToString().Equals("YES"))
                            {
                                sqlsc += "NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }
                            }
                            else
                            {
                                sqlsc += "NOT NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }

                            }
                        }
                    }
                    else if (dr["資料型別"].ToString().Equals("ntext"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [ntext] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }
                    else if (dr["資料型別"].ToString().Equals("smallint"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [smallint] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("date"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [date] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("datetime"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [datetime] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("decimal"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [decimal] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("float"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [float] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("image"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [image] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("int"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [int] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("numeric"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [numeric] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("text"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [text] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("varbinary"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [varbinary] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("xml"))
                    {
                        sqlsc += dr["欄位名稱"].ToString() + " [xml] ";
                        if (dr["允許空值"].ToString().Equals("YES"))
                        {
                            sqlsc += "NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                        else
                        {
                            sqlsc += "NOT NULL ";
                            if (dr["預設值"] is DBNull)
                            {
                                sqlsc += ",";
                            }
                            else
                            {
                                sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                            }
                        }
                    }

                    else if (dr["資料型別"].ToString().Equals("char"))
                    {
                        if (!dr["最大長度"].ToString().Equals("-1"))
                        {
                            sqlsc += dr["欄位名稱"].ToString() + " [char](" + dr["最大長度"].ToString() + ") ";
                            if (dr["允許空值"].ToString().Equals("YES"))
                            {
                                sqlsc += "NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }
                            }
                            else
                            {
                                sqlsc += "NOT NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }

                            }
                        }
                        else
                        {
                            sqlsc += dr["欄位名稱"].ToString() + " [char](" + "max" + ") ";
                            if (dr["允許空值"].ToString().Equals("YES"))
                            {
                                sqlsc += "NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }
                            }
                            else
                            {
                                sqlsc += "NOT NULL ";
                                if (dr["預設值"] is DBNull)
                                {
                                    sqlsc += ",";
                                }
                                else
                                {
                                    sqlsc += "DEFAULT " + dr["預設值"].ToString() + ",";
                                }

                            }
                        }
                    }

                }
                sqlsc += ");";

                dr.Close();
                textBox1.Text = sqlsc;

                //create db table to local
                SqlConnection targetconn = new SqlConnection("");
                targetconn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalConnectionString"].ConnectionString;
                string targetsqlstr = sqlsc;

                SqlCommand targetcmd = new SqlCommand(targetsqlstr, targetconn);
                try
                {
                    targetconn.Open();
                    targetcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    targetcmd.Cancel();
                    targetconn.Close();
                    targetconn.Dispose();
                    sqlsc = string.Empty;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

                }
        }


        private void sqlKeep()
        {
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["SmoothEnterpriseSqlConnStr"].ConnectionString;
            string sqlstr = @"SELECT * FROM dgcode;";
                        SqlCommand cmd = new SqlCommand(sqlstr, conn);
                        try
                        {
                            conn.Open();
                            SqlDataReader dr = cmd.ExecuteReader();


                            while (dr.Read())
                            {
                                ;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
            finally
            {
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }


            SqlConnection targetconn = new SqlConnection("");
            targetconn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalConnectionString"].ConnectionString;
            string targetsqlstr = @"
INSERT INTO [dbo].[dgcode]
           ([id]
           ,[ctype]
           ,[code]
           ,[name]
           ,[namezhtw]
           ,[remark]
           ,[remarkzhtw]
           ,[seq]
           ,[systemcode]
           ,[systemcontrol]
           ,[iconurl]
           ,[style]
           ,[enabled]
           ,[isdefault]
           ,[optvalue]
           ,[inituid]
           ,[initdate]
           ,[modifyuid]
           ,[modifydate]
           ,[namezhcn]
           ,[remarkzhcn]
           ,[namejajp]
           ,[remarkjajp])
     VALUES
           (<id, uniqueidentifier,>
           ,<ctype, varchar(20),>
           ,<code, varchar(5),>
           ,<name, nvarchar(50),>
           ,<namezhtw, nvarchar(50),>
           ,<remark, nvarchar(max),>
           ,<remarkzhtw, ntext,>
           ,<seq, smallint,>
           ,<systemcode, varchar(5),>
           ,<systemcontrol, varchar(1),>
           ,<iconurl, nvarchar(255),>
           ,<style, nvarchar(max),>
           ,<enabled, varchar(1),>
           ,<isdefault, varchar(1),>
           ,<optvalue, nvarchar(50),>
           ,<inituid, uniqueidentifier,>
           ,<initdate, datetime,>
           ,<modifyuid, uniqueidentifier,>
           ,<modifydate, datetime,>
           ,<namezhcn, nvarchar(50),>
           ,<remarkzhcn, nvarchar(max),>
           ,<namejajp, nvarchar(50),>
           ,<remarkjajp, nvarchar(max),>)

";

            SqlCommand targetcmd = new SqlCommand(targetsqlstr, targetconn);
            try
            {
                targetconn.Open();
                targetcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                targetcmd.Cancel();
                targetconn.Close();
                targetconn.Dispose();
            }
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["SmoothEnterpriseSqlConnStr"].ConnectionString;
            string sqlstr = @"
SELECT
    DISTINCT a.TABLE_NAME                as 表格名稱
FROM
    INFORMATION_SCHEMA.TABLES  a
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS b ON ( a.TABLE_NAME=b.TABLE_NAME )
WHERE
    TABLE_TYPE='BASE TABLE'
AND
	a.TABLE_NAME LIKE '%'
ORDER BY a.TABLE_NAME;";

            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            try
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                listView1.View = View.Details;
                listView1.BeginUpdate();
                listView1.Columns.Add("表格名稱");


                while (dr.Read())
                {
                    var listViewItem = new ListViewItem(dr["表格名稱"].ToString());
                    listView1.Items.Add(listViewItem);
                    dgtable.Add(dr["表格名稱"].ToString());
                }

                listView1.EndUpdate();
                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

        }

    }

}
