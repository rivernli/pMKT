//joewa javascript library. last update on 2009/9/23

String.prototype.trim = function() { return this.replace(/^\s+|\s+$/, ''); };
//parseInt
//Date.parse();

var pg = {id:"page"}
pg.html = {id:"html"}
pg.html.setInputValue = function(id,value){
    document.getElementById(id).value = value;
}
pg.html.setChecked = function(id,isCheck){
    document.getElementById(id).checked = (isCheck==1)?true:false;
}
pg.html.getSelectedText = function(obj){
    if(obj.options.length>0){
        var idx = obj.selectedIndex;
        return obj.options[idx].text;
    }else
        return "";
}
pg.html.getSelectedValue = function(obj){
	if(obj.options.length>0){
		var idx = obj.selectedIndex;
		return obj.options[idx].value;
	}else
		return "";
}
pg.html.setSelected = function(obj,key){
    for(var i = 0; i < obj.length; i++){
        if(obj.options[i].value == key)
            obj.selectedIndex = i;
    }
}
pg.html.getSelectionTextByValue = function(id,key){
    obj = document.getElementById(id);
    for(var i = 0 ; i < obj.length; i++){
        if(obj.options[i].value == key)
            return obj.options[i].text;
    }
    return "";
}
pg.html.selectionClone = function(new_obj,target_obj){
    new_obj.options.length =0;
    for(var i=0;i < target_obj.length; i ++){
        new_obj.options[i] = new Option(target_obj.options[i].text,target_obj.options[i].value);
    }
}
pg.html.addToParent = function(newElement,parentElement,newValue){
	var _Element = document.createElement(newElement);
	parentElement.appendChild(_Element);
    if(newValue){
        if(newElement.toString().toLowerCase() == "input"){
            _Element.value = newValue;
        }else
            _Element.innerHTML = newValue;
    }
	return _Element;
}
pg.html.appendObject = function(json,Obj){
    var _ele = document.createElement(json.create);
    for(var p in json.param){
        if(typeof(json.param[p])=="object"){
            for(var su in json.param[p]){
                eval("_ele."+p+"."+su+"=json.param[p][su]");
            }
        }else{
            //eval("_ele."+ p +"= json.param."+p);
            eval("_ele."+p+" = json.param[p]");
        }
    }
    Obj.appendChild(_ele);
    return _ele;
}
pg.html.pageCtrl = function(c_page,t_records,t_pages,p_size,calling){
	var div = document.createElement("div");
	var line_size=20;
	var startIndex = Math.floor((c_page-1)/line_size) * line_size +1;
	var endIndex = startIndex + line_size-1;
	if(endIndex > t_pages)
		endIndex=t_pages;
	if(startIndex>line_size){
		div.innerHTML = "<a href='javascript:"+ calling+"("+ (startIndex-1) +");'>&lt;</a>";
	}
	for(var i =startIndex; i <= endIndex; i++){
		if(i == c_page)
			div.innerHTML += " <u>"+ i +"</u> ";
		else
			div.innerHTML += " <a href='javascript:"+ calling+"("+ i +");'>" + i +"</a> ";
	}
	if(endIndex < t_pages){
		div.innerHTML += "<a href='javascript:"+ calling+"("+ (endIndex+1) +");'>&gt;</a>";
	}
	return div;
}
pg.html.paging=function(currentPage,totalPages,everyPageSize,url,place_id){
    var o = document.getElementById(place_id);
    o.innerHTML = "";//currentPage+"  - "+totalPages;
    o.style.padding="6px 2px";
    if(totalPages <= 0) return;

    var pageStart = Math.floor((currentPage-1)/everyPageSize) * everyPageSize + 1;
    var pageEnd = pageStart+everyPageSize-1;
    if(pageEnd > totalPages)pageEnd=totalPages;

    var span;
    var d2 = pg.html.addToParent("div",o);
    if(pageStart > everyPageSize){
        cn = pg.html.appendObject({"create":"span","param":{"style":{"padding":"2px"}}}, d2);
        sp = pg.html.appendObject({"create":"span","param":{"style":{"padding":"3px","cursor":"pointer","border":"1px #ddd solid"}}},cn);
        sp.innerHTMl = "&lt;&lt;";
        sp.onclick = function(){ eval(url.replace("{page}",pageStart-1));}
    }
    for(var i=pageStart ; i<=pageEnd ; i++){
        cn = pg.html.appendObject({"create":"span","param":{"style":{"padding":"2px"}}}, d2);
        sp = pg.html.appendObject({"create":"span","param":{"style":{"padding":"3px","cursor":"pointer","border":"1px #ddd solid"}}},cn);
        sp.innerHTML = i;
        if(i==currentPage)
            sp.style.backgroundColor = "#cccccc";
        else{
            sp.onclick = function(){ eval(url.replace("{page}",this.innerHTML));}
        }
    }

    if(pageEnd < totalPages){
        cn = pg.html.appendObject({"create":"span","param":{"style":{"padding":"2px"}}}, d2);
        sp = pg.html.appendObject({"create":"span","param":{"style":{"padding":"3px","cursor":"pointer","border":"1px #ddd solid"}}},cn);
        sp.innerHTMl = "&gt;&gt;";
        sp.onclick = function(){ eval(url.replace("{page}",pageEnd + 1));}
    }
}

