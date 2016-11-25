!function(e){e.fn.flexisel=function(i){var n,t=e.extend({visibleItems:7,animationSpeed:200,autoPlay:!1,autoPlaySpeed:3e3,pauseOnHover:!0,setMaxWidthAndHeight:!1,enableResponsiveBreakpoints:!0,flipPage:!1,clone:!0,responsiveBreakpoints:{portrait:{changePoint:480,visibleItems:1},landscape:{changePoint:640,visibleItems:2},tablet:{changePoint:768,visibleItems:3}}},i),s=e(this),l=e.extend(t,i),a=!0,o=l.visibleItems,r=s.children().length,c=[],f={init:function(){return this.each(function(){f.appendHTML(),f.setEventHandlers(),f.initializeItems()})},initializeItems:function(){var i=s.parent(),t=(i.height(),s.children());f.sortResponsiveObject(l.responsiveBreakpoints);var a=i.width();n=a/o,t.width(n),l.clone&&(t.last().insertBefore(t.first()),t.last().insertBefore(t.first()),s.css({left:-n})),s.fadeIn(),e(window).trigger("resize")},appendHTML:function(){s.addClass("nbs-flexisel-ul"),s.wrap("<div class='nbs-flexisel-container'><div class='nbs-flexisel-inner'></div></div>"),s.find("li").addClass("nbs-flexisel-item");var i=s.parent();if(l.setMaxWidthAndHeight){var n=e(".nbs-flexisel-item img").width(),t=e(".nbs-flexisel-item img").height();e(".nbs-flexisel-item img").css("max-width",n),e(".nbs-flexisel-item img").css("max-height",t)}if(e("<div class='nbs-flexisel-nav-left'></div><div class='nbs-flexisel-nav-right'></div>").insertAfter(i),l.clone){var a=s.children().clone();s.append(a)}},setEventHandlers:function(){var i=s.parent(),t=i.parent(),c=s.children(),d=t.find(".nbs-flexisel-nav-left"),v=t.find(".nbs-flexisel-nav-right");e(window).on("resize",function(){f.setResponsiveEvents();var t=e(i).width(),a=e(i).height();if(n=t/o,c.width(n),s.css(l.clone?{left:-n}:{left:0}),!l.clone&&o>=r)d.add(v).css("visibility","hidden");else{d.add(v).css("visibility","visible");var p=d.height()/2,h=a/2-p;d.css("top",h+"px"),v.css("top",h+"px")}}),e(d).on("click",function(){f.scrollLeft()}),e(v).on("click",function(){f.scrollRight()}),1==l.pauseOnHover&&e(".nbs-flexisel-item").on({mouseenter:function(){a=!1},mouseleave:function(){a=!0}}),1==l.autoPlay&&setInterval(function(){1==a&&f.scrollRight()},l.autoPlaySpeed)},setResponsiveEvents:function(){var i=e("html").width();if(l.enableResponsiveBreakpoints){var n=c[c.length-1].changePoint;for(var t in c){if(i>=n){o=l.visibleItems;break}if(i<c[t].changePoint){o=c[t].visibleItems;break}}}},sortResponsiveObject:function(e){var i=[];for(var n in e)i.push(e[n]);i.sort(function(e,i){return e.changePoint-i.changePoint}),c=i},scrollLeft:function(){if(s.position().left<0&&1==a){a=!1;var e=s.parent(),i=e.width();n=i/o;var t=s.children(),r=l.flipPage?i:n;s.animate({left:"+="+r},{queue:!1,duration:l.animationSpeed,easing:"linear",complete:function(){l.clone&&t.last().insertBefore(t.first()),f.adjustScroll(),a=!0}})}},scrollRight:function(){var e=s.parent(),i=e.width();n=i/o;var t=n-i,c=s.position().left+(r-o)*n-i,d=l.flipPage?i:n;if(t<=Math.ceil(c)&&!l.clone)1==a&&(a=!1,s.animate({left:"-="+d},{queue:!1,duration:l.animationSpeed,easing:"linear",complete:function(){f.adjustScroll(),a=!0}}));else if(l.clone&&1==a){a=!1;var v=s.children();s.animate({left:"-="+d},{queue:!1,duration:l.animationSpeed,easing:"linear",complete:function(){v.first().insertAfter(v.last()),f.adjustScroll(),a=!0}})}},adjustScroll:function(){var e=s.parent(),i=s.children(),t=e.width();n=t/o,i.width(n);var a=l.flipPage?t:n;l.clone&&s.css({left:-a})}};return f[i]?f[i].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof i&&i?void e.error('Method "'+method+'" does not exist in flexisel plugin!'):f.init.apply(this)}}(jQuery);