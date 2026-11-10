<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="MarketPriceTracker.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Products</h2>

    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Product name (e.g. Tomatoes)" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ValidationGroup="add"
                ErrorMessage="Name required" ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control" placeholder="Unit (e.g. kg)" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUnit" ValidationGroup="add"
                ErrorMessage="Unit required" ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="col-md-4">
            <asp:Button ID="btnAdd" runat="server" Text="Add Product" CssClass="btn btn-primary"
                ValidationGroup="add" OnClick="btnAdd_Click" />
        </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />

    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False"
        DataKeyNames="ProductId" CssClass="table table-striped"
        OnRowEditing="gvProducts_RowEditing"
        OnRowCancelingEdit="gvProducts_RowCancelingEdit"
        OnRowUpdating="gvProducts_RowUpdating"
        OnRowDeleting="gvProducts_RowDeleting">
        <Columns>
            <asp:BoundField DataField="ProductId" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Unit" HeaderText="Unit" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
