// format date to dd/MM/yyyy
function formatDate(strDate) {
    if (strDate == undefined || strDate == null) {
        return "";
    }
    var fullDate = new Date(strDate);    
    var tmpMonth = (fullDate.getMonth() + 1).toString();
    var tmpDate = fullDate.getDate().toString();
    return (tmpDate.length == 2 ? tmpDate : "0" + tmpDate) + "/" + (tmpMonth.length == 2 ? tmpMonth : "0" + tmpMonth)
        + "/" + fullDate.getFullYear().toString();
}
function getDateIdNew(d) {
    d = d.replace(/\-/g, "");
    return d;
}

function setDefaultValue(str) {
    if (str == undefined || str == null) {
        return "";
    }
    return str;
}

function baocao_search(input, type, fromdate, todate) {
    var inputId = input.id;
    var urlTime = "&fromdate=" + fromdate + "&todate=" + todate;
    if (input.onkeyup.arguments[0].which == 13) {
        baocao_getData(inputId, type, fromdate, todate);
        $(".ui-autocomplete").hide();
        return;
    }
    
    var urlSearch = "";
    var urlGetDetails = "";
    if (type == 1) { // Theo Loai
        urlSearch = '/Home/getLoaiDVHD?keyword=';
    } else if (type == 2) { // Theo Dia Ban
        urlSearch = '/Home/searchQuanHuyen?keyword=';
    } else if (type == 3) { // Theo Doi Tuong
        urlSearch = '/Home/getCMT?keyword=';
    } else if (type == 5) { // Theo Hanh Vi Vi Pham
        urlSearch = '/Home/getHanhViVP?keyword=';
    }

    $('#' + inputId).autocomplete({
        source: urlSearch + input.value + urlTime,
        select: function (event, ui) {
            $(event.target).val(ui.item.value);           
            baocao_getData(inputId, type, fromdate, todate);
            return false;
        },
        minLength: 1
    });
}

function baocao_getData(inputId, type, fromdate, todate) {
    $("#tbResult").html("Đang tổng hợp báo cáo, xin chờ....");
    var keyword = document.getElementById(inputId).value;
    var urlGetDetails = "";
    var urlTime = "&fromdate=" + fromdate + "&todate=" + todate;
    //alert(fromdate);
    //return;
    if (type == 1) { // Theo Loai
        urlGetDetails = '/Home/getLoaiDetails?keyword=';
    } else if (type == 2) { // Theo Dia Ban
        urlGetDetails = '/Home/getQuanHuyenDetails?keyword=';
    } else if (type == 3) { // Theo Doi Tuong
        urlGetDetails = '/Home/getCMTDetails?keyword=';
    } else if (type == 4) { // Hinh Thuc Vi Pham
        urlGetDetails = '/Home/getHTVPDetails?keyword=';
    } else if (type == 5) { // Hanh Vi Vi Pham
        urlGetDetails = '/Home/getHanhViVPDetails?keyword=';
    }
    $.ajax({
        url: urlGetDetails + keyword + urlTime,
        type: 'POST',
        dataType: 'json',
        success: function (result) {
            setResult2Table(result, type);
        }
    });
}

