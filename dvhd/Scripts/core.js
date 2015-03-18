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

function setDefaultValue(str) {
    if (str == undefined || str == null) {
        return "";
    }
    return str;
}

function setResult2Table(result) {
    $("#countResult").text("Tổng Số Vụ Vi Phạm : " + result.length);
    var htmlContent = '<tr><th>Tên Loài DVHD</th><th>Thời Gian Vi Phạm</th>'
        + '<th>Tỉnh</th><th>Đối Tượng Vi Phạm</th>'
        + '<th>Số CMT/Hộ Chiếu</th><th>Hành Vi Vi Phạm</th></tr>';
    $.each(result, function (idx, q) {
        htmlContent += '<tr><td>' + q.loaidongvat + '</td><td>' + formatDate(q.thoigianvipham) + '</td><td>'
            + q.tinhvipham + '</td><td>' + q.hoten + '</td><td>' + setDefaultValue(q.cmthochieu) + '</td><td>' + q.hanhvivipham + '</td></tr>';
    });
    $("#tbResult").html(htmlContent);
    $("#btnCreateReport").removeAttr("disabled");
}

function baocao_searchLoai() {
    var keyword = document.getElementById('baocao_loaiDVHD').value;
    $('#baocao_loaiDVHD').autocomplete({
        source: '/Home/getLoai?keyword=' + keyword,
        select: function (event, ui) {
            $(event.target).val(ui.item.value);
            $.ajax({
                url: '/Home/getLoaiDetails?keyword=' + ui.item.value,
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                    setResult2Table(result);
                }
            });
            return false;
        },
        minLength: 1
    });
}

function baocao_doituong() {
    var keyword = document.getElementById('baocao_cmt').value;
    $('#baocao_cmt').autocomplete({
        source: '/Home/getCMT?keyword=' + keyword,
        select: function (event, ui) {
            $(event.target).val(ui.item.value);
            $.ajax({
                url: '/Home/getCMTDetails?keyword=' + ui.item.value,
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                    setResult2Table(result);
                }
            });
            return false;
        },
        minLength: 1
    });
}

function baocao_searchTinh() {
    var keyword = document.getElementById('baocao_tinh').value;
    $('#baocao_tinh').autocomplete({
        source: '/Home/getTinh?keyword=' + keyword,
        select: function (event, ui) {
            $(event.target).val(ui.item.value);
            $.ajax({
                url: '/Home/getTinhDetails?keyword=' + ui.item.value,
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                    setResult2Table(result);
                }
            });
            return false;
        },
        minLength: 1
    });
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
        xhr.open('POST', '/User/Login');
        xhr.send(formdata);
        var content = "";
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                if (xhr.responseText == 0) {
                    alert("Sai user hoặc mật khẩu!");
                } else {
                    window.location.href = "/Admin/Index";
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
   if (obj.checked) {
       if (document.getElementById("hanhvivipham").value.indexOf(value) < 0) document.getElementById("hanhvivipham").value += ", " + value;
   } else {
       document.getElementById("hanhvivipham").value = document.getElementById("hanhvivipham").value.replace(", " + value, "");
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