pg.html.showDivContent = function(id,content,e){
    if(document.readyState != "complete")
        return;
    var poxy = pg.event.getXYpos(e);
    var pageCertObj = document.getElementById(id);
	if(!pageCertObj){
		pageCertObj = document.createElement("<DIV id='"+ id+ "' style='FILTER: alpha(opacity=80); padding:1px 8px;position:absolute;visibility:visible;left:10;top:10;background-color:#C2F0FA;border:1px #689AFE solid;'>");
		var mybody=document.getElementsByTagName("body").item(0);
		mybody.appendChild(pageCertObj);
	}
	if(pageCertObj.style.visibility == "hidden")
      		pageCertObj.style.visibility = "visible";
	pageCertObj.innerHTML = content;
	pageCertObj.style.whiteSpace='nowrap'; 
	var myWidth = pageCertObj.clientWidth;
	if(myWidth + poxy[0] > document.body.clientWidth && myWidth < document.body.clientWidth)
			pageCertObj.style.left = document.body.clientWidth - (myWidth + 1);
	else
		pageCertObj.style.left = poxy[0] + 5  ;	
	pageCertObj.style.top = poxy[1] + 2 + document.documentElement.scrollTop;
}
pg.html.hideDivContent = function(id){
    var pageCertObj = document.getElementById(id);
    if(pageCertObj){
		pageCertObj.style.visibility = 'hidden';
		pageCertObj.innerHTML = "";
	}
}

pg.html.showPanel =function(e,id,height,width){
	var dyn = document.getElementById(id);
	if(!dyn){
		dyn =document.createElement("div");
		dyn.id=id;
		dyn.style.filter="alpha(opacity=90)";
		dyn.style.backgroundColor="#ffffff";
		dyn.style.position="absolute";
		dyn.style.padding="3px";
		dyn.style.border = "1px #689AFE solid";
		var htmlbody = document.getElementsByTagName("body").item(0);
		htmlbody.appendChild(dyn);
	}
	dyn.style.visibility = "visible";
	dyn.style.width = width;
	dyn.style.height = height;
        var poxy = pg.event.getXYpos(e);
	dyn.style.left = (poxy[0] + 5) +"px"  ;
	var w = parseInt(width) + parseInt(dyn.style.left);
	if(w > document.body.clientWidth){
		dyn.style.left = (document.body.clientWidth - parseInt(width)-7) +"px";
	}
	dyn.style.top = (poxy[1] + 2 + document.documentElement.scrollTop)+"px";
	return dyn;
}
pg.html.addFavorite = function(pageName){
    if (window.external){
        window.external.AddFavorite(location.href,pageName)
    }else{
        alert("Sorry! Your browser doesn't support this function.");
    }
}
pg.html.removeHTMLObject = function(id){
    var p=document.getElementById(id);
    if(p)
        p.parentNode.removeChild(p);
}
pg.html.addTextArea = function(spanName,id,parent,br,cols,rows){
    if(br)pg.html.addToParent("br",parent);
    pg.html.addToParent("span",parent,spanName).style.padding = "2px";
    return pg.html.appendObject({"create":"textarea","param":{"name":id,"id":id,"cols":(cols)?cols:40,"rows":(rows)?rows:4}},parent)
}
pg.html.addTitleAndObject = function(title,json,parent,br){
    if(br)
        pg.html.addToParent("br",parent);
    pg.html.addToParent("span",parent,title);
    return pg.html.appendObject(json,parent);
}
pg.html.addTextInput = function(spanName,id,parent,br,size){
    if(br)
        pg.html.addToParent("br",parent,"");
    s=(size)?size:"20";
    pg.html.addToParent("span",parent,spanName).style.padding = "2px";
    return pg.html.appendObject({"create":"input","param":{"type":"text","name":id,"id":id,"size":s,"value":""}},parent)
}
pg.html.addCheckBox = function(spanName,id,parent,br){
    if(br)
        pg.html.addToParent("br",parent);
    var __obj= pg.html.appendObject({"create":"input","param":{"type":"checkbox","name":id,"id":id,"value":"1"}},parent);
    pg.html.addToParent("span",parent,spanName+" &nbsp;");
    return __obj;
}
pg.html.addSelect = function(spanName,id,parent,br){
    if(br)
        pg.html.addToParent("br",parent);
    pg.html.addToParent("span",parent,"&nbsp;"+spanName);
    return pg.html.appendObject({"create":"select","param":{"name":id,"id":id}}, parent);
}
pg.html.resetTable=function(id){
    var table = document.getElementById(id);
    if(table.firstChild)
        table.removeChild(table.firstChild);
    return table;
}


