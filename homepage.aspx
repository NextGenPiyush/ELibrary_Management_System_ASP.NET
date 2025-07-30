<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="ElibraryManagementSystem.homepage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section>
        <img src="imgs/library-img.jpeg" class="img-fluid"/>
    </section>

    <section>
        <div class="container">

            <div class="row">
                <div class="col-12">
                    <center>
                        <h2>Our Features</h2>
                        <p><b>Our 3 Primary Features -</b></p>
                    </center>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <center>
                        <img width="150px" src="imgs/digitalBookInventory.jpeg" />
                        <h4>Digital Book Inventory</h4>
                        <p class="text-justify">A Digital Book Inventory is a system that tracks, manages, and organizes eBooks and digital publications for easy access and control.</p>
                    </center>
                </div>
                <div class="col-md-4">
                    <center>
                        <img width="205px" src="imgs/searchbook.jpeg" />
                        <h4>Search Book</h4>
                        <p class="text-justify">Search and find books instantly by title, author, or keyword.</p>
                    </center>
                </div>
                <div class="col-md-4">
                    <center>
                        <img width="265px" src="imgs/triangleExclamation.jpeg" />
                        <h4>Defaulter List</h4>
                        <p class="text-justify">View a list of users with overdue book returns or pending fines.</p>
                    </center>
                </div>
            </div>

        </div>

    </section>

    <section>
        <img src="imgs/Library2.jpeg" class="img-fluid"/>
    </section>

    <section>
        <div class="container">

            <div class="row">
                <div class="col-12">
                    <center>
                        <h2>Our Process</h2>
                        <p><b>We have a simple 3 step process -</b></p>
                    </center>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <center>
                        <img width="280px" src="imgs/signUp.jpg" />
                        <h4>Sign up</h4>
                        <p class="text-justify">Create a new account to access and manage your digital library.</p>
                    </center>
                </div>
                <div class="col-md-4">
                    <center>
                        <img width="205px" src="imgs/searchbook.jpeg" />
                        <h4>Search Book</h4>
                        <p class="text-justify">Search and find books instantly by title, author, or keyword.</p>
                    </center>
                </div>
                <div class="col-md-4">
                    <center>
                        <img width="200px" src="imgs/visitUs.jpg" />
                        <h4>Visit us</h4>
                        <p class="text-justify">Get our location and contact details to visit us in person.</p>
                    </center>
                </div>
            </div>

        </div>

    </section>

</asp:Content>
