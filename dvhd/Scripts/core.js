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