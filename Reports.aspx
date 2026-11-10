<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="MarketPriceTracker.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Price Reports</h2>

    <div class="row mb-3">
        <div class="col-md-3">
            <asp:DropDownList ID="ddlMarket" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Value="">-- Select market --</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlMarket" InitialValue=""
                ValidationGroup="add" ErrorMessage="Select a market" ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Value="">-- Select product --</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlProduct" InitialValue=""
                ValidationGroup="add" ErrorMessage="Select a product" ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Price (RWF)" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrice" ValidationGroup="add"
                ErrorMessage="Price required" ForeColor="Red" Display="Dynamic" />
            <asp:CompareValidator runat="server" ControlToValidate="txtPrice" ValidationGroup="add"
                Operator="GreaterThan" ValueToCompare="0" Type="Double"
                ErrorMessage="Enter a valid price" ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="col-md-3">
            <asp:Button ID="btnAdd" runat="server" Text="Add Report" CssClass="btn btn-primary"
                ValidationGroup="add" OnClick="btnAdd_Click" />
        </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />

    <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="False"
        DataKeyNames="ReportId" CssClass="table table-striped"
        EmptyDataText="No price reports yet."
        OnRowEditing="gvReports_RowEditing"
        OnRowCancelingEdit="gvReports_RowCancelingEdit"
        OnRowUpdating="gvReports_RowUpdating"
        OnRowDeleting="gvReports_RowDeleting">
        <Columns>
            <asp:BoundField DataField="ReportId" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="Reporter" HeaderText="Reported by" ReadOnly="True" />
            <asp:BoundField DataField="Market" HeaderText="Market" ReadOnly="True" />
            <asp:BoundField DataField="Product" HeaderText="Product" ReadOnly="True" />
            <asp:BoundField DataField="Price" HeaderText="Price (RWF)" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="ReportDate" HeaderText="Date" ReadOnly="True" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
