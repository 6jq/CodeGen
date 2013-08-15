using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeGen
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		private static string modelName = "";
		public MainWindow()
		{
			InitializeComponent();
		}

		private void txtConnStr_TextChanged(object sender, TextChangedEventArgs e)
		{
			cmbCodeTypes.IsEnabled = false;
			cmbTables.IsEnabled = false;
			btnGen.IsEnabled = false;
		}

		private void btnConn_Click(object sender, RoutedEventArgs e)
		{
			string sql = @"select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_TYPE='BASE TABLE'";
			DataTable table;
			try
			{
				table = ExecuteDataTable(sql);
			}
			catch (Exception ex)
			{
				MessageBox.Show("数据库链接失败！\n" + ex.Message);
				return;
			}
			string[] tables = new string[table.Rows.Count];
			for (int i = 0; i < tables.Length; i++)
			{
				DataRow row = table.Rows[i];
				tables[i] = (string)row["TABLE_NAME"];
			}
			cmbTables.ItemsSource = tables;
			cmbTables.SelectedIndex = 0;
			cmbTables.IsEnabled = true;
			string[] codeTypes = {"Model","DAL" };
			cmbCodeTypes.ItemsSource = codeTypes;
			cmbCodeTypes.SelectedIndex = 0;
			cmbCodeTypes.IsEnabled = true;
			btnGen.IsEnabled = true;
		}

		/// <summary>
		/// 执行sql代码并返回结果集的表
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <returns></returns>
		private DataTable ExecuteDataTable(string sql)
		{
			using (SqlConnection conn = new SqlConnection(txtConnStr.Text))
			{
				conn.Open();
				using (SqlCommand cmd = conn.CreateCommand())
				{
					cmd.CommandText = sql;
					SqlDataAdapter adapter = new SqlDataAdapter(cmd);
					DataTable table = new DataTable();
					adapter.FillSchema(table, SchemaType.Source);
					adapter.Fill(table);
					return table;
				}
			}
		}

		private void btnGen_Click(object sender, RoutedEventArgs e)
		{
			string tableName = (string)cmbTables.SelectedItem;
			string codeType = (string)cmbCodeTypes.SelectedItem;
			if (txtModelName.Text == "") modelName = tableName;
			else modelName = txtModelName.Text;
			switch (codeType)
			{
				case "Model":
					txtCode.Text = GenerateModelCode(tableName);
					break;
				case "DAL":
					txtCode.Text = GenerateDalCode(tableName);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 根据表名生成Model层的代码
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns>生成的代码</returns>
		private string GenerateModelCode(string tableName)
		{
			string sql = @"select top 0 * from " + tableName;
			DataTable table = ExecuteDataTable(sql);
			StringBuilder sb = new StringBuilder();
			sb.Append("public class ").Append(modelName).AppendLine(" {");
			foreach (DataColumn column in table.Columns)
			{
				sb.Append("public ").Append(GetDataType(column)).Append(" ")
					.Append(column.ColumnName).AppendLine(" {get;set;}");
			}
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string GetDataType(DataColumn column)
		{
			//如果列是可空类型并且对应的C#类型为值类型，则类型后面加上?
			if (column.AllowDBNull && column.DataType.IsValueType)
				return column.DataType + "?";
			else
				return column.DataType.ToString();
		}

		/// <summary>
		/// 根据表名生成DAL层的代码
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns>生成的代码</returns>
		private string GenerateDalCode(string tableName)
		{
			string sql = @"select top 0 * from " + tableName;
			DataTable table = ExecuteDataTable(sql);
			StringBuilder sb = new StringBuilder();
			sb.Append("public class ").Append(modelName).Append("DAL {\n\n");

			#region //FromDbValue方法开始
			sb.Append("private object FromDbValue(object obj) {\n")
				.Append("if (obj == DBNull.Value) return null;\n")
				.Append("else return obj;\n}\n\n");
			#endregion //FromDbValue方法结束

			#region //ToModel方法开始
			sb.Append("private ").Append(modelName).Append(" ToModel(DataRow row){\n");
			sb.Append(modelName).Append(" model = new ").Append(modelName).Append("();\n");
			foreach (DataColumn column in table.Columns)
			{
				sb.Append("model.").Append(column.ColumnName).Append(" = (")
					.Append(GetDataType(column)).Append(")FromDbValue(row[\"")
					.Append(column.ColumnName).Append("\"]);\n");
			}
			sb.Append("return model;\n}\n\n");
			#endregion//ToMedel方法结束

			#region //ListAll方法开始
			sb.Append("public List<").Append(modelName).Append("> ListAll(){\n");
			sb.Append("List<").Append(modelName).Append("> list = new List<")
				.Append(modelName).Append(">();\n");
			sb.Append("string sql = @\"select * from ").Append(tableName).Append("\";\n");
			sb.Append("DataTable dt = SqlHelper.ExecuteDataTable(sql);\n");
			sb.Append("foreach (DataRow row in dt.Rows){\n");
			sb.Append(modelName).Append(" model = ToModel(row);\n");
			sb.Append("list.Add(model);\n}\n");
			sb.Append("return list;\n}\n\n");
			#endregion //ListAll方法结束

			#region //Insert方法开始
			sb.Append("public int Insert(").Append(modelName).Append(" model){\n");
			sb.Append("string  sql = @\"insert into ").Append(tableName).Append("(");
			List<string> tempList = new List<string>();
			foreach (DataColumn col in table.Columns)
			{
				tempList.Add(col.ColumnName);
			}
			sb.Append(string.Join(",", tempList)).Append(")values(");
			tempList.Clear();
			foreach (DataColumn col in table.Columns)
			{
				tempList.Add("@" + col.ColumnName);
			}
			sb.Append(string.Join(",", tempList)).Append(")\";\n");
			sb.Append("List<SqlParameter> parameters = new List<SqlParameter>();\n");
			foreach (DataColumn col in table.Columns)
			{
				sb.Append("parameters.Add(new SqlParameter(\"@").Append(col.ColumnName)
					.Append("\",model.").Append(col.ColumnName).Append("));\n");
			}
			sb.Append("return SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());\n}\n\n");
			#endregion //Insert方法结束

			#region //Update方法开始
			sb.Append("public int Update(").Append(modelName).Append(" model){\n");
			sb.Append("string sql = @\"update ").Append(tableName).Append(" set ");
			tempList.Clear();
			foreach (DataColumn col in table.Columns)
			{
				tempList.Add(col.ColumnName + "=@" + col.ColumnName);
			}
			sb.Append(string.Join(",", tempList)).Append(" where Id=@Id\";\n");
			sb.Append("List<SqlParameter> parameters = new List<SqlParameter>();\n");
			foreach (DataColumn col in table.Columns)
			{
				sb.Append("parameters.Add(new SqlParameter(\"@").Append(col.ColumnName)
					.Append("\",model.").Append(col.ColumnName).Append("));\n");
			}
			sb.Append("return SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());\n}\n\n");
			#endregion //Update方法结束

			#region //Delete方法开始
			sb.Append("public int Delete(").Append(modelName).Append(" model){\n");
			sb.Append("string sql=@\"delete from ").Append(tableName).Append(" where Id=@Id\";\n");
			sb.Append("SqlParameter parameter = new SqlParameter(\"@Id\",model.Id);\n");
			sb.Append("return SqlHelper.ExecuteNonQuery(sql,parameter);\n}\n\n");
			#endregion //Delete方法结束

			#region //GetById方法开始
			sb.Append("public ").Append(modelName).Append(" GetById(Guid id){\n");
			sb.Append("string sql=@\"select * from ").Append(tableName).Append(" where Id=@Id\";\n");
			sb.Append("SqlParameter parameter = new SqlParameter(\"@Id\",id);\n");
			sb.Append("DataTable table = SqlHelper.ExecuteDataTable(sql,parameter);\n");
			sb.Append("return ToModel(table.Rows[0]);\n}\n\n");
			#endregion //GetById方法结束

			sb.AppendLine("}"); //Dal类结束
			return sb.ToString();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (File.Exists("CodeGen.ini"))
			{
				//读取配置
				//todo : 使用Linq
				string[] configs = File.ReadAllLines("CodeGen.ini");
				this.txtConnStr.Text = configs[0];
			}
		}

	}
}
