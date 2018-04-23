using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using DevExpress.Web.ASPxGridView;

public partial class _Default : System.Web.UI.Page 
{
    ASPxGridView masterGrid;
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["CategoryID"] = 4;
        CreateMasterGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        masterGrid.DataBind();
    }

    private void CreateMasterGrid()
    {
        masterGrid = new ASPxGridView();
        masterGrid.ID = "masterGrid";
        masterGrid.AutoGenerateColumns = false;
        form1.Controls.Add(masterGrid);

        CreateMasterColumns(masterGrid);
        masterGrid.SettingsDetail.ShowDetailRow = true;
        masterGrid.KeyFieldName = "CategoryID";
        masterGrid.DataSource = GetMasterDataSource();

        masterGrid.Templates.DetailRow = new ProductsDetailGridTemplate();

    }

    private DataTable GetMasterDataSource()
    {
        DataTable table = new DataTable("Categories");
        string query = "SELECT [CategoryID], [CategoryName], [Description] FROM [Categories]";
        SqlCommand cmd = new SqlCommand(query, DataHelper.GetConnection());
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(table);
        return table;
    }


    private void CreateMasterColumns(ASPxGridView masterGrid)
    {
        masterGrid.Columns.Add(new GridViewDataColumn("CategoryID"));
        masterGrid.Columns.Add(new GridViewDataColumn("CategoryName"));
        masterGrid.Columns.Add(new GridViewDataColumn("Description"));
    }
}

internal static class DataHelper
{
    public static SqlConnection GetConnection()
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(connStr);
        return conn;
    }
}
public class ProductsDetailGridTemplate : ITemplate
{
    Control parent;
    object masterKey;
    ASPxGridView detailGrid;

    public void InstantiateIn(Control container)
    {
        parent = container;
        masterKey = ((GridViewDetailRowTemplateContainer)parent).KeyValue;
        CreateDetailGrid();
    }

    private void CreateDetailGrid()
    {
        detailGrid = new ASPxGridView();
        detailGrid.ID = "detailGrid";
        detailGrid.AutoGenerateColumns = false;
        detailGrid.SettingsDetail.ShowDetailRow = true;
        parent.Controls.Add(detailGrid);

        CreateDetailColumns(detailGrid);
        detailGrid.KeyFieldName = "ProductID";
        detailGrid.DataSource = GetDetailDataSource();
        detailGrid.DataBind();

        detailGrid.Templates.DetailRow = new OrderDetailsDetailGridTemplate();
    }

    private DataTable GetDetailDataSource()
    {
        DataTable table = new DataTable("Products");
        string query = "SELECT [ProductID], [ProductName], [SupplierID], [CategoryID], [UnitPrice], [Discontinued] FROM [Products] WHERE ([CategoryID] = @CategoryID)";
        SqlCommand cmd = new SqlCommand(query, DataHelper.GetConnection());
        cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = masterKey;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(table);
        return table;
    }

    private void CreateDetailColumns(ASPxGridView detailGrid)
    {
        detailGrid.Columns.Add(new GridViewDataColumn("ProductID"));
        detailGrid.Columns.Add(new GridViewDataColumn("ProductName"));
        detailGrid.Columns.Add(new GridViewDataColumn("SupplierID"));
        detailGrid.Columns.Add(new GridViewDataColumn("CategoryID"));
        detailGrid.Columns.Add(new GridViewDataColumn("UnitPrice"));
    }

}

public class OrderDetailsDetailGridTemplate : ITemplate
{
    Control parent;
    object masterKey;
    ASPxGridView detailGrid;

    public void InstantiateIn(Control container)
    {
        parent = container;
        masterKey = ((GridViewDetailRowTemplateContainer)parent).KeyValue;
        CreateDetailGrid();
    }

    private void CreateDetailGrid()
    {
        detailGrid = new ASPxGridView();
        detailGrid.ID = "OrderDetailsDetailGrid";
        detailGrid.AutoGenerateColumns = false;
        parent.Controls.Add(detailGrid);

        CreateDetailColumns(detailGrid);
        detailGrid.KeyFieldName = "OrderID";
        detailGrid.DataSource = GetDetailDataSource();
        detailGrid.DataBind();
    }

    private DataTable GetDetailDataSource()
    {
        DataTable table = new DataTable("Orders");
        string query = "SELECT [OrderID], [ProductID], [UnitPrice], [Quantity] FROM [Order Details] WHERE ([ProductID] = @ProductID)";
        SqlCommand cmd = new SqlCommand(query, DataHelper.GetConnection());
        cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = masterKey;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(table);
        return table;
    }

    private void CreateDetailColumns(ASPxGridView detailGrid)
    {
        detailGrid.Columns.Add(new GridViewDataColumn("OrderID"));
        detailGrid.Columns.Add(new GridViewDataColumn("ProductID"));
        detailGrid.Columns.Add(new GridViewDataColumn("UnitPrice"));
        detailGrid.Columns.Add(new GridViewDataColumn("Quantity"));
    }
}