function setResult2Table(result, type) {
    $("#countResult").text("Tổng Số Vụ Vi Phạm : " + result.length);
    var htmlContent = '';
    if (type == 1) { // Theo Loai
        htmlContent = '<tr><th style="width:22px;">Stt</th><th style="width:160px;">Tên Loài</th>'
        + '<th style="width:80px;">Tình Trạng Bảo Tồn</th><th style="width:70px;">Đơn Vị Tính</th>'
        + '<th style="width:250px;">Số Lượng Chi Tiết</th><th style="width:90px;">Trị Giá Tang Vật</th>'
		+ '<th style="width:220px;">Tên Đơn Vị Bắt Giữ</th><th style="width:110px;">Phương Thức Vận Chuyển</th>'
		+ '<th style="width:110px;">Tuyến Đường Vận Chuyển</th><th style="width:75px;">Ngày Vi Phạm</th></tr>';
        $.each(result, function (idx, q) {
            htmlContent += '<tr><td>' + (idx + 1) + '</td><td>' + setDefaultValue(q.loaidongvat) + '</td><td>' + setDefaultValue(q.tinhtrangbaoton) + '</td><td>'
                + setDefaultValue(q.donvitinh) + '</td><td>' + setDefaultValue(q.soluongchitiet) + '</td><td>' + setDefaultValue(q.trigiatangvat) + '</td><td>' 
                + setDefaultValue(q.tendonvibatgiu) + '</td><td>' + setDefaultValue(q.phuongthucvanchuyen) + '</td><td>'
                + setDefaultValue(q.tuyenduongvanchuyen) + '</td><td>' + formatDate(q.thoigianvipham) + '</td></tr>';
        });
    } else if (type == 2) { // Theo Dia Ban        
        htmlContent = '<tr><th style="width:22px;">Stt</th><th style="width:100px;">Tên Địa Bàn</th>'
        + '<th style="width:120px;">Đối Tượng Vi Phạm</th><th style="width:260px;">Địa Chỉ Thường Trú</th>'
        + '<th style="width:105px;">Hành Vi Vi Phạm</th><th style="width:150px;">Tên Loài</th>'
		+ '<th style="width:250px;">Số Lượng Chi Tiết</th><th style="width:220px;">Tên Đơn Vị Bắt Giữ</th>'
		+ '<th style="width:75px;">Ngày Vi Phạm</th></tr>';
        $.each(result, function (idx, q) {            
            htmlContent += '<tr><td>' + (idx + 1) + '</td><td>' + setDefaultValue(q.diaban) + '</td><td>' + setDefaultValue(q.hoten) + '</td><td>'
                + setDefaultValue(q.choohientai) + '</td><td>' + setDefaultValue(q.hanhvivipham) + '</td><td>' + setDefaultValue(q.loaidongvat) + '</td><td>' 
                + setDefaultValue(q.soluongchitiet) + '</td><td>' + setDefaultValue(q.tendonvibatgiu) + '</td><td>'
                + formatDate(q.thoigianvipham) + '</td></tr>';
        });
    } else if (type == 3) { // Theo Doi Tuong
        htmlContent = '<tr><th style="width:22px;">Stt</th><th style="width:120px;">Đối Tượng Vi Phạm</th><th style="width:80px;">Số CMT/Hộ Chiếu</th>'
        + '<th style="width:235px;">Địa Chỉ Thường Trú</th><th style="width:110px;">Thông Tin Thân Nhân</th>'
		+ '<th style="width:200px;">Tiền Án Tiền Sự</th><th style="width:160px;">Hành Vi Vi Phạm</th>'
		+ '<th style="width:150px;">Tên Loài</th><th style="width:150px;">Số Lượng Chi Tiết</th><th style="width:75px;">Ngày Vi Phạm</th></tr>';
        $.each(result, function (idx, q) {
            htmlContent += '<tr><td>' + (idx + 1) + '</td><td>' + setDefaultValue(q.hoten) + '</td><td>' + setDefaultValue(q.cmthochieu) + '</td><td>'
                + setDefaultValue(q.choohientai) + '</td><td>Họ tên Cha: ' + setDefaultValue(q.hotencha) + '<br/>Họ tên Mẹ: ' + setDefaultValue(q.hotenme) + '</td><td>'
                + setDefaultValue(q.tienantiensu) + '</td><td>' + setDefaultValue(q.hanhvivipham) + '</td><td>'
                + setDefaultValue(q.loaidongvat) + '</td><td>' + setDefaultValue(q.soluongchitiet) + '</td><td>' + formatDate(q.thoigianvipham) + '</td></tr>';
        });
    } else if (type == 4) { // Theo Hinh Thuc Vi Pham
        htmlContent = '<tr><th style="width:22px;">Stt</th><th style="width:70px;">Hình Thức Vi Phạm</th><th style="width:70px;">Hành Vi Vi Phạm</th>'
                + '<th style="width:100px;">Loại Vi Phạm</th><th style="width:100px;">Địa Điểm Vi Phạm</th>'
		        + '<th style="width:470px;">Mô Tả Chi Tiết</th><th style="width:150px;">Số Lượng Chi Tiết</th><th style="width:120px;">Tên Đơn Vị Bắt Giữ</th>'
                + '<th style="width:70px;">Phương Thức Vận Chuyển</th><th style="width:70px;">Tuyến Đường Vận Chuyển</th><th style="width:75px;">Ngày Vi Phạm</th></tr>';
        $.each(result, function (idx, q) {
            htmlContent += '<tr><td>' + (idx + 1) + '</td><td>' + setDefaultValue(q.hinhthucvipham) + '</td><td>' + setDefaultValue(q.hanhvivipham) + '</td><td>'
                + setDefaultValue(q.loaidongvat) + '</td><td>' + setDefaultValue(q.diaban) + '</td><td>' + setDefaultValue(q.motavipham) + '</td><td>'
                + setDefaultValue(q.soluongchitiet) + '</td><td>' + setDefaultValue(q.tendonvibatgiu) + '</td><td>'
                + setDefaultValue(q.phuongthucvanchuyen) + '</td><td>' + setDefaultValue(q.tuyenduongvanchuyen) + '</td><td>' + formatDate(q.thoigianvipham) + '</td></tr>';
        });        
    } else if (type == 5) { // Theo Hanh Vi Vi Pham
        htmlContent = '<tr><th style="width:22px;">Stt</th><th style="width:70px;">Hành Vi Vi Phạm</th><th style="width:70px;">Hình Thức Vi Phạm</th>'
                + '<th style="width:100px;">Loại Vi Phạm</th><th style="width:100px;">Địa Điểm Vi Phạm</th>'
		        + '<th style="width:470px;">Mô Tả Chi Tiết</th><th style="width:150px;">Số Lượng Chi Tiết</th><th style="width:120px;">Tên Đơn Vị Bắt Giữ</th>'
                + '<th style="width:70px;">Phương Thức Vận Chuyển</th><th style="width:70px;">Tuyến Đường Vận Chuyển</th><th style="width:75px;">Ngày Vi Phạm</th></tr>';
        $.each(result, function (idx, q) {
            htmlContent += '<tr><td>' + (idx + 1) + '</td><td>' + setDefaultValue(q.hanhvivipham) + '</td><td>' + setDefaultValue(q.hinhthucvipham) + '</td><td>'
                + setDefaultValue(q.loaidongvat) + '</td><td>' + setDefaultValue(q.diaban) + '</td><td>' + setDefaultValue(q.motavipham) + '</td><td>'
                + setDefaultValue(q.soluongchitiet) + '</td><td>' + setDefaultValue(q.tendonvibatgiu) + '</td><td>'
                + setDefaultValue(q.phuongthucvanchuyen) + '</td><td>' + setDefaultValue(q.tuyenduongvanchuyen) + '</td><td>' + formatDate(q.thoigianvipham) + '</td></tr>';
        });
    }
    $("#tbResult").html(htmlContent);
    $("#btnCreateReport").removeAttr("disabled");
    $("#btnCreateReportBanIn").removeAttr("disabled");
}

