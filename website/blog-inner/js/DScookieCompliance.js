if (typeof jQuery == 'undefined') { var script = document.createElement('script'); script.src = "https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"; var head = document.getElementsByTagName('head')[0], done = false; script.onload = script.onreadystatechange = function () { if (!done && (!this.readyState || this.readyState == 'loaded' || this.readyState == 'complete')) { done = true; successmain(); script.onload = script.onreadystatechange = null; head.removeChild(script); } }; head.appendChild(script); } else { $(document).ready(function () { successmain() }) } function scss(url) { var t = document.createElement("link"); t.type = "text/css"; t.href = url; t.rel = "stylesheet"; document.getElementsByTagName("head")[0].appendChild(t); } function sjs(u) { var script = document.createElement('script'); script.src = u; var head = document.getElementsByTagName('head')[0]; head.appendChild(script); } function successmain() { scss("http://www.dotsquares.com/assets/wp-content/themes/dotsquares/content/css/dotsquares_cookiecompliance.css"); if (getCookie("DScookieCompliance") == null || getCookie("DScookieCompliance") == false) { $("body").append('<div id="dotsquares"></div>'); $("#dotsquares").html(SetDotsquaresHtml()); $("#dotsquares #cookie-allow").click(function () { setCookie("DScookieCompliance", "true"); $("#dotsquares #cookie-down").slideUp(500); $("#dotsquares").animate({ top: "-175px" }, 650); }); $("#dotsquares #dotsquares-tabs-1-link").click(function () { tabshow(1); }); $("#dotsquares #dotsquares-tabs-2-link").click(function () { tabshow(2); }); $("#dotsquares #dotsquares-tabs-3-link").click(function () { tabshow(3); }); $("#dotsquares #dotsquares-tabs-4-link").click(function () { tabshow(4); }); $("#dotsquares #dotsquares-tabs-5-link").click(function () { tabshow(5); }); $("#dotsquares #cookie-more").click(function () { if ($("#dotsquares #cookie-down").css("display") == "none") { $("#dotsquares #cookie-down").slideDown(650, function () { tabshow(1); $("#dotsquares #cookie-more").html("Hide Details"); }); } else { $("#dotsquares #cookie-down").slideUp(650, function () { $("#dotsquares #cookie-more").html("Show Details"); }); } }); var myWidth = 0, myHeight = 0; if (typeof (window.innerWidth) == 'number') { myWidth = window.innerWidth; myHeight = window.innerHeight; } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) { myWidth = document.documentElement.clientWidth; myHeight = document.documentElement.clientHeight; } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) { myWidth = document.body.clientWidth; myHeight = document.body.clientHeight; } $("#dotsquares").css("left", myWidth - 194 + "px"); $("#dotsquares #cookie-top").slideDown(650); } } function tabshow(tid) { for (var i = 1; i <= 5; i++) { if (parseInt(tid) == parseInt(i)) { $("#dotsquares #dotsquares-tabs-" + i).slideDown("slow"); $("#dotsquares #dotsquares-tabs-" + i + "-link").addClass("active"); } else { $("#dotsquares #dotsquares-tabs-" + i).hide(); $("#dotsquares #dotsquares-tabs-" + i + "-link").removeClass("active"); } } } function setCookie(key, value) { var expires = new Date(); expires.setTime(expires.getTime() + 31536000000); document.cookie = key + '=' + value + ';expires=' + expires.toUTCString(); } function getCookie(key) { var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)'); return keyValue ? keyValue[2] : null; } function SetDotsquaresHtml() { var htmlbody = ""; return htmlbody += '<div id="cookie-top" style="display:none;"><div class="cookie-logo"><a href="javascript:;"><img src="http://www.dotsquares.com/assets/wp-content/themes/dotsquares/content/images/cookie-logo.png" alt="" border="0" /></a></div><div class="cookie-top-content">To make this website work better ensure that blocked cookies are allowed.</div><div class="cookie-top-content"><a href="javascript:;" id="cookie-allow"><img src="http://www.dotsquares.com/assets/wp-content/themes/dotsquares/content/images/cookie-allow-btn.png" alt="" border="0" /></a></div><div class="cookie-top-content"><a href="javascript:;" id="cookie-more">Show Details</a></div></div><div id="cookie-down" style="display:none;"><div class="cookie-btn-zone" ><ul><li><a href="javascript:;" id="dotsquares-tabs-1-link">Know About Cookies</a><div class="cookie-btn-content" id="dotsquares-tabs-1" style="display:none;">Cookies are very small files that are stored in a user&rsquo;s computer during net surfing; these files store information of user\'s browsing data such as password, ID, preference etc so that webpage loading becomes faster when a user visit the same pages next time. According to new Cookie law, you have to allow cookies to get the optimum surfing experience of this website. For more information on the cookies stored by this website, click each category. To remove this message and enable cookies, click Allow Cookies.<br /></div></li><li><a href="javascript:;" id="dotsquares-tabs-2-link">Compliance Cookie</a><div class="cookie-btn-content" id="dotsquares-tabs-2" style="display:none;">This cookie is placed if you click the Allow button in this message.  It tells us you have given your consent to the use of cookies on our site and stops this message from displaying.<br /><br /><strong>Cookies used:</strong> DScookieCompliance</div></li><li><a href="javascript:;" id="dotsquares-tabs-3-link">Types Of Cookies</a><div class="cookie-btn-content" id="dotsquares-tabs-3" style="display:none;"><strong>Analytical Cookies:</strong><br />With the help of these cookies we can find out traffic sources to our website along with page count, which facilitates us to measure the performance of our website at the same time, improvise on the sluggish areas. This all is attained by means of a service offered by Google Analytics.<br /><br /><strong>Third Party Cookies:</strong><br />There are numerous social media tools that we utilize on our website to increase the visitor interaction. By now if you use these platforms, their cookies possibly are set by means of our site. The said data/information may then be gathered by the companies that permit them to display advertisements on any other websites that they believe are appropriate as per your field of interest. Our website will not position these cookies on any of your device, unless you do not use such platforms.<br /><br /><strong>Functional Cookies:</strong><br />The vital site functionality is allowed by these &rsquo;functional cookies&rsquo;. These get erased automatically when the browser is closed by the visitor and also do not consist any private details.</div></li></ul></div></div>'; }