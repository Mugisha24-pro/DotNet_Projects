<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Markets.aspx.cs" Inherits="MarketPriceTracker.Markets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Markets</h2>

    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Market name" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ValidationGroup="add"
                ErrorMessage="Name required" ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control" placeholder="District" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDistrict" ValidationGroup="add"
                ErrorMessage="District required" ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="col-md-4">
            <asp:Button ID="btnAdd" runat="server" Text="Add Market" CssClass="btn btn-primary"
                ValidationGroup="add" OnClick="btnAdd_Click" />
        </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />

    <asp:GridView ID="gvMarkets" runat="server" AutoGenerateColumns="False"
        DataKeyNames="MarketId" CssClass="table table-striped"
        OnRowEditing="gvMarkets_RowEditing"
        OnRowCancelingEdit="gvMarkets_RowCancelingEdit"
        OnRowUpdating="gvMarkets_RowUpdating"
        OnRowDeleting="gvMarkets_RowDeleting">
        <Columns>
            <asp:BoundField DataField="MarketId" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="District" HeaderText="District" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
