<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="MarketPriceTracker.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <style>
        .profile-card input { max-width: 100%; }
    </style>

    <h2>My Profile</h2>

    <div class="row g-4 mt-1">
        <!-- Personal details -->
        <div class="col-lg-6">
            <div class="card profile-card">
                <div class="card-body p-4">
                    <h5 class="mb-3">Personal details</h5>

                    <div class="mb-3">
                        <asp:Label runat="server" Text="Full name" />
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ValidationGroup="profile"
                            ErrorMessage="Name is required" ForeColor="Red" Display="Dynamic" />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" Text="Email" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="profile"
                            ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" Text="Phone (optional)" />
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <span class="text-muted">Role:</span>
                        <asp:Label ID="lblRole" runat="server" CssClass="badge bg-primary" />
                    </div>

                    <asp:Button ID="btnSave" runat="server" Text="Save changes" CssClass="btn btn-primary"
                        ValidationGroup="profile" OnClick="btnSave_Click" />
                    <div class="mt-2"><asp:Label ID="lblProfileMsg" runat="server" /></div>
                </div>
            </div>
        </div>

        <!-- Change password -->
        <div class="col-lg-6">
            <div class="card profile-card">
                <div class="card-body p-4">
                    <h5 class="mb-3">Change password</h5>

                    <div class="mb-3">
                        <asp:Label runat="server" Text="Current password" />
                        <asp:TextBox ID="txtCurrent" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCurrent" ValidationGroup="password"
                            ErrorMessage="Enter your current password" ForeColor="Red" Display="Dynamic" />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" Text="New password (min. 6 characters)" />
                        <asp:TextBox ID="txtNew" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNew" ValidationGroup="password"
                            ErrorMessage="Enter a new password" ForeColor="Red" Display="Dynamic" />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" Text="Confirm new password" />
                        <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:CompareValidator runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtNew"
                            ValidationGroup="password" ErrorMessage="Passwords do not match" ForeColor="Red" Display="Dynamic" />
                    </div>

                    <asp:Button ID="btnPassword" runat="server" Text="Change password" CssClass="btn btn-primary"
                        ValidationGroup="password" OnClick="btnPassword_Click" />
                    <div class="mt-2"><asp:Label ID="lblPwdMsg" runat="server" /></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
