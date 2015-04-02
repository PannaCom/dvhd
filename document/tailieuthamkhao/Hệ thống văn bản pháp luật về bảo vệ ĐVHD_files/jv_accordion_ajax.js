var JVAccordionAjax = Fx.Elements.extend({
	options: {
		onActive: Class.empty,
		onBackground: Class.empty,
		display: 0,
		show: false,
		height: true,
		width: false,
		opacity: true,
		fixedHeight: false,
		fixedWidth: false,
		wait: false,
		alwaysHide: false		
	},
	initialize: function(){		
		var options, togglers, elements, container;		
		$each(arguments, function(argument, i){			
			switch($type(argument)){
				case 'object': options = argument; break;
				case 'element': container = $(argument); break;
				default:				   
					var temp = $$(argument);										
					if (!togglers) {togglers = temp;}
					else {elements = temp;}
			}
		});		
		this.togglers = togglers || [];				
		this.elements = elements || [];		
		this.container = $(container);				
		this.setOptions(options);		
		this.previous = -1;
		if (this.options.alwaysHide) this.options.wait = true;		
		if ($chk(this.options.show)){			
			this.options.display = false;
			this.previous = this.options.show;
		} 
		if (this.options.start){
			this.options.display = false;
			this.options.show = false;
		} 
		this.effects = {};
		var obj = {}; 
		if (this.options.opacity) this.effects.opacity = 'fullOpacity';
		if (this.options.width) this.effects.width = this.options.fixedWidth ? 'fullWidth' : 'offsetWidth';		
		if (this.options.height) this.effects.height = this.options.fixedHeight ? 'fullHeight' : 'scrollHeight';				
		for (var i = 0, l = this.togglers.length; i < l; i++) this.addSection(this.togglers[i], this.elements[i]);		
		this.elements.each(function(el, i){			
			if (this.options.show === i){				
				this.fireEvent('onActive', [this.togglers[i], el]);
			} else {
				if(this.options.isLoadAjax == 0){
					 for (var fx in this.effects) el.setStyle(fx, 0);
				}										
				if(this.options.isAllItem == 0){
					if(this.options.isLoadFirstArticle == 1){                        
                        obj[0] = {};
                        this.fireEvent('onActive',[this.togglers[0], this.elements[0]]);                        
                        for (var fx in this.effects) obj[0][fx] =  this.elements[0][this.effects[fx]]; 
                    }    													
				} else {								
					obj[i] = {}; 				
					this.fireEvent('onActive',[this.togglers[i], this.elements[i]]);
                    for (var fx in this.effects) obj[i][fx] =  el[this.effects[fx]];                    
				} 
			}
		}, this);	
		if(this.options.isAllItem == 1 || this.options.isLoadFirstArticle == 1){this.start(obj);}
		this.parent(this.elements);
		if ($chk(this.options.display)) this.display(this.options.display);
	},
	/*
	Property: addSection
		Dynamically adds a new section into the JVAccordion at the specified position.

	Arguments:
		toggler - (dom element) the element that toggles the JVAccordion section open.
		element - (dom element) the element that stretches open when the toggler is clicked.
		pos - (integer) the index where these objects are to be inserted within the JVAccordion.
	*/
	addSection: function(toggler, element, pos){		
		toggler = $(toggler);
		element = $(element);		
		//console.log(toggler);
		var test = this.togglers.contains(toggler);		
		var len = this.togglers.length;
		this.togglers.include(toggler);		
		this.elements.include(element);
		if (len && (!test || pos)){			
			pos = $pick(pos, len - 1);
			toggler.injectBefore(this.togglers[pos]);
			element.injectAfter(toggler);
		} else if (this.container && !test){					
			toggler.inject(this.container);
			element.inject(this.container);
		}		
		var idx = this.togglers.indexOf(toggler);
		toggler.addEvent('click', this.display.bind(this, idx));		
		if (this.options.height) element.setStyles({'padding-top': 0, 'border-top': 'none', 'padding-bottom': 0, 'border-bottom': 'none'});
		if (this.options.width) element.setStyles({'padding-left': 0, 'border-left': 'none', 'padding-right': 0, 'border-right': 'none'});
		element.fullOpacity = 1;
		if (this.options.fixedWidth) element.fullWidth = this.options.fixedWidth;
		if (this.options.fixedHeight) element.fullHeight = this.options.fixedHeight;
		element.setStyle('overflow', 'hidden');		
		if (!test){				
			for (var fx in this.effects) element.setStyle(fx, 0);
		}
		return this;
	},

	/*
	Property: display
		Shows a specific section and hides all others. Useful when triggering an JVAccordion from outside.

	Arguments:
		index - integer, the index of the item to show, or the actual element to show.
	*/

	display: function(index){		
		this.isToggle = [];		
		index = ($type(index) == 'element') ? this.elements.indexOf(index) : index;
		if ((this.timer && this.options.wait) || (index === this.previous && !this.options.alwaysHide)) return this;
		this.previous = index;
		var obj = {};			
	    if(this.options.isMultiItem == 1){//If select open mutiple item 
	       	if(this.elements[index].innerHTML == ''){	       		
	       		var articleClassId = this.elements[index].getProperty('id');
                 articleId = articleClassId.split('_')[1];//Get article Id                          
                 moduleId = this.options.moduleId;                
                 urlAjax = this.options.urlAjax+'?moduleId='+moduleId+'&articleId='+articleId+'&readMoreText='+encodeURIComponent(this.options.readMoreText);                                                                      
                 var request = new Json.Remote(urlAjax,{onRequest:function(){
                    if(this.options.jvStyle == 'jv_default_accordion'){
	                 	this.togglers[index].setStyle('padding-left','0px');
	                     var children = this.togglers[index].innerHTML;                    
	                     var wrapDiv = "<div class='ajax-loading-accordion'>"+children+"</div>";                                     
	                     this.togglers[index].innerHTML = wrapDiv; 
                    } else if(this.options.jvStyle == 'jv_button_accordion'){
                    	var children = this.togglers[index].getChildren();
                        children[0].innerHTML = "<div class='ajax-loading-accordion'>"+children.getText()+"</div>";
                    } else {
                    	this.togglers[index].getParent().getParent().addClass('ajax-loading-accordion');
                        this.togglers[index].getParent().getParent().setStyles('border-bottom:1px dashed #1A1A1A');
                        this.togglers[index].getParent().setStyles('border-bottom:0px solid');
                    } 
                 }.bind(this)}).send();                                         
                 request.addEvent('onComplete', function(jsonObj){
                       this.elements[index].setHTML(jsonObj.articleContent);                                                                                 
                 }.bind(this)).chain(function(){
                 	 if(this.options.jvStyle == 'jv_default_accordion'){
                 	    var children = this.togglers[index].getChildren().getText();                                                   
                        this.togglers[index].empty();
                        this.togglers[index].setStyle('padding-left','10px');                                          
                        this.togglers[index].innerHTML = "<span>"+children+"</span>"; 
                 	 } else if(this.options.jvStyle=='jv_button_accordion'){
                 	 	var children = this.togglers[index].getText();                      
                        this.togglers[index].empty();
                        this.togglers[index].innerHTML = "<span>"+children+"</span>";
                 	 } else {
                 	 	this.togglers[index].getParent().getParent().removeClass('ajax-loading-accordion');
                        this.togglers[index].getParent().getParent().setStyles('border-bottom:0px dashed #1A1A1A');
                        this.togglers[index].getParent().setStyles('border-bottom:1px dashed #1A1A1A');
                 	 }                                             	                        	      
                   if(this.elements[index].offsetHeight <= 0){
		                isShow = 1;
		            } else {isShow = 0;}		              
		            obj[index] = {}; 		                      
		            this.fireEvent(isShow? 'onBackground' : 'onActive',[this.togglers[index], this.elements[index]]);
		            for (var fx in this.effects) obj[index][fx] = (isShow == 0) ? 0 : this.elements[index][this.effects[fx]];                 
                    return this.start(obj);    
                 }.bind(this)); 
	       	}  else {      
			if(this.elements[index].offsetHeight > 0){
				hide = 1;
			} else {hide = 0;}
			obj[index] = {};			
			this.fireEvent(hide? 'onBackground' : 'onActive',[this.togglers[index], this.elements[index]]);
			for (var fx in this.effects) obj[index][fx] = hide ? 0 : this.elements[index][this.effects[fx]];
		    return this.start(obj);
	      }	
	    } else {	    		    	
	    	if(this.elements[index].innerHTML == ''){	    			
	    		 var articleClassId = this.elements[index].getProperty('id');
	    		 articleId = articleClassId.split('_')[1];//Get article Id	    		     		
	    		 moduleId = this.options.moduleId;
	    		 urlAjax = this.options.urlAjax+'?moduleId='+moduleId+'&articleId='+articleId+'&readMoreText='+encodeURIComponent(this.options.readMoreText);     		 		    		 		   				
	    		 var request = new Json.Remote(urlAjax,{onRequest:function(){	    		 	
                     if(this.options.jvStyle == 'jv_default_accordion'){                     	
	                     this.togglers[index].setStyle('padding-left','0px');
	                     var children = this.togglers[index].innerHTML;                    
	                     var wrapDiv = "<div class='ajax-loading-accordion'>"+children+"</div>";                                     
	                     this.togglers[index].innerHTML = wrapDiv;                    
                     } else if(this.options.jvStyle == 'jv_button_accordion'){                     	
                     	 var children = this.togglers[index].getChildren();
                         children[0].innerHTML = "<div class='ajax-loading-accordion'>"+children.getText()+"</div>";                      
                     } else {                    	            	
                     	this.togglers[index].getParent().getParent().addClass('ajax-loading-accordion');
                     	this.togglers[index].getParent().getParent().setStyles('border-bottom:1px dashed #1A1A1A');
                     	this.togglers[index].getParent().setStyles('border-bottom:0px solid');                 	               	                 	
                     }                   
	    		 }.bind(this)
	    		 }).send();   			
	    		 request.addEvent('onComplete', function(jsonObj){	    		 	     		 	
	    		 	   this.elements[index].setHTML(jsonObj.articleContent);    		 	   		 	     		 	    		 	
	             }.bind(this)).chain(function(){
	             	 if(this.options.jvStyle == 'jv_default_accordion'){	             	              	 	
	             	    var children = this.togglers[index].getChildren().getText();	             	               	      	   
	             	    this.togglers[index].empty();
	             	    this.togglers[index].setStyle('padding-left','10px');	             	   	             	   
	             	    this.togglers[index].innerHTML = "<span>"+children+"</span>";
	             	    
	             	 }else if(this.options.jvStyle =='jv_button_accordion'){	             	 	
	             	 	var children = this.togglers[index].getText();	             	 	
	             	 	this.togglers[index].empty();
	             	 	this.togglers[index].innerHTML = "<span>"+children+"</span>";	             	             	 	
	             	 } else {
	             	 	
	             	 	this.togglers[index].getParent().getParent().removeClass('ajax-loading-accordion');
	             	 	this.togglers[index].getParent().getParent().setStyles('border-bottom:0px dashed #1A1A1A');
	             	 	this.togglers[index].getParent().setStyles('border-bottom:1px dashed #1A1A1A');            	 	
	             	 }             	 	
		            this.elements.each(function(el, i){          	
                    for (var fx in this.effects) this.elements[index].setStyle(fx, 0);                                  			            
	                obj[i] = {};                                    
	                var hide = (i != index) || (this.options.alwaysHide && (el.offsetHeight >  0));	
	                this.fireEvent(hide ? 'onBackground' : 'onActive', [this.togglers[i], el]); 	               
	                for (var fx in this.effects) obj[i][fx] = hide ? 0 :  el[this.effects[fx]];	                             
		            }, this);		         
		            return this.start(obj);    
	             }.bind(this));       	    	
	       } else {       	   	   	    		
	    	this.elements.each(function(el, i){
                obj[i] = {};                                    
                var hide = (i != index) || (this.options.alwaysHide && (el.offsetHeight > 0));                                       
                for (var fx in this.effects) obj[i][fx] = hide ? 0 : el[this.effects[fx]];  
                this.fireEvent(hide ? 'onBackground' : 'onActive', [this.togglers[i], el]);              
            }, this);
             return this.start(obj); 
	       }
	    }	   
	},
	showThisHideOpen: function(index){return this.display(index);}

});
Fx.JVAccordionAjax = JVAccordionAjax;