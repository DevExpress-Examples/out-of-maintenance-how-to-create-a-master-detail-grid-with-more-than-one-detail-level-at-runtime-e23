Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports DevExpress.Web.ASPxGridView

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private masterGrid As ASPxGridView
	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		Session("CategoryID") = 4
		CreateMasterGrid()
	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		masterGrid.DataBind()
	End Sub

	Private Sub CreateMasterGrid()
		masterGrid = New ASPxGridView()
		masterGrid.ID = "masterGrid"
		masterGrid.AutoGenerateColumns = False
		form1.Controls.Add(masterGrid)

		CreateMasterColumns(masterGrid)
		masterGrid.SettingsDetail.ShowDetailRow = True
		masterGrid.KeyFieldName = "CategoryID"
		masterGrid.DataSource = GetMasterDataSource()

		masterGrid.Templates.DetailRow = New ProductsDetailGridTemplate()

	End Sub

	Private Function GetMasterDataSource() As DataTable
		Dim table As New DataTable("Categories")
		Dim query As String = "SELECT [CategoryID], [CategoryName], [Description] FROM [Categories]"
		Dim cmd As New SqlCommand(query, DataHelper.GetConnection())
		Dim da As New SqlDataAdapter(cmd)
		da.Fill(table)
		Return table
	End Function


	Private Sub CreateMasterColumns(ByVal masterGrid As ASPxGridView)
		masterGrid.Columns.Add(New GridViewDataColumn("CategoryID"))
		masterGrid.Columns.Add(New GridViewDataColumn("CategoryName"))
		masterGrid.Columns.Add(New GridViewDataColumn("Description"))
	End Sub
End Class

Friend NotInheritable Class DataHelper
	Private Sub New()
	End Sub
	Public Shared Function GetConnection() As SqlConnection
		Dim connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings("NorthwindConnectionString").ConnectionString
		Dim conn As New SqlConnection(connStr)
		Return conn
	End Function
End Class
Public Class ProductsDetailGridTemplate
	Implements ITemplate
	Private parent As Control
	Private masterKey As Object
	Private detailGrid As ASPxGridView

	Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
		parent = container
		masterKey = (CType(parent, GridViewDetailRowTemplateContainer)).KeyValue
		CreateDetailGrid()
	End Sub

	Private Sub CreateDetailGrid()
		detailGrid = New ASPxGridView()
		detailGrid.ID = "detailGrid"
		detailGrid.AutoGenerateColumns = False
		detailGrid.SettingsDetail.ShowDetailRow = True
		parent.Controls.Add(detailGrid)

		CreateDetailColumns(detailGrid)
		detailGrid.KeyFieldName = "ProductID"
		detailGrid.DataSource = GetDetailDataSource()
		detailGrid.DataBind()

		detailGrid.Templates.DetailRow = New OrderDetailsDetailGridTemplate()
	End Sub

	Private Function GetDetailDataSource() As DataTable
		Dim table As New DataTable("Products")
		Dim query As String = "SELECT [ProductID], [ProductName], [SupplierID], [CategoryID], [UnitPrice], [Discontinued] FROM [Products] WHERE ([CategoryID] = @CategoryID)"
		Dim cmd As New SqlCommand(query, DataHelper.GetConnection())
		cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = masterKey
		Dim da As New SqlDataAdapter(cmd)
		da.Fill(table)
		Return table
	End Function

	Private Sub CreateDetailColumns(ByVal detailGrid As ASPxGridView)
		detailGrid.Columns.Add(New GridViewDataColumn("ProductID"))
		detailGrid.Columns.Add(New GridViewDataColumn("ProductName"))
		detailGrid.Columns.Add(New GridViewDataColumn("SupplierID"))
		detailGrid.Columns.Add(New GridViewDataColumn("CategoryID"))
		detailGrid.Columns.Add(New GridViewDataColumn("UnitPrice"))
	End Sub

End Class

Public Class OrderDetailsDetailGridTemplate
	Implements ITemplate
	Private parent As Control
	Private masterKey As Object
	Private detailGrid As ASPxGridView

	Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
		parent = container
		masterKey = (CType(parent, GridViewDetailRowTemplateContainer)).KeyValue
		CreateDetailGrid()
	End Sub

	Private Sub CreateDetailGrid()
		detailGrid = New ASPxGridView()
		detailGrid.ID = "OrderDetailsDetailGrid"
		detailGrid.AutoGenerateColumns = False
		parent.Controls.Add(detailGrid)

		CreateDetailColumns(detailGrid)
		detailGrid.KeyFieldName = "OrderID"
		detailGrid.DataSource = GetDetailDataSource()
		detailGrid.DataBind()
	End Sub

	Private Function GetDetailDataSource() As DataTable
		Dim table As New DataTable("Orders")
		Dim query As String = "SELECT [OrderID], [ProductID], [UnitPrice], [Quantity] FROM [Order Details] WHERE ([ProductID] = @ProductID)"
		Dim cmd As New SqlCommand(query, DataHelper.GetConnection())
		cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = masterKey
		Dim da As New SqlDataAdapter(cmd)
		da.Fill(table)
		Return table
	End Function

	Private Sub CreateDetailColumns(ByVal detailGrid As ASPxGridView)
		detailGrid.Columns.Add(New GridViewDataColumn("OrderID"))
		detailGrid.Columns.Add(New GridViewDataColumn("ProductID"))
		detailGrid.Columns.Add(New GridViewDataColumn("UnitPrice"))
		detailGrid.Columns.Add(New GridViewDataColumn("Quantity"))
	End Sub
End Class
