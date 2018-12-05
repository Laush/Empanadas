
var dataArray = [];
dataArray.length = 0;
var tablaArray = [];
tablaArray.length = 0;

$('select').selectpicker();
function changeOption(select) {
    $("#cantidad").fadeIn();
    $("#btncantidad").fadeIn();
}
function agregar() {
    $("#midiv").fadeOut();
    var idgusto = $("#cboGustos").val();
    var gusto = getSelectedText("cboGustos");
    var cantidad = $("#cantidad").val();
    if (!include(dataArray, idgusto)) {
        dataArray.push({
            IdGustoEmpanada: idgusto,
            Cantidad: cantidad
        });

        tablaArray.push({
            Gusto: gusto,
            Cantidad: cantidad
        });

        createTable();
    }
    else {
        $("#midiv").fadeIn();
    }
}

function include(arr, obj) {
    var flag = false;
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].IdGustoEmpanada == obj) {
            flag = true;
        }
    }
    return flag;
}

function getSelectedText(elementId) {
    var elt = document.getElementById(elementId);

    if (elt.selectedIndex == -1)
        return null;

    return elt.options[elt.selectedIndex].text;
}

function confirmar() {

    var data = {
        IdUsuario: $("#IdUsuario").val(),
        Token: "@ViewBag.Token",
        GustosEmpanadasCantidad: dataArray
    };
    return $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        url: "/api/PedidoApi/ConfirmarGustos",
        data: JSON.stringify(data),
        success: function (Resultado) {
            alert(Resultado.Resultado + " " + Resultado.Mensaje);
            location.reload();
        },
        error: function (Resultado) {
            alert('Error al seleccionar los gustos.' + Resultado);
        }
    });
}





function createTable() {

    var tbody = '';
    var theader = '<table class="table table-condensed" style="color:white"> <tr><th>Gusto</th><th>Cantidad</th></tr>\n';
    for (var i = 0; i < tablaArray.length; i++) {
        tbody += '<tr>';
        tbody += '<td>';
        tbody += tablaArray[i].Gusto;
        tbody += '</td>'
        tbody += '<td>';
        tbody += tablaArray[i].Cantidad;
        tbody += '</td>'
        tbody += '</tr>\n';
    }

    var tfooter = '</table>';
    document.getElementById('midiv').innerHTML = theader + tbody + tfooter;
    $("#midiv").fadeIn();
}







});