//Export Excel
function ExportExcel(keyword, type,fromdate,todate) {
    window.open("/baocao/ExportExcel?keyword=" + keyword + "&type=" + type+"&fromdate="+fromdate+"&todate="+todate, "_blank");
}

//function baocao_createPDF(inputId, type) {    
//    $.ajax({
//        url: urlCreateBaoCaoPDF,
//        data: { keyword: $("#" + inputId).val(), type: type }, //  $("#divResult").html()
//        type: 'POST',
//        success: function (result) {
//            //alert(result);
//            window.open("/Images/Report/"+result,"_blank");
//        }
//    });
//}

function baocao_createPDFBanin() {
    window.print();  
}

function searchQuanHuyen() {
    var keyword = document.getElementById('quanvipham').value;
    //alert(keyword);
    $('#quanvipham').autocomplete({
        source: '/Home/getQuanHuyen?keyword=' + keyword,
        select: function (event, ui) {
            //alert(ui.item.id);
            $(event.target).val(ui.item.value);
            //search();
            //$('#search_form').submit();
            return false;
        },
        minLength: 1
    });
}

function searchTinhThanh() {
    var keyword = document.getElementById('tinhvipham').value;
    //alert(keyword);
    $('#tinhvipham').autocomplete({
        source: '/Home/getTinhThanh?keyword=' + keyword,
        select: function (event, ui) {
            //alert(ui.item.id);
            $(event.target).val(ui.item.value);
            //search();
            //$('#search_form').submit();
            return false;
        },
        minLength: 1
    });
}
function searchLoaidDv() {
    var keyword = document.getElementById('loaidongvat').value;
    //alert(keyword);
    $('#loaidongvat').autocomplete({
        source: '/Home/getLoaiDVHD?keyword=' + keyword,
        select: function (event, ui) {
            //alert(ui.item.id);
            $(event.target).val(ui.item.value);
            //search();
            //$('#search_form').submit();
            return false;
        },
        minLength: 1
    });
}
function unicodeToNoMark(str) {
    if (str == null) return "";
    //return str;
    //input = input.toLowerCase();
    //var noMark = "a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,e,u,u,u,u,u,u,u,u,u,u,u,u,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,i,i,i,i,i,i,y,y,y,y,y,y,d,A,A,E,U,O,O,D";
    //var unicode = "a,á,à,ả,ã,ạ,â,ấ,ầ,ẩ,ẫ,ậ,ă,ắ,ằ,ẳ,ẵ,ặ,e,é,è,ẻ,ẽ,ẹ,ê,ế,ề,ể,ễ,ệ,u,ú,ù,ủ,ũ,ụ,ư,ứ,ừ,ử,ữ,ự,o,ó,ò,ỏ,õ,ọ,ơ,ớ,ờ,ở,ỡ,ợ,ô,ố,ồ,ổ,ỗ,ộ,i,í,ì,ỉ,ĩ,ị,y,ý,ỳ,ỷ,ỹ,ỵ,đ,Â,Ă,Ê,Ư,Ơ,Ô,Đ";
    //var a_n = noMark.split(',');
    //var a_u = unicode.split(',');
    //for (var i = 0; i < a_n.length; i++) {
    //    input = input.replace(a_u[i],a_n[i]);
    //}
    str = removeSpecialCharater(str);
    str = str.replace(/\s/g, '-');
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    //str = str.replace(/!|@|\$|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|\.|\:|\'| |\"|\&|\#|\[|\]|~/g, "-");
    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    str = str.replace(/^\-+|\-+$/g, "");//cắt bỏ ký tự - ở đầu và cuối chuỗi

    return str;

}
function unicodeToNoMarkCat(str) {
    if (str == null) return "";
    //return str;
    //input = input.toLowerCase();
    //var noMark = "a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,e,u,u,u,u,u,u,u,u,u,u,u,u,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,i,i,i,i,i,i,y,y,y,y,y,y,d,A,A,E,U,O,O,D";
    //var unicode = "a,á,à,ả,ã,ạ,â,ấ,ầ,ẩ,ẫ,ậ,ă,ắ,ằ,ẳ,ẵ,ặ,e,é,è,ẻ,ẽ,ẹ,ê,ế,ề,ể,ễ,ệ,u,ú,ù,ủ,ũ,ụ,ư,ứ,ừ,ử,ữ,ự,o,ó,ò,ỏ,õ,ọ,ơ,ớ,ờ,ở,ỡ,ợ,ô,ố,ồ,ổ,ỗ,ộ,i,í,ì,ỉ,ĩ,ị,y,ý,ỳ,ỷ,ỹ,ỵ,đ,Â,Ă,Ê,Ư,Ơ,Ô,Đ";
    //var a_n = noMark.split(',');
    //var a_u = unicode.split(',');
    //for (var i = 0; i < a_n.length; i++) {
    //    input = input.replace(a_u[i],a_n[i]);
    //}
    str = removeSpecialCharater(str);
    str = str.replace(/\s/g, '-');
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    //str = str.replace(/!|@|\$|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|\.|\:|\'| |\"|\&|\#|\[|\]|~/g, "-");
    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    str = str.replace(/^\-+|\-+$/g, "");//cắt bỏ ký tự - ở đầu và cuối chuỗi
    str = str.replace(/-/g, ""); //thay thế 2- thành 1-

    return str;

}
function removeSpecialCharater(input) {
    if (input == null) return "";
    //input = input.replace(/&quot;/g, '"');
    input = input.trim();
    input = input.replace(/\./g, "");
    //input = input.replace(/\,/g, "");
    input = input.replace(/\&/g, "");
    input = input.replace(/\'/g, "");
    input = input.replace(/\"/g, "");
    input = input.replace(/\;/g, "");
    input = input.replace(/\?/g, "");
    input = input.replace(/\!/g, "");
    input = input.replace(/\~/g, "");
    input = input.replace(/\*/g, "");
    input = input.replace(/\:/g, "");
    input = input.replace(/\"/g, "");
    input = input.replace("/", "");
    input = input.replace("%", "");
    input = input.replace("‘", "");
    input = input.replace("’", "");
    input = input.replace(/\"/g, "");
    input = input.replace("+", "");
    input = input.replace("“", "");
    input = input.replace("-", "_");
    input = input.replace("”", "");
    //input = input.replace(",", "");
    input = input.replace(/\,/g, "");
    //input = input.replace(".", "");

    return input;
    //.replace(",", "").replace("_", "").replace("'", "").replace("\"", "").replace(";", "").replace("”", "").replace(".", "");
}
function getDateId(sDate) {
    if (sDate==null || sDate=="") {return null;}
    sDate = sDate.replace(/\//g, "");
    //alert(sDate);
    sDate = sDate.substring(4, 8) + sDate.substring(2, 4) + sDate.substring(0, 2);
    return sDate;
}
function convertFromDateIdToDateString(sDate) {
    //sDate = sDate.replace(/\//g, "");
    //alert(sDate);
    sDate = sDate.substring(6, 8) + "/" + sDate.substring(4, 6) + "/" + sDate.substring(0, 4);
    return sDate;
}
function Login() {
        var formdata = new FormData(); //FormData object
        var name = document.getElementById("name").value;
        var pass = document.getElementById("pass").value;
        formdata.append("name", name);
        formdata.append("pass", pass);
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/users/Login');
        xhr.send(formdata);
        var content = "";
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                if (xhr.responseText == 0) {
                    alert("Sai user hoặc mật khẩu!");
                } else {
                    window.location.href = "/Home/Index";
                }
            }
        }
    }
function isEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        ///^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$./;
        return re.test(email);
}
function checkHVVP(obj, value) {
   var space = ", ";
   if (document.getElementById("hanhvivipham").value == "") space = "";
   if (obj.checked) {
       if (document.getElementById("hanhvivipham").value.indexOf(value) < 0) document.getElementById("hanhvivipham").value += space + value;
   } else {
       document.getElementById("hanhvivipham").value = document.getElementById("hanhvivipham").value.replace(", " + value, "");
       document.getElementById("hanhvivipham").value = document.getElementById("hanhvivipham").value.replace(value, "");
   }
}

function uploadAnhDongVat() {
    if (document.getElementById("anhloaidongvativ").style.display == "none") {
        $("#anhloaidongvativ").show();
    } else {
        $("#anhloaidongvativ").hide();
    }   
}
function uploadAnhDaiDien() {
    if (document.getElementById("anhdaidiendiv").style.display == "none") {
        $("#anhdaidiendiv").show();
    } else {
        $("#anhdaidiendiv").hide();
    }
}
//Image upload
function uploadProcess() {

    var formdata = new FormData(); //FormData object
    var fileInput = document.getElementById('imageFile');
    for (i = 0; i < fileInput.files.length; i++) {
        //Appending each file to FormData object
        formdata.append(fileInput.files[i].name, fileInput.files[i]);
        break;
    }
    formdata.append("filename", "");
    //showLoadingImage();
    var xhr = new XMLHttpRequest();
    xhr.upload.addEventListener("progress", function (evt) {
        if (evt.lengthComputable) {
            var progress = Math.round(evt.loaded * 100 / evt.total);
            $("#progressbar").progressbar("value", progress);
        }
    }, false);

    xhr.open('POST', '/hoso/UploadImageProcess');
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#imageShow').html("<img src=\"" + xhr.responseText + "\" width=200 height=126>");
            $('#anhdoituong').val(xhr.responseText);

        }
    }
    return false;
}

