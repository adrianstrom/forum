<%@ Page Title="Rediger Forum" Language="C#" MasterPageFile="~/Adminpanel/Admin.Master" AutoEventWireup="true" CodeBehind="edit_category.aspx.cs" Inherits="Forum_Team2.Adminpanel.Rediger_kategorier" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<br />
<div class="container">    

    <div class="row">
        <div class="col-md-6">
            <div align="center" style="height:275px;" class="panel panel-success">
                <div class="panel-heading"><p align="center">1. Legg til kategori</p></div><br />
                    <asp:Label ID="lblCategoryName" runat="server" Text="Kategorinavn: "></asp:Label>
                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control"></asp:TextBox><br />
                    
                    <asp:Label ID="lblImage" runat="server" Text="Bilde (bruk link): "></asp:Label>
                    <asp:TextBox ID="txtImage" runat="server" CssClass="form-control"></asp:TextBox><br />


                    <asp:Button ID="btnCreateCategory" runat="server" Text="Opprett kategori" CssClass="btn btn-primary" OnClick="btnCreateCategory_Click" />
                    <br />
            </div>

        </div>
        <div class="col-md-6">
            <div align="center" style="height:275px;" class="panel panel-danger">
                <div class="panel-heading"><p align="center">Slett kategori</p></div><br />
                    <asp:Label ID="lblCategoryNameDelete" runat="server" Text="Kategorinavn: "></asp:Label><br />
                    <asp:DropDownList ID="ddlCategoryNameDelete" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" runat="server" DataSourceID="sqlDeleteCategory" DataTextField="categoryname" DataValueField="categoryid"></asp:DropDownList>

                    <asp:SqlDataSource ID="sqlDeleteCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" SelectCommand="SELECT [categoryid], [categoryname] FROM [Category]"></asp:SqlDataSource>
                    <br /><br />
                    <asp:Button ID="btnDeleteCategory" runat="server" Text="Slett kategori" CssClass="btn btn-primary" OnClick="btnDeleteCategory_Click" />
                    <br />
                    <br />
            </div>

        </div>
        <div class="col-md-6">
            <div align="center" style="height:200px;" class="panel panel-success">
                <div class="panel-heading"><p align="center">2. Legg til subkategori</p></div><br />
                    <asp:Label ID="lblSubCategoryName" runat="server" Text="Subkategorinavn: "></asp:Label>
                    <asp:TextBox ID="txtSubCategoryName" runat="server" CssClass="form-control"></asp:TextBox><br />
                
                    <asp:Button ID="btnCreateSubCategory" runat="server" Text="Opprett subkategori" CssClass="btn btn-primary" OnClick="btnCreateSubCategory_Click" />

                    <br />
            </div>

        </div>
        <div class="col-md-6">
            <div align="center" style="height:200px;" class="panel panel-danger">
                <div class="panel-heading"><p align="center">Slett subkategori</p></div><br />
                    <asp:Label ID="lblSubCategoryNameDelete" runat="server" Text="Subkategorinavn: "></asp:Label><br />
                     <asp:DropDownList ID="ddlSubCategoryNameDelete" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" runat="server" DataSourceID="sqlDeleteSubCategory" DataTextField="subcategoryname" DataValueField="subcategoryid"></asp:DropDownList>
                    <asp:SqlDataSource ID="sqlDeleteSubCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" SelectCommand="SELECT [subcategoryid], [subcategoryname] FROM [SubCategory]"></asp:SqlDataSource>
                    <br /><br />
                    <asp:Button ID="btnDeleteSubCategory" runat="server" Text="Slett subkategori" CssClass="btn btn-primary" OnClick="btnDeleteSubCategory_Click" />
                    <br />
            </div>

        </div>

        <div class="col-md-6">
            <div align="center" style="height:275px;" class="panel panel-success">
                <div class="panel-heading"><p align="center">3. Få en subkategori til å høre til en kategori</p></div><br />
                    <p>Subkategorien</p><asp:DropDownList ID="ddlSubCategory" CssClass="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" runat="server" DataSourceID="sqlSubCategory" DataTextField="subcategoryname" DataValueField="subcategoryid"></asp:DropDownList>
                    <asp:SqlDataSource ID="sqlSubCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" SelectCommand="SELECT [subcategoryid], [subcategoryname] FROM [SubCategory]"></asp:SqlDataSource>
                    <br /><br />
                    <p>skal høre til kategorien</p><asp:DropDownList ID="ddlCategory" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" runat="server" DataSourceID="sqlCategory" DataTextField="categoryname" DataValueField="categoryid"></asp:DropDownList>
                    <asp:SqlDataSource ID="sqlCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" SelectCommand="SELECT [categoryid], [categoryname] FROM [Category]"></asp:SqlDataSource>
                    
                    <br />
                    <br />
                <asp:Button ID="btnCategorySubCategory" runat="server" OnClick="btnCategorySubCategory_Click" CssClass="btn btn-primary" Text="Kjør" />
            </div>

        </div>

        <div class="col-md-6">
            <div align="center" style="height:275px;" class="panel panel-danger">
                <div class="panel-heading"><p align="center">Slett emne</p></div><br />
                    <asp:SqlDataSource ID="sqlGetTopics" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" SelectCommand="SELECT [topicid], [topicname] FROM [Topic]"></asp:SqlDataSource>
                    <p>Emnenavn:</p><asp:DropDownList ID="ddlTopicDelete" CssClass="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" runat="server" DataSourceID="sqlGetTopics" DataTextField="topicname" DataValueField="topicid"></asp:DropDownList>
                    
                <br /><br />
                <asp:Button ID="btnDeleteTopic" runat="server" OnClick="btnDeleteTopic_Click" CssClass="btn btn-primary" Text="Kjør" />
            </div>

        </div>

                <div class="col-md-6">
            <div align="center" style="height:275px;" class="panel panel-danger">
                <div class="panel-heading"><p align="center">Slett kommentar</p></div><br />
                    <asp:SqlDataSource ID="sqlGetComments" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" SelectCommand="SELECT [commentid], [commenttext] FROM [Comment]"></asp:SqlDataSource>
                    <p>Kommentartekst:</p><asp:DropDownList ID="ddlCommentDelete" CssClass="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" runat="server" DataSourceID="sqlGetComments" DataTextField="commenttext" DataValueField="commentid"></asp:DropDownList>
                    
                <br /><br />
                <asp:Button ID="btnDeleteComment" runat="server" OnClick="btnDeleteComment_Click" CssClass="btn btn-primary" Text="Kjør" />
            </div>

        </div>

    </div>

</div>
   
</asp:Content>

               
