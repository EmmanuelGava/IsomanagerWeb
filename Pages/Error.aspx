<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Error.aspx.cs" 
    Inherits="IsomanagerWeb.Pages.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <div class="card border-danger">
                    <div class="card-header bg-danger text-white">
                        <h2 class="mb-0"><i class="fas fa-exclamation-triangle"></i> Error</h2>
                    </div>
                    <div class="card-body">
                        <div class="alert alert-danger">
                            <asp:Label ID="lblError" runat="server" CssClass="h5"></asp:Label>
                        </div>
                        
                        <asp:Label ID="lblErrorDetails" runat="server" CssClass="text-muted small" Visible="false"></asp:Label>
                        
                        <div class="mt-4">
                            <asp:Button ID="btnVolver" runat="server" Text="Volver al Inicio" 
                                CssClass="btn btn-primary" OnClick="btnVolver_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 