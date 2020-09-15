function insertAfter(referenceNode, newNode) {
	if (referenceNode != null) {
		referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
	}
}

var spinnerImg = chrome.runtime.getURL('/images/spinner2.gif');
var hashCode = function(s) { return s.split("").reduce(function(a,b){a=((a<<5)-a)+b.charCodeAt(0);return a&a},0); };
var getElementsByClassName = function(el, className) {
	var results = [];
	var iterateBod = function(bod) {
		if (bod.classList && bod.classList.contains(className)) {
			results.push(bod);
		}
		
		if (bod.childNodes) {
			for (var i = 0; i < bod.childNodes.length; i++)
			{
				iterateBod(bod.childNodes[i]);
			}
		}
	};
	
	iterateBod(el);
	return results;
}

function checkPage(link, div) {
	fetch('https://mediver.karmaflux.com/Home/GetRank?uri=' + encodeURIComponent(link))
		.then( function(response) {
			if (response.status !== 200) {
				console.log('fetch error: ' + response.status);
				return;
			}

			// Examine the text in the response
			response.json().then(function(data) {
				var vertype = 'neutral'; 
				var score = data.rank;
				if (score == 0 || score == null) vertype = 'neutral';
				else if (score < 0) vertype = "bad";
				else vertype = "good";

				var url = new URL(link);
				if (url.hostname.endsWith('google.com') || url.hostname.endsWith('google.co.il')) {
					vertype = 'neutral';
				}
				
				var vertext = '-';
				if (window.location.toString().startsWith('https://www.google.co.il')) {
					switch(vertype) {
						case 'neutral': vertext = 'MediVer: אין דירוג'; break;
						case 'good': vertext = 'MediVer: מקור אמין'; break;
						case 'bad': vertext = 'MediVer: מקור לא אמין!'; break;
					}
				} else {
					switch(vertype) {
						case 'neutral': vertext = 'MediVer: Not enough data.'; break;
						case 'good': vertext = 'MediVer: Site is based on science.'; break;
						case 'bad': vertext = 'MediVer: Site is not based on science.'; break;
					}
				}

				var imageTag = '<img src="' + chrome.runtime.getURL('/images/' + vertype + '.png') + '" width="32" height="32" class="mediver-icon">';
				div.innerHTML = imageTag + vertext;
				div.classList += " mediver-tag-" + vertype;
			});
		})
		.catch(function(err) {
			console.log('fetch error', err);
		});
}

function processPage() {
	var linksToProcess = [];
	var searchTag = document.getElementById('search');
	if (!searchTag) return;
	var linkDivs = getElementsByClassName(searchTag, 'g');
	for (var g of linkDivs) {
		if (g.classList != 'g') continue;
		g.style += '; display: flex;';
		let mediverDiv = document.createElement("div");
		mediverDiv.classList = "mediverdiv";
		mediverDiv.innerHTML = 'MediVer: Checking...<br>'
			+ '<img src="' + spinnerImg + '" width="64" height="64" class="mediver-spinner">';
		var rc = g.getElementsByClassName('rc')[0];
		if (rc != null) {
			g.insertBefore(mediverDiv, rc);
			var rtags = rc.getElementsByClassName('r');
			if (rtags == null || rtags.length == 0) continue;
			var atags = rtags[0].getElementsByTagName('a');
			if (atags == null || atags.length == 0) continue;
			
			linksToProcess.push({
				div: mediverDiv,
				ahref: atags[0].href,
			});
		}
	}

	for (var foo of linksToProcess) {
		var ahref = foo.ahref;
		var div = foo.div;
		setTimeout(checkPage, 2000 + 3500 * Math.random(), ahref, div);
	}
}

window.addEventListener('DOMContentLoaded', processPage, false);
