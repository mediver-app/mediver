var url = window.location.href.split('?')[0];
var badSite = false;

fetch('https://mediver.karmaflux.com/Home/GetRank?uri=' + encodeURIComponent(url))
  .then(
    function(response) {
      if (response.status !== 200) {
        console.log('fetch error: ' + response.status);
        return;
      }

      // Examine the text in the response
      response.json().then(function(data) {
		  badSite = data.rank < 0;
          
      if (badSite) {
        var badTag = document.createElement("div");
        badTag.classList = "mediver-bad-header";
        badTag.innerHTML = '<img src="' + chrome.runtime.getURL('/images/bad.png') + '" width="64" height="64"> MediVer experts claim this site provides unreliable data';
        document.body.style += ';margin-top: 80px !important;';
        document.body.insertBefore(badTag, document.body.firstChild);
      }
      });
    }
  )
  .catch(function(err) {
    console.log('fetch error', err);
  });
