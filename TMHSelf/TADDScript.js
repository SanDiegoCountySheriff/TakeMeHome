var typeAheadData =
{
    keyStrokes: "",
    focusDDLId: "",
    ResetOnNewDDLRequest: function (id) {
        if (this.focusDDLId != id) {
            this.focusDDLId = id; this.keyStrokes = "";
        }
    }
};

function TADD_OnKeyDown(tb) {
    if (event.ctrlKey)
        return;

    typeAheadData.ResetOnNewDDLRequest(tb.id);

    switch (event.keyCode) {

        case 38: //Up arrow
        case 40: //Down arrow
            typeAheadData.keyStrokes = "";
            return;

        case 27: //Esc, By pressing the Esc key, the user can start over without having to Backspace all entered letters.


            typeAheadData.keyStrokes = "";
            window.status = "";
            return;

        case 13: //Enter
        case 9: //Tab
            typeAheadData.keyStrokes = "";
            window.status = "";
            tb.fireEvent("onchange");
            return;

        case 8: //Backspace
            if (typeAheadData.keyStrokes.length > 0) {
                typeAheadData.keyStrokes = typeAheadData.keyStrokes.substr(0,
        typeAheadData.keyStrokes.length - 1);
            }
            event.cancelBubble = true;
            event.returnValue = false;
            break;

        default:
            var c = '';
            if ((event.keyCode >= 96) && (event.keyCode <= 105)) //Numbers 0-9
            {
                c = (event.keyCode - 96).toString();
            }
            else {
                c = String.fromCharCode(event.keyCode).toLowerCase();
            }
            if (c != null) {
                typeAheadData.keyStrokes += c;
            }
            event.cancelBubble = true;
            event.returnValue = false;
            break;
    }
    if (TADD_SelectItem(typeAheadData) == false) {
        typeAheadData.keyStrokes = "";
        window.status = "Not found";
    }
    else {
        tb.fireEvent("onchange");
        window.status = "KeyStrokes: " + typeAheadData.keyStrokes;
    }
}

function TADD_SelectItem(typeAheadData) {
    var ddl = document.getElementById(typeAheadData.focusDDLId);
    ddl.selectedIndex = -1;
    if (typeAheadData.keyStrokes.length > 0) {
        for (i = 0; i < ddl.options.length; i++) {
            if ((ddl.options[i].text.length >= typeAheadData.keyStrokes.length)
      && (ddl.options[i].text.substr(0,
      typeAheadData.keyStrokes.length).toLowerCase() == typeAheadData.keyStrokes)) {
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    return false;
}


