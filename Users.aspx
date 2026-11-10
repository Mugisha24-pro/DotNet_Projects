<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="MarketPriceTracker.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Users</h2>
    <asp:Label ID="lblMsg" runat="server" />

    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False"
        DataKeyNames="UserId" CssClass="table table-striped"
        OnRowEditing="gvUsers_RowEditing"
        OnRowCancelingEdit="gvUsers_RowCancelingEdit"
        OnRowUpdating="gvUsers_RowUpdating"
        OnRowDeleting="gvUsers_RowDeleting"
        OnRowCommand="gvUsers_RowCommand">
        <Columns>
            <asp:BoundField DataField="UserId" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="FullName" HeaderText="Full name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" NullDisplayText="-" />
            <asp:TemplateField HeaderText="Role">
                <ItemTemplate><%# Eval("Role") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control"
                        SelectedValue='<%# Eval("Role") %>'>
                        <asp:ListItem>User</asp:ListItem>
                        <asp:ListItem>Admin</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandName="ResetPwd" CommandArgument='<%# Eval("UserId") %>'
                        CausesValidation="false"
                        OnClientClick="return confirm('Reset this user password to Welcome123?');">Reset password</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
