<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compare.aspx.cs" Inherits="MarketPriceTracker.Compare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Compare Prices</h2>
    <p class="text-muted">Based on the prices reported by all users.</p>

    <h5 class="mt-4">Price summary by product</h5>
    <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="False"
        CssClass="table table-striped" EmptyDataText="No data yet.">
        <Columns>
            <asp:BoundField DataField="Product" HeaderText="Product" />
            <asp:BoundField DataField="Unit" HeaderText="Unit" />
            <asp:BoundField DataField="Reports" HeaderText="Reports" />
            <asp:BoundField DataField="Lowest" HeaderText="Lowest (RWF)" DataFormatString="{0:N0}" NullDisplayText="-" />
            <asp:BoundField DataField="Average" HeaderText="Average (RWF)" DataFormatString="{0:N0}" NullDisplayText="-" />
            <asp:BoundField DataField="Highest" HeaderText="Highest (RWF)" DataFormatString="{0:N0}" NullDisplayText="-" />
        </Columns>
    </asp:GridView>

    <h5 class="mt-4">Find the cheapest market</h5>
    <div class="row mb-3">
        <div class="col-md-4">
            <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control"
                AutoPostBack="true" AppendDataBoundItems="true"
                OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                <asp:ListItem Value="">-- Select product --</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>

    <asp:Label ID="lblCheapest" runat="server" CssClass="alert alert-success d-block" Visible="false" />

    <asp:GridView ID="gvMarkets" runat="server" AutoGenerateColumns="False"
        CssClass="table table-striped" Visible="false"
        EmptyDataText="No reports for this product yet."
        OnRowDataBound="gvMarkets_RowDataBound">
        <Columns>
            <asp:BoundField DataField="Market" HeaderText="Market" />
            <asp:BoundField DataField="District" HeaderText="District" />
            <asp:BoundField DataField="Reports" HeaderText="Reports" />
            <asp:BoundField DataField="Lowest" HeaderText="Lowest (RWF)" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="Average" HeaderText="Average (RWF)" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="LastReported" HeaderText="Last reported" DataFormatString="{0:yyyy-MM-dd}" />
        </Columns>
    </asp:GridView>
</asp:Content>
