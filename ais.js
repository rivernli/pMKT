        
    var tbl = "";
    
    //copy to other page.
    var selection;
    var selectionObject;
    function getAllValue(o,fil)
    {
        if(tbl=="")
        {
            alert("No tbl provided");
            return;
        }
        selection = pg.html.showPanel(null,"selection","300",200);
        selection.style.display = "none";
        selection.style.whiteSpace='nowrap'; 
        selection.style.overflow = "auto";
        selection.innerHTML = "";
        
        selectionObject = document.getElementById(o);
        AIS.filtering.getAllSelection(tbl,fil,setValue);
        return;
    }
    
    function getValue(o,fil)
    {
        if(tbl=="")
        {
            alert("No tbl provided");
            return;
        }
        selection = pg.html.showPanel(null,"selection","auto","auto");
        selection.style.display = "none";
        selection.style.whiteSpace='nowrap'; 
        selection.style.overflow = "auto";
        selection.innerHTML = "";
        
        selectionObject = o;
        if(o.value.trim() != "")
            AIS.filtering.getSelection(tbl,fil,o.value,setValue);
        return;
    }
    function setValue(obj)
    {
        var lst = eval(obj);
        selection.innerHTML = "";
        var cls = document.createElement("div");
        cls.innerHTML = "Close";
        cls.style.backgroundColor = "blue";
        cls.style.color = "#ffffff";
        cls.style.padding = "1px 2px";
        cls.style.cursor="pointer";
        cls.onclick = function(){ selection.style.display = "none";}
        selection.appendChild(cls);
        for(var i=0; i < lst.length; i++){
            var div = document.createElement("div");
            div.innerHTML = lst[i];
            div.style.cursor = "pointer";
            div.onclick = function()
            {
                selectionObject.value = this.innerText;
                selection.innerHTML = "";
                selection.style.display = "none";   
            }
            selection.appendChild(div);
        }
        selection.style.display = "block";
    }
    function OOthis(o)
    {
        var pno = document.getElementById("search_panel");
        var su_panel = document.getElementById("su_panel");
        if(o.innerHTML == "Close")
        {
            o.innerHTML = "Open";
            pno.style.display = "none";
        }else
        {
            o.innerHTML = "Close";
            pno.style.display = "block";
        }
        if(su_panel)
            su_panel.style.display = pno.style.display;
    }
    //end of copy
    
    
    function trColor(line)
    {
        //ec = line.style.backgroundColor;
        line.style.backgroundColor = (line.style.backgroundColor== "")?"#dddddd":"";
    }