pg.util = {id:"util"}
pg.util.isString = function(){
}
pg.util.addFavorite = function(name){
    if (window.external)
        window.external.AddFavorite(location.href,name);
    else
        alert("Sorry! Your browser doesn't support this function.");
}
pg.util.isMail = function(email){
    var regex = new RegExp("^([\\w\.\-])+\@(([\\w\-])+\.)+([a-zA-Z]{2,4})+$");
    if(regex.test(email.trim()))
        return true;
    else
        return false;
}
pg.util.AlphaDigi= function(str){
	var regex = new RegExp("[a-zA-Z0-9]{6,16}");
	 if(regex.test(str.trim()))
		return true;
	else
		return false;
}
pg.util.compareToday=function(cdate){
    var today = new Date();
    if(cdate.indexOf("-") >0 ){
		cdate = cdate.replace("-","/");
    }
    var d= new Date(cdate);
    if(today > d)
        return 1;
    else if(today < d)
        return -1;
    else
        return 0;
}
pg.util.getToday=function(cdate){
    var d = new Date();
    if(cdate){
        if(typeof(cdate) == "string" && cdate.indexOf("-") >0 ){
			cdate = cdate.replace("-","/");
		}
        d= new Date(cdate);
	}
    return d.getFullYear() +"-"+ (d.getMonth()+1)+"-"+ d.getDate();
}
pg.util.dateAdd=function(ymd,date,num){
    var myDate = new Date();
    if(date)
        myDate = new Date(date);
    switch(ymd){
    case "d":
        myDate.setDate(myDate.getDate() + num);
        break;
    case "m":
        myDate.setMonth(myDate.getMonth() + num);
        break;
    case "y":
        myDate.setYear(myDate.getYear() + num);
        break;
    }
    return myDate;
}
pg.event = {id:"event"}
pg.event.getXYpos = function(e){
	var XY = new Array(0,0);
	if(e==null) e=window.event;
	if(e.pageX || e.pageY){
		XY[0]=e.pageX;
		XY[1]=e.pageY;
	}else{
		XY[0]=e.clientX;
		XY[1]=e.clientY;
	}
	return XY;
}
pg.event.isReturnKey = function(e){
	var key;
	if(window.event)
		key = window.event.keyCode;
	else
		key = e.which;
	if(key == 13){
		return true;
	}else
		return false;
}

