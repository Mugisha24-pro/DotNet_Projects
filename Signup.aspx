<%@ Page Title="" Language="C#" MasterPageFile="~/Auth.Master" UnobtrusiveValidationMode="None" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="MarketPriceTracker.Signup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="text-center mb-4">Create Account</h3>
    <div class="mb-3">
        <asp:Label runat="server" Text="Full name" />
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName"
            ErrorMessage="Name is required" ForeColor="Red" Display="Dynamic" />
    </div>
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
    <div class="mb-3">
        <asp:Label runat="server" Text="Confirm password" />
        <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" />
        <asp:CompareValidator runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtPassword"
            ErrorMessage="Passwords do not match" ForeColor="Red" Display="Dynamic" />
    </div>
    <asp:Button ID="btnSignup" runat="server" Text="Sign up" CssClass="btn btn-primary w-100"
        OnClick="btnSignup_Click" />
    <div class="text-center mt-2">
        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />
    </div>
    <p class="text-center mt-3 mb-0">
        Already have an account? <a href="Login.aspx">Click here to login</a>
    </p>
</asp:Content>
