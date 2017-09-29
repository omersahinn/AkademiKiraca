var this_js_script = $('script[src*=bc]'); 

var my_page_id_param = this_js_script.attr('data-page-id');   
if (typeof my_page_id_param === "undefined" ) {
   var my_page_id_param = '1';
}
var my_page_id = my_page_id_param.split(".");
var my_page_id_len = my_page_id.length;
//alert(my_page_id_len);
//alert(my_page_id["0"]);

var page_titles = { "0":{
						 "0":{
							  "title":"Anasayfa",
							  "link":"anasayfa.html"
							 }
					    },
					"1":{
						 "0":{
							  "title":"Hakkımızda",
							  "link":"anasayfa.html"
							 },
						 "1":{
							  "title":"Misyon",
							  "link":"misyon.html"
							 },
						 "2":{
							  "title":"Amaç",
							  "link":"amac.html"
							 },
						 "3":{
							  "title":"Taahütlerimiz",
							  "link":"taahutlerimiz.html"
							 },
						 "4":{
							  "title":"İletişim",
							  "link":"iletisim.html"
							 }

						},
					"2":{
						 "0":{
							  "title":"Kapsam",
							  "link":"anasayfa.html"
							 },
					     "1":{
							  "title":"Liderlik",
							  "link":"liderlik.html"
							 },
					     "2":{
							  "title":"Kişisel Gelişim",
							  "link":"meslekivekisiselgelisim.html"
							 },
					     "3":{
							  "title":"İş Mükemmelliği",
							  "link":"ismukemmelligi.html"
							 }
						}, 
					"3":{
						 "0":{
							  "title":"Faaliyetler",
							  "link":"anasayfa.html"
							 },
						 "1":{
							  "title":"Eğitimler",
							  "link":"egitimler.html"
							 },
						 "2":{
							  "title":"Proje Grupları",
							  "link":"projegruplari.html"
							 },
						 "3":{
							  "title":"Best Pratice",
							  "link":"bestpratice.html"
							 },
						 "4":{
							  "title":"Sinerji Grupları",
							  "link":"sinerjigruplari.html"
							 },
						 "5":{
							  "title":"E-Learning",
							  "link":"elearning.html"
							 },
						 "6":{
							  "title":"MBA",
							  "link":"mba.html"
							 }
	 
						}, 
					"4":{
						 "0":{
							  "title":"İç Eğitimcilerimiz",
							  "link":"anasayfa.html"
							 },
						 "1":{
							  "title":"Eğitmen Sistemi",
							  "link":"icegitmensistemi.html"
							 },
						 "2":{
							  "title":"İç Eğitimcilerimiz",
							  "link":"icegitimcilerimiz.html"
							 }
						}, 
					"5":{
						 "0":{
							  "title":"Haberler",
							  "link":"anasayfa.html"
							 }
						}, 
					"6":{
						 "0":{
							  "title":"Benim Akademim",
							  "link":"benimakademim.html"
							 }
						}	
				};
//alert(page_titles["1"]["0"]["title"]);

	document.write("<a class='bc_link1' href='" + page_titles["0"]["0"]["link"] + "'>" + page_titles["0"]["0"]["title"] + "</a>");

if (my_page_id_len>=1) {
	
	document.write("<a class='bc_link2' href='" + page_titles[my_page_id["0"]]["0"]["link"] + "'>" + page_titles[my_page_id["0"]]["0"]["title"] + "</a>");
}

if (my_page_id_len>=2) {
	document.write("<a class='bc_link3' href='" + page_titles[my_page_id["0"]][my_page_id["1"]]["link"]+ "'>" + page_titles[my_page_id["0"]][my_page_id["1"]]["title"] + "</a>");
}

