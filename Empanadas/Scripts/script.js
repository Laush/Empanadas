﻿
$(document).ready(function () {

    $("#Confirmar").click(function () {

        var InvitacionPedidoGustoEmpanadaUsuario = {
            GustoEmpanada: getSelectedText("cboGustos")
        };
        $.ajax({
            url: "/api/PedidoApi/ConfirmarGustos",
            type: "POST",
            data: JSON.stringify(InvitacionPedidoGustoEmpanadaUsuario),
            contentType: "application/json; charset=utf-8",
            success: function (data) { alert("ok") },
            error: function (x, y, z) { alert(x + y + z) }
        });

    })










});