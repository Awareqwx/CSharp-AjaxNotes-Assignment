// Write your Javascript code.
$(document).ready(function(){
    $.get("/fetchdata", function(res){
        let string = "";
        for(let k in res)
        {
            string += "\
                <form>\
                    <h4 class='title'>" + res[k]["title"] + "\
                    <input type='hidden' name='id' value='" + res[k]["id"] + "'>\
                    <input type='hidden' name='title' value='" + res[k]["title"] + "'>\
                    <a href='/deletedata/" + res[k]["id"] + "'>Delete</a><br/>\
                    <textarea name='text' class='desc'>" + res[k]["text"] + "</textarea>\
                </form>\
            ";
        }
        $("#notelist").html(string);
    });
    $("#newnote input[type='submit']").click(function(){
        $.post("/adddata", $("#newnote").serialize(), function(res){
            for(let i in res[0])
            {
                console.log(i + ", " + res[0][i]);
            }
            $("#notelist").append("\
                <form>\
                    <h4 class='title'>" + res[0]["title"] + "\
                    <input type='hidden' name='id' value='" + res[0]["id"] + "'>\
                    <input type='hidden' name='title' value='" + res[0]["title"] + "'>\
                    <a href='/deletedata/" + res[0]["id"] + "'>Delete</a><br/>\
                    <textarea name='text' class='desc'>" + res[0]["text"] + "</textarea>\
                </form>\
            ");
        });
        return false;
    });
});

$("#notelist").on("focusout", "form", function(){
    $.post("/editdata", $(this).serialize())
});