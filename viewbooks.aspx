<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="viewbooks.aspx.cs" Inherits="ElibraryManagementSystem.viewbooks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">

                        <!-- Heading -->
                        <div class="row">
                            <div class="col">
                                <center>
                                    <h3>Book Inventory List</h3>
                                </center>
                            </div> 
                        </div>

                        <div class="row">
                            <div class="col">
                                <hr />
                            </div>
                        </div>

                        <!-- GridView -->
                        <div class="row">
                            <div class="col">
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:elibraryDBConnectionString %>" 
                                    SelectCommand="SELECT * FROM [book_master_tbl]">
                                </asp:SqlDataSource>

                                <asp:GridView CssClass="table table-striped table-bordered" 
                                    ID="GridView1" runat="server" 
                                    AutoGenerateColumns="False" 
                                    DataKeyNames="book_id" 
                                    DataSourceID="SqlDataSource1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" SortExpression="book_id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookID" runat="server" Text='<%# Eval("book_id") %>' Font-Bold="True"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-lg-10">
                                                            <div class="row">
                                                                <div class="col-12">
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("book_name") %>' Font-Bold="True" Font-Size="X-Large"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-12">
                                                                    <span>Author - </span>
                                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text='<%# Eval("author_name")%>'></asp:Label>
                                                                    &nbsp; | <span>Genre - </span>
                                                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text='<%# Eval("genre") %>'></asp:Label>
                                                                    &nbsp; | <span>Language - </span>
                                                                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Text='<%# Eval("language") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-12">
                                                                    <span>Publisher - </span>
                                                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Text='<%# Eval("publisher_name")%>'></asp:Label>
                                                                    &nbsp; | <span>Publish Date - </span>
                                                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Text='<%# Eval("publish_date") %>'></asp:Label>
                                                                    &nbsp; | <span>Pages - </span>
                                                                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Text='<%# Eval("no_of_page") %>'></asp:Label>
                                                                    &nbsp; | <span>Edition - </span>
                                                                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Text='<%# Eval("edition") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-12">
                                                                    <span>Cost - </span>
                                                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Text='<%# Eval("book_cost")%>'></asp:Label>
                                                                    &nbsp; | <span>Actual Stock - </span>
                                                                    <asp:Label ID="Label10" runat="server" Font-Bold="True" Text='<%# Eval("actual_stock") %>'></asp:Label>
                                                                    &nbsp; | <span>Available - </span>
                                                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Text='<%# Eval("current_stock") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-12">
                                                                    <span>Description - </span>
                                                                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Italic="True" Text='<%# Eval("book_description") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:Image ID="Image1" CssClass="img-fluid" runat="server" ImageUrl='<%# Eval("book_img_link") %>' />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                    </div>
                </div>

                <!-- Back to Home -->
                <div>
                    <center>
                        <a href="homepage.aspx"><< Back to Home</a><br><br>
                    </center>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
