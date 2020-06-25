<%@ Page Title="Rediger Bruker" Language="C#" MasterPageFile="~/Adminpanel/Admin.Master" AutoEventWireup="true" CodeBehind="edit_user.aspx.cs" Inherits="Forum_Team2.Adminpanel.Rediger_bruker" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<br />
<div class="container">    

    <div class="row">
        <div class="col-md-6">
            <div style="height:480px;" align="center" class="panel panel-primary">
                <div class="panel-heading"><p align="center">Oppdater bruker</p></div><br />
                    <asp:Label ID="lblUsername" runat="server" Text="Brukernavn: "></asp:Label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblPassword" runat="server" Text="Passord: "></asp:Label>
                    <asp:TextBox ID="txtPassword" type="password" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblGender" runat="server" Text="Kjønn: "></asp:Label>
                    <asp:TextBox ID="txtGender" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblEducation" runat="server" Text="Utdanning: "></asp:Label>
                    <asp:TextBox ID="txtEducation" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblDescription" runat="server" Text="Beskrivelse: "></asp:Label>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Button ID="btnUpdateProfile" runat="server" Text="Oppdater bruker" OnClick="btnUpdateProfile_Click" CssClass="btn-primary" />
                    <br />
            </div>

        </div>
        <div class="col-md-6">
            <div style="height:250px;" align="center" class="panel panel-primary">
                <div class="panel-heading"><p align="center">Endre rolle</p></div><br />
                    <asp:Label ID="lblUsernameRole" runat="server" Text="Brukernavn: "></asp:Label>
                    <asp:TextBox ID="txtUsernameRole" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblRole" runat="server" Text="Rolle: "></asp:Label><br />
                    <asp:DropDownList class="btn btn-default dropdown-toggle" ID="ddlRole" runat="server" DataSourceID="sqlSetRole" DataTextField="rolename" DataValueField="roleid"></asp:DropDownList>

                    
                    <asp:SqlDataSource ID="sqlSetRole" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" SelectCommand="SELECT [roleid], [rolename] FROM [Role]"></asp:SqlDataSource>

                    <br /><br />
                    <asp:Button ID="btnEditRole" runat="server" Text="Endre rolle" OnClick="btnEditRole_Click" CssClass="btn-primary" />
                    <br />
            </div>

        </div>

        <div class="col-md-6">
            <div style="height:210px;" align="center" class="panel panel-primary">
                <div class="panel-heading"><p align="center">Statistikk</p></div><br />
                    

       
                <asp:Chart ID="chrtRoles" runat="server" DataSourceID="sqlRoles" Height="100px">
                    <Series>
                        <asp:Series Name="Series1" XValueMember="rolename" YValueMembers="antall">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <asp:SqlDataSource ID="sqlRoles" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" 
                    SelectCommand="SELECT COUNT(Role.rolename) as antall, rolename FROM Role, Users
                                   WHERE Users.roleid = Role.roleid
                                   GROUP BY rolename
                    "></asp:SqlDataSource>
                    

       
            </div>

        </div>
    </div>

</div>
   
</asp:Content>

               
