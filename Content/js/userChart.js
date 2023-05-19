

$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/DangNhaps/UserChart',
        data: {},
        contentType: "application/json;charset=utf-8",
        dataType: 'json',
        success: function (res) {
            successFunc(res);
            console.log(res);
        },
        error: function (errormessage) {
            alert("error");
            console.log(errormessage.responseText);
        }
    });
});



var mob = {
    init: function () {
        mob.regEvents();
    },
    regEvents: function () {
        
    }
}
mob.init();

function successFunc(jsondata) {
    //var jsonData = jQuery.parseJSON(jsondata);

    var name = ['x'];
    var Tongngay = ['Tongngay'];
    var soDonHang = ['soDonHang'];
    var soTienMua = ['soTienMua'];
    jsondata.forEach(function (e) {
        Tongngay.push(e.Tongngay);
        soDonHang.push(e.soDonHang);
        soTienMua.push(e.soTienMua);
        name.push(e.name);
    })
    console.log(Tongngay);
    console.log(soDonHang);
    console.log(soTienMua);
    console.log(name);
    
    var chart = c3.generate({
        bindto: '#userChart',
        data: {
            //json: jsondata,
            //keys: {
            //    value: ['Tongngay', 'soDonHang', 'soTienMua'],
            //},
            x: 'x',
            columns: [
                    name,
                    Tongngay,
                    soDonHang,
                    soTienMua
                ],
            type: 'bar',
        },
        axis: {
            x: {
                type: 'category', // this needed to load string x value
                label: {
                    text: 'Biểu đồ đánh giá khách hàng tiềm năng',
                    position: 'outer-center'
                }
                
            }
        },
        color: {
            Tongngay: '#ff0000',
            soDonHang: '#00ff00',
            soTienMua: '#0000ff'
        },
        labels: true
    });

    chart.data.names(
        {
            Tongngay: 'Số ngày mua hàng gần nhất (tính tới hiện tại)',
            soDonHang: 'Số đơn hàng đã đặt',
            soTienMua: 'Tổng tiền đã chi tại shop (đơn vị: chục triệu VNĐ)'
        });
}
