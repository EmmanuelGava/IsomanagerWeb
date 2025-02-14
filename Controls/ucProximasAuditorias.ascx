<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProximasAuditorias.ascx.cs" Inherits="IsomanagerWeb.Controls.ucProximasAuditorias" %>

<div class="upcoming-audits">
    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
        <asp:Literal ID="litError" runat="server"></asp:Literal>
    </asp:Panel>

    <asp:Panel ID="pnlNoAuditorias" runat="server" CssClass="no-audits" Visible="false">
        <i class="bi bi-calendar-x"></i>
        <p>No hay auditorías programadas para los próximos 30 días</p>
    </asp:Panel>

    <asp:Repeater ID="rptAuditorias" runat="server">
        <ItemTemplate>
            <div class="audit-item">
                <div class="audit-date">
                    <span class="date"><%# FormatearFecha((DateTime)Eval("FechaAuditoria")) %></span>
                </div>
                <div class="audit-content">
                    <h5 class="audit-title"><%# Eval("Titulo") %></h5>
                    <span class="audit-status <%# ObtenerClaseEstado(Eval("Estado")?.ToString()) %>">
                        <%# Eval("Estado") %>
                    </span>
                    <p class="audit-description">
                        <strong>Proceso:</strong> <%# Eval("Proceso.Nombre") %><br />
                        <strong>Responsable:</strong> <%# Eval("Asignado.Nombre") %>
                    </p>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div> 