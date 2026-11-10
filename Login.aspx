<%@ Page Title="" Language="C#" MasterPageFile="~/Auth.Master" UnobtrusiveValidationMode="None" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MarketPriceTracker.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="text-center mb-4">Login</h3>
    <div class="mb-3">
        <asp:Label runat="server" Text="Email" />
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
            ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" />
    </div>
    <div class="mb-3">
        <asp:Label runat="server" Text="Password" />
        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
            ErrorMessage="Password is required" ForeColor="Red" Display="Dynamic" />
    </div>
    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100"
        OnClick="btnLogin_Click" />
    <div class="text-center mt-2">
        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />
    </div>
    <p class="text-center mt-3 mb-0">
        Don't have an account? <a href="Signup.aspx">Click here to sign up</a>
    </p>
</asp:Content>
