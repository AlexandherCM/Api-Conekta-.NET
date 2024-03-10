<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="OxxoPay.aspx.cs" Inherits="Api_OxxoPay.Views.OxxoPay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../dist/styles.css" />
	<link href="file:///Users/lizbethgamez/Downloads/oxxopay-payment-stub-master/demo/css" rel="stylesheet"/>
</head>

<body>

    <form id="form1" runat="server">
        <div>
            <asp:Label ID="UserID" runat="server" Text="User ID"></asp:Label>
            <br />
            <asp:TextBox ID="txtUserID" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="PrincipalUser" runat="server" Text="Principa"></asp:Label>
            <br />
            <asp:CheckBox ID="chbxPrincipal" runat="server" />
            <br />
        </div>
        <div>
            <asp:Button ID="BtnSend" runat="server" Text="Button" OnClick="BtnSend_Click" />
            <asp:Button ID="btnDelete" runat="server" Text="Button" OnClick="btnDelete_Click" />
        </div>
    </form>

    <div class="opps">
        <div class="opps-header">
            <div class="opps-reminder">Ficha digital. No es necesario imprimir.</div>
            <div class="opps-info">
                <div class="opps-brand">
                    <img src="../dist/img/oxxopay_brand.png" alt="OXXOPay" />
                </div>
                <div class="opps-ammount">
                    <h3>Monto a pagar</h3>
                    <h2>$ 0,000.00 <sup>MXN</sup></h2>
                    <p>OXXO cobrará una comisión adicional al momento de realizar el pago.</p>
                </div>
            </div>
            <div class="opps-reference">
                <h3>Referencia</h3>
                <h1>0000-0000-0000-00</h1>
            </div>
        </div>
        <div class="opps-instructions">
            <h3>Instrucciones</h3>
            <ol>
                <li>Acude a la tienda OXXO más cercana. <a href="https://www.google.com.mx/maps/search/oxxo/" target="_blank">Encuéntrala aquí</a>.</li>
                <li>Indica en caja que quieres realizar un pago de servicio<strong></strong>.</li>
                <li>Dicta al cajero el número de referencia en esta ficha para que tecleé directamete en la pantalla de venta.</li>
                <li>Realiza el pago correspondiente con dinero en efectivo.</li>
                <li>Al confirmar tu pago, el cajero te entregará un comprobante impreso. <strong>En el podrás verificar que se haya realizado correctamente.</strong> Conserva este comprobante de pago.</li>
            </ol>
            <div class="opps-footnote">Al completar estos pasos recibirás un correo de <strong>Nombre del negocio</strong> confirmando tu pago.</div>
        </div>
    </div>


</body>
</html>
