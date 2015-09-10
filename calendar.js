// JavaScript Document
		function calendar(obj){
		
		    var cal = pg.html.showPanel(null,'winPanel','auto',200);
			cal.innerHTML = "";
			d = new Date();
			showcalendar(d.getFullYear(), d.getMonth(), d.getDate(), obj);
		}
		function closeCalendar(){
				var cal=document.getElementById("winPanel")
				if(cal){
						cal.innerHTML = "";
						cal.style.visibility ="hidden";
				}
		}
		function getd(obj,d){
			obj.value = d;
			closeCalendar();
		}
		
		function showcalendar(y,m,d,obj){
			if(m<0){
				y = y - 1;	m = 11;	
			}
			if(m>11){
				y = y + 1;m = 0;
			}
			var a;
			var c =0;
			var today = new Date(y,m,d);
			m ++;
			var thisDay;
			var monthDays = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
			var year = today.getYear();
			var thisDay = today.getDate();
			if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
				monthDays[1] = 29;
			var nDays = monthDays[today.getMonth()];
			firstDay = today;
			firstDay.setDate(1); // works fine for most systems
			testMe = firstDay.getDate();
			if (testMe == 2) 
				firstDay.setDate(0);
			startDay = firstDay.getDay();
			x = m - 2;
			var table = document.createElement("table");
			table.style.width = "200"; table.style.tableLayout = "fixed";table.style.emptyCells="show"; table.style.borderCollapse="collapse";
			table.bgColor = "#FAFFE3"; table.cellSpacing = "1"; table.cellPadding = "1";
			table.border = "1"; table.borderColor = "#FFB3B3";
			var tbody = document.createElement("tbody");
			table.appendChild(tbody);
			var tr = document.createElement("tr");
			tbody.appendChild(tr);

			var td = document.createElement("td");
			td.innerHTML = " &lt; ";
			td.style.cursor = "pointer";
			td.onclick = function(){showcalendar(y,x,d,obj);	}
			tr.appendChild(td);
			var td2 = document.createElement("td");
			td2.style.textAlign="center";
			td2.colSpan = "5";
			td2.innerHTML = y + "-"+ m;
			tr.appendChild(td2);
			var td3 = document.createElement("td");
			td3.style.textAlign="right";
			td3.innerHTML = " &gt; ";
			td3.style.cursor = "pointer";
			td3.onclick = function(){showcalendar(y,m,d,obj);}
			tr.appendChild(td3);
			
			var i;
			var atr = document.createElement("tr");
			for(i=0;i<startDay;i++){
				c++;
				var td1row = document.createElement("td");
				td1row.innerHTML = "&nbsp;";
				atr.appendChild(td1row);
			}
			tbody.appendChild(atr);
			var j;
			for( j=1; j<=nDays; j++){
				if(c==7)	{
					c=0;
					var atr = document.createElement("tr");
					tbody.appendChild(atr);
				}
				c++;
				var tdnrow = document.createElement("td");
				tdnrow.date = m+"/"+ j +"/"+ y;
				tdnrow.style.cursor = "pointer";
				tdnrow.onclick = function(){getd(obj,this.date);}
				if(j==thisDay){
					tdnrow.innerHTML = "<b style='color:#FFB80F' );>" + j + "</b>";
					atr.appendChild(tdnrow);
				}else{
					tdnrow.innerHTML = j;
					atr.appendChild(tdnrow);
				}
			}
			if(c<7 || c>0){
				for(i=c;i<7;i++){
					var td = document.createElement("td");
					td.innerHTML = "&nbsp;";
					atr.appendChild(td);
				}
			}
			var tr = document.createElement("tr");
			var td =document.createElement("td");
			td.colSpan = "7";
			var sp = document.createElement("span")
			sp.innerHTML = "close";
			sp.style.cursor = "pointer";
			sp.onclick =function(){closeCalendar();} 
			td.appendChild(sp);
			tr.appendChild(td);
			tbody.appendChild(tr);

			var cal = document.getElementById("winPanel");
			if(cal){			
				cal.innerHTML = "";
				cal.appendChild(table);
			}
		}