//Image upload
function uploadProcessLoaidongvat() {

    var formdata = new FormData(); //FormData object
    var fileInput = document.getElementById('imageFileloaidongvat');
    for (i = 0; i < fileInput.files.length; i++) {
        //Appending each file to FormData object
        formdata.append(fileInput.files[i].name, fileInput.files[i]);
        break;
    }
    formdata.append("filename", "");
    //showLoadingImage();
    var xhr = new XMLHttpRequest();
    xhr.upload.addEventListener("progress", function (evt) {
        if (evt.lengthComputable) {
            var progress = Math.round(evt.loaded * 100 / evt.total);
            $("#progressbarloaidongvat").progressbar("value", progress);
        }
    }, false);

    xhr.open('POST', '/hoso/UploadImageProcessLoaidongvat');
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#imageShowloaidongvat').html("<img src=\"" + xhr.responseText + "\" width=200 height=126>");
            $('#anhdongvat').val(xhr.responseText);

        }
    }
    return false;
}
function getListTinhThanh(idoption,value) {
    
    var xhr = new XMLHttpRequest();
    xhr.open('GET', ' /Home/getAllTinhThanh?keyword=');
    xhr.send();
    var content = "";
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var news = '{"news":' + xhr.responseText + '}';
            var json_parsed = $.parseJSON(news);

            $("#"+idoption).html("");
            for (var i = 0; i < json_parsed.news.length; i++) {
                if (json_parsed.news[i]) {
                    var name = json_parsed.news[i];
                    //alert(name);
                    $("#" + idoption).append("<option value='" + name + "'>" + name + "</option>");
                }
            }
            if (value != "") $("#" + idoption).val(value);
        }
    }
   
}
//Báo cáo thống kê tổng hợp
function baocaotonghop() {
    $("#tbResult").html("Đang tổng hợp báo cáo, xin chờ....");
    var formdata = new FormData(); //FormData object
    var tinhvipham = document.getElementById("tinhvipham").value;
    var fromdate = getDateIdNew(document.getElementById("fromdate").value);
    var todate = getDateIdNew(document.getElementById("todate").value);
    //var pass = document.getElementById("pass").value;
    formdata.append("tinhvipham", tinhvipham);
    formdata.append("fromdate", fromdate);
    formdata.append("todate", todate);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/baocao/baocaotonghopreport');
    xhr.send(formdata);
    var content = "";
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (xhr.responseText == 0) {
                alert("Không có dữ liệu!");
            } else {
                $("#tbResult").html(xhr.responseText);
            }
        }
    }
    $("#btnCreateReportBanIn").removeAttr("disabled");
    $("#btnCreateReport").removeAttr("disabled");

}
//Phần dành cho phân quyền user
function checkPermission(obj, value) {
    if (obj.checked) {
        if (document.getElementById("permission").value.indexOf(value) < 0) document.getElementById("permission").value += value;
    } else {
        document.getElementById("permission").value = document.getElementById("permission").value.replace(value, "");
    }
}
function checkHS() {
    if (!document.getElementById("HS").checked) {
        for (var i = 0; i <= 4; i++) {
            if (document.getElementById("HS" + i)) document.getElementById("HS" + i).checked = false;
            document.getElementById("permission").value = document.getElementById("permission").value.replace("HS" + i, "");
        }
        return;
    }
    for (var i = 0; i <= 4; i++) {
        if (document.getElementById("HS" + i)) document.getElementById("HS" + i).checked = true;
        if (document.getElementById("permission").value.indexOf("HS" + i) < 0) document.getElementById("permission").value += "HS" + i;
    }
}
function checkUS() {
    if (!document.getElementById("US").checked) {
        for (var i = 0; i <= 3; i++) {
            if (document.getElementById("US" + i)) document.getElementById("US" + i).checked = false;
            document.getElementById("permission").value = document.getElementById("permission").value.replace("US" + i, "");
        }
        return;
    }
    for (var i = 0; i <= 3; i++) {
        if (document.getElementById("US" + i)) document.getElementById("US" + i).checked = true;
        if (document.getElementById("permission").value.indexOf("US" + i) < 0) document.getElementById("permission").value += "US" + i;
    }
}
function checkBC(from, to) {
    if (!document.getElementById("BC_" + from + "_" + to).checked) {
        for (var i = from; i <= to; i++) {
            if (document.getElementById("BC" + i)) document.getElementById("BC" + i).checked = false;
            document.getElementById("permission").value = document.getElementById("permission").value.replace("BC" + i, "");
        }
        return;
    }
    for (var i = from; i <= to; i++) {
        if (document.getElementById("BC" + i)) document.getElementById("BC" + i).checked = true;
        if (document.getElementById("permission").value.indexOf("BC" + i) < 0) document.getElementById("permission").value += "BC" + i;
    }
}
function checkAll() {
    if (!document.getElementById("ALL").checked) {
        clearAll();
        return;
    }
    if (document.getElementById("permission").value.indexOf("ALL") < 0) document.getElementById("permission").value += "ALL";
    //var per = "";
    for (var i = 0; i <= 4; i++) {
        if (document.getElementById("HS" + i)) document.getElementById("HS" + i).checked = true;
        if (document.getElementById("permission").value.indexOf("HS" + i) < 0) document.getElementById("permission").value += "HS" + i;
    }
    for (var i = 1; i <= 9; i++) {
        if (document.getElementById("BC" + i)) document.getElementById("BC" + i).checked = true;
        if (document.getElementById("permission").value.indexOf("BC" + i) < 0) document.getElementById("permission").value += "BC" + i;
    }
    for (var i = 0; i <= 3; i++) {
        if (document.getElementById("US" + i)) document.getElementById("US" + i).checked = true;
        if (document.getElementById("permission").value.indexOf("US" + i) < 0) document.getElementById("permission").value += "US" + i;
    }
    document.getElementById("HS").checked = true;
    document.getElementById("US").checked = true;
    document.getElementById("BC_1_3").checked = true;
    document.getElementById("BC_4_5").checked = true;
    document.getElementById("BC_6_9").checked = true;
}
function clearAll() {
    for (var i = 0; i <= 4; i++) {
        if (document.getElementById("HS" + i)) document.getElementById("HS" + i).checked = false;
    }
    for (var i = 1; i <= 9; i++) {
        if (document.getElementById("BC" + i)) document.getElementById("BC" + i).checked = false;
    }
    for (var i = 0; i <= 3; i++) {
        if (document.getElementById("US" + i)) document.getElementById("US" + i).checked = false;
    }
    document.getElementById("permission").value = "";
    document.getElementById("HS").checked = false;
    document.getElementById("US").checked = false;
    document.getElementById("BC_1_3").checked = false;
    document.getElementById("BC_4_5").checked = false;
    document.getElementById("BC_6_9").checked = false;
}
function changePermission() {
    var groupname = $("#groups").val();
    //alert(group);
    clearAll();
    switch(groupname) {
        case "Tổng cục môi trường":
        case "Admin hệ thống":            
            document.getElementById("ALL").checked = true;
            checkAll();
            break;
        case "Cảnh sát môi trường":
        case "Hải quan":
        case "Kiểm lâm":
        case "Quản lý thị trường":
        case "Cơ quan điều tra":
        case "Nhập dữ liệu":
            document.getElementById("ALL").checked = false;
            document.getElementById("HS").checked = true;
            checkHS();
            break;
        case "Viện kiểm sát":
        case "Tòa án":
            document.getElementById("ALL").checked = false;
            document.getElementById("BC_1_3").checked = true;
            document.getElementById("BC_4_5").checked = true;
            document.getElementById("BC_6_9").checked = true;
            checkBC(1, 3);
            checkBC(4, 5);
            checkBC(6, 9);
            break;
        default:
            alert("Chọn quyền");
               
    } 
}
function selectTATS() {
    if (document.getElementById("slck").value == "1") {
        $("#tienantiensu").show();
        $("#mahosotienan").show();
    } else {
        $("#tienantiensu").hide();
        $("#mahosotienan").hide();
    }
}
function checkPrice(obj) {
    if (isNaN(obj.value))
        return 0;
    else
        return parseInt(obj.value);
}