pg.xml = {id:"xml"}
pg.xml.loadXML = function(xml){
    var xmldoc;
	if(window.ActiveXObject){
        xmldoc = new ActiveXObject("Microsoft.XMLDOM");
		xmldoc.async="false";
		xmldoc.loadXML(xml);
	}else{
		var parser = new DOMParser();
		xmldoc = parser.parseFromString(xml,"text/xml");
	}
	return xmldoc;
}
pg.xml.setNode = function(dom,name,value,parent){
    var _t = dom.createElement(name);
    if(value)
        _t.text = value;
    parent.appendChild(_t);
    return _t;
}
function Ajax(){
    this.req = function(){
        var xObj;
	if(window.XMLHttpRequest) {
		try {
                    xObj = new XMLHttpRequest();
		} catch(e) {
                    xObj = false;
		}
	} else if(window.ActiveXObject) {
		try {
			xObj = new ActiveXObject("Msxml2.XMLHTTP");
		} catch(e) {
			try {
				xObj = new ActiveXObject("Microsoft.XMLHTTP");
			} catch(e) {
				xObj = false;
			}
		}
	}
	return xObj;
    }
}
Ajax.prototype.GetHttpRequest = function(url,callBack){
        var HttpRequest = this.req();
	if(HttpRequest){
		HttpRequest.onreadystatechange = function(){
			if(HttpRequest.readyState==4){if(HttpRequest.status==200){
				eval(callBack +'(HttpRequest)');
			}}
		}
		HttpRequest.open("GET",url,true);
		HttpRequest.send(null);
	}
}
Ajax.prototype.PostHttpRequest = function (url,sendDoc,resultFunction){
	var HttpRequest = this.req();
	if(HttpRequest){
		HttpRequest.onreadystatechange = function(){
    		if(HttpRequest.readyState==4){if(HttpRequest.status==200){
				eval(resultFunction +'(HttpRequest)');
			}}
		}
		HttpRequest.open("POST",url,true);
		HttpRequest.setRequestHeader("Content-Type","text/xml; charset=utf-8");
		HttpRequest.send(sendDoc);
        /*
			mxg.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
			mxg.setRequestHeader("Content-length", parameters.length);
			mxg.setRequestHeader("Connection", "close");
			mxg.send(parameters);
       */
	}
}





























if (!window.ActiveXObject) {
	Element.prototype.selectNodes = function(sXPath) {
		var oEvaluator = new XPathEvaluator();
		var oResult = oEvaluator.evaluate(sXPath, this, null, XPathResult.ORDERED_NODE_ITERATOR_TYPE, null);
		var aNodes = new Array();
		if (oResult != null) {
			var oElement = oResult.iterateNext();
			while(oElement) {
				aNodes.push(oElement);
				oElement = oResult.iterateNext();
			}
    	}
		return aNodes;
	}
	Element.prototype.selectSingleNode = function(sXPath) {
		var oEvaluator = new XPathEvaluator();
		// FIRST_ORDERED_NODE_TYPE returns the first match to the xpath.
		var oResult = oEvaluator.evaluate(sXPath, this, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
		if (oResult != null) {
			return oResult.singleNodeValue;
		} else {
			return null;
		}
	}
}


var  isIE  =   !! document.all;
if (!isIE){
     var  ex;
    XMLDocument.prototype.__proto__.__defineGetter__("xml",function (){
         try {
             return   new  XMLSerializer().serializeToString(this);
        } catch (ex){
             var  d  =  document.createElement("div");
            d.appendChild( this.cloneNode( true ));
             return  d.innerHTML;
        }
    });
    Element.prototype.__proto__.__defineGetter__( "xml" ,  function (){
         try {
             return   new  XMLSerializer().serializeToString( this );
        } catch (ex){
             var  d  =  document.createElement( "div" );
            d.appendChild( this.cloneNode( true ));
             return  d.innerHTML;
        }
    });
    XMLDocument.prototype.__proto__.__defineGetter__( "text" ,  function (){
         return   this.firstChild.textContent
    });
    Element.prototype.__proto__.__defineGetter__( "text" ,  function (){
         return   this.textContent
    });

    XMLDocument.prototype.selectSingleNode = Element.prototype.selectSingleNode = function (xpath){
         var  x = this.selectNodes(xpath)
         if ( ! x  ||  x.length < 1 ) return   null ;
         return  x[0];
    }
    XMLDocument.prototype.selectNodes = Element.prototype.selectNodes = function (xpath){
         var  xpe  =   new  XPathEvaluator();
         var  nsResolver  =  xpe.createNSResolver( this.ownerDocument  ==   null   ?
             this.documentElement :  this.ownerDocument.documentElement);
         var  result  =  xpe.evaluate(xpath,  this , nsResolver,  0 ,  null );
         var  found  =  [];
         var  res;
         while  (res  =  result.iterateNext())
            found.push(res);
         return  found;
    }
}


if (typeof (HTMLElement) != "undefined" && !window.opera) {
    Object.defineProperty(HTMLElement.prototype, "parentElement", {
        get: function() {
            if (this.parentNode == this.ownerDocument) return null;
            return this.parentNode;
        }
    });
    /*
    HTMLElement.prototype.__defineGetter__("parentElement",function(){
    if(this.parentNode == this.ownerDocument) return null;
    return this.parentNode;
    })
    */
}
