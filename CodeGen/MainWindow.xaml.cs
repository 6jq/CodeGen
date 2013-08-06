using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
				table = ExecuteTable(sql);
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
		private DataTable ExecuteTable(string sql)
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
			switch (codeType)
			{
				case "Model":
					GenerateModelCode(tableName);
					break;
				case "DAL":
					GenerateDalCode(tableName);
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
			DataTable table = ExecuteTable(sql);
			StringBuilder sb = new StringBuilder();
			sb.Append("public class ").Append(tableName).AppendLine(" {");
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
			DataTable table = ExecuteTable(sql);
			StringBuilder sb = new StringBuilder();
			sb.Append("public class ").Append(tableName).Append("DAL {\n\n");

			#region //FromDbValue方法开始
			sb.Append("private object FromDbValue(object obj) {\n")
				.Append("if (obj == DBNull.Value) return null;\n")
				.Append("else return obj;\n}\n\n");
			#endregion //FromDbValue方法结束

			#region //ToModel方法开始
			sb.Append("public ").Append(tableName).Append(" ToModel(DataRow row){\n");
			sb.Append(tableName).Append(" model = new ").Append(tableName).Append("();\n");
			foreach (DataColumn column in table.Columns)
			{
				sb.Append("model.").Append(column.ColumnName).Append(" = (")
					.Append(GetDataType(column)).Append(")FromDbValue(row[\"")
					.Append(column.ColumnName).Append("\"]);\n");
			}
			sb.Append("return model;\n}\n\n");
			#endregion//ToMedel方法结束

			#region //ListAll方法开始
			sb.Append("public List<").Append(tableName).Append("> ListAll(){\n");
			sb.Append("List<").Append(tableName).Append("> list = new List<")
				.Append(tableName).Append(">();\n");
			sb.Append("string sql = @\"select * from ").Append(tableName).Append("\";\n");
			sb.Append("DataTable dt = SqlHelper.ExecuteTable(sql);\n");
			sb.Append("foreach (DataRow row in dt.Rows){\n");
			sb.Append(tableName).Append(" model = ToModel(row);\n");
			sb.Append("list.Add(model);\n}\n");
			sb.Append("return list;\n}\n\n");
			#endregion //ListAll方法结束

			sb.AppendLine("}"); //Dal类结束
			return sb.ToString();
		}

	}
}
